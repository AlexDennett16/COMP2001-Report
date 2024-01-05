using COMP_2001_Report.Data;
using COMP_2001_Report.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace COMP_2001_Report.Controllers
{
    public static class UserRoleStorageSingleton
    {
        public static string? userRole { get; set; }
        public static string? email { get; set; }
    }


    // Login Method
    [Route("account")]
    public class AccountController() : Controller
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //API Link
                    var apiUrl = "https://web.socem.plymouth.ac.uk/COMP2001/auth/api/users";

                    //Add username to singleton class for name checking
                    UserRoleStorageSingleton.email = model.email;

                    // Prep the String from swagger to JSON
                    var jsonContent = System.Text.Json.JsonSerializer.Serialize(model);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultJSON = await response.Content.ReadAsStringAsync();
                        var result = System.Text.Json.JsonSerializer.Deserialize<List<string>>(resultJSON);

                        string role;

                        //Show user their role once responce receieved
                        if (result[1] == "True")
                        {
                            role = "admin";
                        }
                        else if (result[1] == "False")
                        {
                            role = "user";
                        }
                        else
                        {
                            return BadRequest("User type unable to be identified");
                        }

                        UserRoleStorageSingleton.userRole = role;



                        return Ok("Successful sign-on as a " + role);
                    }
                    else
                    {
                        return BadRequest(new { Message = "Authentication failed", StatusCode = response.StatusCode });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", ErrorDetails = ex.Message });
            }
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserDbContext _dbContext;


        public UsersController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Users

        [HttpGet]
        public IActionResult Get()
        {
            string userType = UserRoleStorageSingleton.userRole;
            var users = _dbContext.Users.Where(u => u.username != null && u.username != "").Select(u => u.username).ToList();

            //null at intialisation so we must force user to login
            if (userType == null)
            {
                return BadRequest("Please Login!");
            }
            //Success, we also only return earlier defined usernames, no other sensitive info
            else if (userType == "user" || userType == "admin")
            {
                return Ok(users);
            }
            //Incase of unexpected values
            else
            {

                return BadRequest("Unknown User type");
            }

        }

        // GET api/Users/{id}
        [HttpGet("{id}")]
        public IActionResult GetUsers(int id)
        {
            string userType = UserRoleStorageSingleton.userRole;
            string userEmail = UserRoleStorageSingleton.email;
            try
            {
                var user = _dbContext.Users.Find(id);

                if (user == null)
                {
                    return NotFound();
                }

                if (userType == null || userEmail == null)
                {
                    return BadRequest("Please Login!");
                }
                else if ((userType == "user" || userType == "admin") && (userEmail == user.email))
                {
                    return Ok("Hello " + user.username + " here are you login details (this service does not support showing password, and no private password data is sent) " + "\n\nEmail: " + user.email + "\nPassword: *******");
                }
                else if (userType == "user" || userType == "admin")
                {
                    return Ok("Viewing public profile " + user.username);
                }
                
                else
                {
                    return BadRequest("Unexpected Error");
                }

                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest("This user has been archived or does not exist");
            }



        }


        // POST api/Users/{id}
        [HttpPost]
        public IActionResult Post([FromBody] UserModel userInput)
        {

            //No user tpye comparisons, anyone can make an account!
            if (userInput == null)
            {
                return BadRequest("Invalid user data");
            }

            try
            {
                var newUser = new Users
                {
                    username = userInput.username,
                    password = userInput.password,
                    email = userInput.email
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = newUser.user_id }, newUser);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/Users/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserModel userInput)
        {

            string userType = UserRoleStorageSingleton.userRole;
            string userEmail = UserRoleStorageSingleton.email;
        
            try
            {

                var existingUser = _dbContext.Users.Find(id);

                if (userEmail != existingUser.email)
                {
                    return BadRequest("You're not logged into this account, and so cannot edit it!");
                }

                existingUser.username = userInput.username;
                existingUser.password = userInput.password;
                existingUser.email = userInput.email;


                _dbContext.SaveChanges();

                return NoContent();

            }
            catch (Exception)
            {
                return BadRequest("This user has been archived or does not exist");
            }

        }

        // api/ArchiveUsers/{id}
        [HttpDelete("Archive-User/{id}")]
        public IActionResult Archive(int id)
        {
            string userType = UserRoleStorageSingleton.userRole;

            try
            {
                if (userType != "admin")
                {
                    return BadRequest("You do not have the priveldges to do this action!");
                }


                var userToDelete = _dbContext.Users.Find(id);

                
                var archiveUser = new Archive_User
                {
                    archive_user_id = userToDelete.user_id,
                    username = userToDelete.username,
                    email = userToDelete.email,
                    password = userToDelete.password,
                };

                userToDelete.username = null;
                userToDelete.password = null;
                userToDelete.email = null;

                _dbContext.SaveChanges();

                _dbContext.ArchiveUser.Add(archiveUser);

                _dbContext.SaveChanges();

                return Ok("Successfully Archived User!");
            }
            catch (Exception)
            {
                return BadRequest("Attempting to archive an already archived or non-existant user");
            }
        }

        // GET: api/ArchiveUsers
        [HttpGet("Archive")]
        public IActionResult GetArchived()
        {
            string userType = UserRoleStorageSingleton.userRole;
            var users = _dbContext.ArchiveUser.Where(u => u.username != null && u.username != "").Select(u => new { u.username, u.archive_user_id }).ToList();

            //null at intialisation so we must force user to login
            if (userType == null)
            {
                return BadRequest("Please Login!");
            }
            //Success, we also only return earlier defined usernames, no other sensitive info
            else if (userType == "user" || userType == "admin")
            {
                return Ok(users);
            }
            //Incase of unexpected values
            else
            {
                return BadRequest("Unknown User type");
            }

        }

        //api/ArchiveUsers/{id}
        [HttpPost("Unarchive-User/{id}")]
        public IActionResult Unarchive(int id)
        {
            string userType = UserRoleStorageSingleton.userRole;

            try
            {
                if (userType != "admin")
                {
                    return BadRequest("You do not have the priveldges to do this action!");
                }

                var archivedUser = _dbContext.ArchiveUser.Find(id);

                var toUnarchive = _dbContext.Users.Find(id);

                

                toUnarchive.username = archivedUser.username;
                toUnarchive.password = archivedUser.password;
                toUnarchive.email = archivedUser.email;
                _dbContext.SaveChanges();

                _dbContext.ArchiveUser.Remove(archivedUser);

                _dbContext.SaveChanges();

                return Ok("Successfully Unarchived User");
            }
            catch (Exception)
            {
                return BadRequest("Attempting to unarchive a non-existant archive");
            }
        }
    }
}


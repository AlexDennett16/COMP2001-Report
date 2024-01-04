using COMP_2001_Report.Data;
using COMP_2001_Report.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP_2001_Report.Controllers
{
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
            var users = _dbContext.Users.Where(u => u.username != null && u.username != "").ToList();

            return Ok(users);
        }

        // GET api/Users/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var user = _dbContext.Users.Find(id);

                if (user == null)
                {
                    return NotFound();
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

            try
            {

                var existingUser = _dbContext.Users.Find(id);


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

        // DELETE api/Users/{id}
        [HttpDelete("Archive-User/{id}")]
        public IActionResult Archive(int id)
        {
            try
            {
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

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Attempting to archive an already archived or non-existant user");
            }
        }

        [HttpPost("Unarchive-User/{id}")]
        public IActionResult Unarchive(int id)
        {
            try
            {
                var archivedUser = _dbContext.ArchiveUser.Find(id);

                var toUnarchive = _dbContext.Users.Find(id);

                toUnarchive.username = archivedUser.username;
                toUnarchive.password = archivedUser.password;
                toUnarchive.email = archivedUser.email;
                _dbContext.SaveChanges();

                _dbContext.ArchiveUser.Remove(archivedUser);

                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Attempting to unarchive a non-existant archive");
            }
        }
    }
}


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using COMP_2001_Report.Data;
using static System.Net.WebRequestMethods;
using COMP_2001_Report.Models;

namespace COMP_2001_Report.Controllers
{
    // AccountController.cs
    [Route("account")]
    public class AccountController : Controller
    {

        private readonly AuthUser _LoginAPI;

        public AccountController(AuthUser loginAPI)
        {
            _LoginAPI = loginAPI;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            try
            {
                // Validate the model if needed
                using (HttpClient client = new HttpClient())
                {
                    // Send the user input to the external API
                    var apiUrl = "https://web.socem.plymouth.ac.uk/COMP2001/auth/api/user";
                    var result = await _LoginAPI.SendPostRequestAsync(apiUrl, model.email, model.password);

                    // Process the result from the external API
                    if (result.Length == 2)
                    {
                        var verificationStatus = result[0];
                        var additionalInfo = result[1];

                        // Customize the response based on the verification status
                        return verificationStatus == "Verified"
                            ? Ok(new { Message = "Authentication successful", AdditionalInfo = additionalInfo })
                            : BadRequest(new { Message = "Authentication failed", AdditionalInfo = additionalInfo });
                    }
                    else
                    {
                        // Handle unexpected result format
                        return BadRequest(new { Message = "Unexpected result from external API" });
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, new { Message = "Internal server error", ErrorDetails = ex.Message });
            }
        }

    }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }

    }

}

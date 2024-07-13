using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using UserAccessor.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserAccessor.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserAccessorDbController(ILogger<UserAccessorDbController> logger, DaprClient daprClient) : ControllerBase
    {
        private readonly ILogger<UserAccessorDbController> _logger = logger;
        private readonly DaprClient _client = daprClient;

        [HttpPost("/preferences")]
        public async Task<ActionResult<UserEmailPreferenceResponse>> SetPreferences(UserEmailPreferenceRequest userEmailpreferences)
        {
            UserEmailPreferenceResponse response = new UserEmailPreferenceResponse()
            {
                Id = 0,
                Email = userEmailpreferences.Email,
                Preferences = userEmailpreferences.Preferences
            };

            try
            {
                if (userEmailpreferences is null)
                {
                    return BadRequest("couldn't set your preferences, sorry");
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SetPreferences error: " + ex.Message);
                return Ok(NotFound("There was a problem"));

            }
        }

        [HttpGet("/LatestNews")]
        public async Task<ActionResult<UserEmailResponse>> SendLatestNews(UserEmailRequest userEmail)
        {
            try
            {
                UserEmailResponse response = new UserEmailResponse()
                {
                    Id = 0,
                    Email = userEmail.Email,
                    
                };
                return Ok(userEmail);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("/DeleteUser")]
        public async Task<ActionResult<UserEmailResponse>>? DeletePreferences(UserEmailRequest userEmail)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /////////////////////////////////////////////Internal use functions
        [HttpGet("/getPreferences")]
        private async Task<List<string>> fetchPreferences(UserEmailRequest userEmail)
        {
            try
            {


                //connect DB to get the preferences of the user
                List<string> fakeList = new List<string>
                {
                    "science", "sport"
                };


                return await Task.FromResult(new List<string> { "science", "sport"});

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("/EmailExist")]
        private async Task<bool> EmailExistInDB(UserEmailRequest userEmail)
        {
            try
            {
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}

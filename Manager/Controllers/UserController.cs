using Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Dapr.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ILogger<UserController> logger, Dapr.Client.DaprClient daprClient) : ControllerBase
    {
        private readonly ILogger <UserController> _logger = logger;
        private readonly DaprClient _daprclient = daprClient;

        [HttpPost("SetPreferences")]
        public async Task<ActionResult<UserEmailPreferenceResponse>> SetPreferences([FromQuery]UserEmailPreferenceRequest userValuesRequest)
        {
            try
            {
                var response = await _daprclient.InvokeMethodAsync<UserEmailPreferenceRequest, UserEmailPreferenceResponse?>("useraccessor", "/preferences", userValuesRequest);
                if (response == null)
                {
                    return BadRequest(response);
                }
                return Ok("Your Preferences were set");
            }
            catch (Exception ex)
            {
                _logger.LogError("SetPreferences error: {ex}", ex.Message);
                return Problem("There was a problem setting the user preferences");
            }
        }



        [HttpGet("/LatestNews")]
        public async Task<ActionResult<UserEmailResponse>> FetchLatestNews([FromQuery]UserEmailRequest userEmail)
        {
            try
            {
                //Brings the preferences of the specific user
                UserPreferencesResponse responsePreferences = await _daprclient.InvokeMethodAsync<UserEmailRequest, UserPreferencesResponse>(HttpMethod.Get, "useraccessor", "/Preferences", userEmail);
                //Requests latest news from News.io
                List<string> latestNews = await _daprclient.InvokeMethodAsync<UserPreferencesResponse, List<string>>(HttpMethod.Get, "newsapi", "/LatestNews", responsePreferences);
                //Send the Preferences and the latest news to gemini AI api.
                
                //Send the latest News Based on preferences to Email of the User
                
                
                return Ok("The latest news were sent to your Email!");
                
            }

            catch (Exception ex)
            {
                _logger.LogError("LatestNews error: {ex}", ex.Message);
                return Problem("The news could not be sent to your email at this time");
            }
        }


        [HttpDelete("/DeleteUser")]
        public async Task<ActionResult<bool>> DeleteUser([FromQuery]UserEmailRequest userEmail)
        {
            try
            {
                var response = await _daprclient.InvokeMethodAsync<UserEmailRequest, bool>(HttpMethod.Delete, "useraccessor", "/User", userEmail);
                if (response == false)
                    return Ok("The user could not be deleted");
                return Ok("user deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteUser error: " + ex.Message);
                return Problem("User was not deleted from DB");
            }
        }

    }
}

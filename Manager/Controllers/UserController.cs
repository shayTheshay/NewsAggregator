using Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Dapr.Client;
using System.Threading;
using System.ComponentModel.DataAnnotations;

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
                var response = await _daprclient.InvokeMethodAsync<UserEmailRequest, UserEmailResponse?>(HttpMethod.Get, "useraccessor", "/LatestNews", userEmail);
                return Ok("The latest news were sent to your Email!");
            }

            catch (Exception ex)
            {
                _logger.LogError("LatestNews error: {ex}", ex.Message);
                return Problem("The news could not be sent to your email at this time");
            }
        }


        [HttpDelete("/DeleteUser")]
        public async Task<ActionResult<UserEmailResponse>> DeleteUser([FromQuery]UserEmailRequest userEmail)
        {
            try
            {
                var response = await _daprclient.InvokeMethodAsync<UserEmailRequest, UserEmailResponse?>(HttpMethod.Delete, "useraccessor", "/DeleteUser", userEmail);
                if (response == null)
                    return Ok("There was no User to begin with");
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

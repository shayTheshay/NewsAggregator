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

        [HttpGet("SetPreferences")]
        public async Task<ActionResult<string>> SetPreferences(string str)//UserEmailPreferenceRequest requestUserValues)
        {
            try
            {
                var response = await _daprclient.InvokeMethodAsync<List<string>>(HttpMethod.Get, "useraccessor", "/preferences");
                

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("SetPreferences error: " + ex.Message);
                return Problem("There was a problem: " + ex.Message);

            }
        }



        [HttpGet("/LatestNews")]
        public string FetchLatestNews(string userMail)
        {
            try
            {
                return "The latest news were sent to your Email!";
            }

            catch (Exception ex)
            {
                _logger.LogError("LatestNews error: " + ex.Message);
                return ex.Message;
            }
        }

       /*
            setPreferences -> Accessor -> DB -> Accessor -> Manager ->Ok ro created which is Okay
            UpdatePreferences -> Accessor -> DB -> Accessor -> Manager -> OK

            fetchLatestNews -> Accessor -> DB ->Accessor ->Manager -> 
        */



        [HttpDelete("/DeleteUser")]
        public string DeleteUser(string userMail)
        {
            try
            {
                return "User Deleted!";
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteUser error: " + ex.Message);
                return ex.Message;
            }

        }


 

    }
}

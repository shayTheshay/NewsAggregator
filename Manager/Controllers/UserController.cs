using Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Dapr.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger <UserController> _logger;
        private readonly DaprClient _client;

        public UserController(ILogger<UserController> logger, Dapr.Client.DaprClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpPatch("/SetPreferences")]
        public string SetPreferences(UserEmailPreferenceRequest requestUserValues)
        {
            try
            {

                //Check if the strings gotten are compliant with what I want like sport, science etc...


                //check if the userEmail doesn't exist, if it does not than make one and then set the preferences. This happens on UserAccessor

                return "Your preferences were set!";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SetPreferences error: " + ex.Message);
                return ex.Message;

            }
        }

        

        [HttpGet("/GetYourPreferences")]
        public string getPreferences(string email)
        {
            try
            {
                return "Here are your preferences!";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GettingPreferences error: " + ex.Message);
                return ex.Message;

            }
            //Check for the email through accesssor -> if exists one
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
                _logger.LogInformation("LatestNews error: " + ex.Message);
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
                _logger.LogInformation("DeleteUser error: " + ex.Message);
                return ex.Message;
            }

        }
 

    }
}

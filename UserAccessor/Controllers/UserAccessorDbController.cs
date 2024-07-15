using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using UserAccessor.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserAccessor.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserAccessorDbController(ILogger<UserAccessorDbController> logger, DaprClient daprClient, DbContext db) : ControllerBase
    {
        private readonly ILogger<UserAccessorDbController> _logger = logger;
        private readonly DaprClient _client = daprClient;
        private readonly DbContext _dbContext = db;

        #region Preferences
        [HttpGet("/Preferences")]
        public async Task<List<string>> fetchPreferences(UserEmailRequest userEmail)
        {
            try
            {


                //connect DB to get the preferences of the user
                List<string> fakeList = new List<string>
                {
                    "science", "sport"
                };


                return await Task.FromResult(new List<string> { "science", "sport" });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("/Preferences")]
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
        #endregion

        #region User
         

        [HttpDelete("/User")]
        public async Task<ActionResult<UserEmailResponse>> DeleteUser(UserEmailRequest userEmail)
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
        #endregion

        #region Internal
        private async Task<bool> EmailExistInDB(UserEmailRequest userEmail)
        {
            try
            {
                var query = $"SELECT * FROM NewsUsersDB.useremail WHERE email = '{userEmail.Email}'";
                var results = await _dbContext.RunQuery(query);
                var exists = results.HasRows;
                results.Close();
                return exists;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

    }
}

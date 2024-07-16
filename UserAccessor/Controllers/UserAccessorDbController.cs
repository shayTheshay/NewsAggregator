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
        public async Task<ActionResult<UserEmailPreferenceResponse>> SetPreferences(UserEmailPreferenceRequest userEmailPreferences)
        {            
            try
            {
                var userExist = await EmailExistInDB(userEmailPreferences.Email);


                if (!userExist)
                {
                    var queryNewUser = $"INSERT INTO NewsUsersDB.useremail VALUES {userEmailPreferences.Email}";
                    _dbContext.RunCommand(queryNewUser);
                    
                    var queryUserId = $"SELECT id FROM NewsUsersDB.useremail WHERE email = {userEmailPreferences.Email}";
                    var resultUserId = await _dbContext.RunQuery(queryUserId);    

                    var queryPreferences = $"INSERT INTO NewsUsersDB.preferences VALUES {userEmailPreferences.Preferences}";
                    _dbContext.RunCommand(queryPreferences);

                    var results = await _dbContext.RunQuery(queryNewUser);

                    results.Close();

                }
                else if (userExist)
                {
                    var queryUserId = $"SELECT id FROM NewsUsersDB.useremail WHERE email = {userEmailPreferences.Email}";
                    var queryUpdatePreferences = $"UPDATE preferences SET preference = {userEmailPreferences.Preferences}";
                }
                


                UserEmailPreferenceResponse user = new UserEmailPreferenceResponse { Email = userEmailPreferences.Email , Id = queryUserId, Preferences = userEmailPreferences.Preferences};

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
                var userExist = await EmailExistInDB(userEmail);
                if (!userExist)
                {
                    return NotFound();
                }
                
                return Ok(userExist);//For now -> Please complete the other stuff
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

        private async Task<bool> EmailExistInDB(string email)
        {
            try
            {
                var query = $"SELECT * FROM NewsUsersDB.useremail WHERE email = '{email}'";
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

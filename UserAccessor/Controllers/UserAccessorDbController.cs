using Dapr.Client;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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
        public async Task<UserPreferencesResponse> FetchPreferences(UserEmailRequest userEmail)
        {
            try
            {
                var userExist = await EmailExistInDB(userEmail.Email);
                if (userExist)
                {
                    var userId = await _dbContext.RunQuerySingleAsync<int>(
                        "SELECT id FROM NewsUsersDB.useremail WHERE email = @Email",
                        new MySqlParameter("@Email", userEmail.Email));

                    var preferences = await _dbContext.RunQueryAsync<string>(
                        "SELECT preference FROM NewsUsersDB.preferences WHERE id =@Id",
                        new MySqlParameter("@Id", userId)
                    );

                    return new UserPreferencesResponse { Id = userId, Preferences = preferences };
                }

                else
                {
                    List<string> empty = [];
                    return new UserPreferencesResponse { Id = 0, Preferences = empty };

                }
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
                var userId = -1;
                if (!userExist)
                {
                    await _dbContext.RunCommandAsync(
                        "INSERT INTO NewsUsersDB.useremail (email) VALUES (@Email)",
                        new MySqlParameter("@Email", userEmailPreferences.Email));

                    userId = await _dbContext.RunQuerySingleAsync<int>(
                        "SELECT id FROM NewsUsersDB.useremail WHERE email = @Email",
                        new MySqlParameter("@Email", userEmailPreferences.Email));

                    await _dbContext.RunCommandAsync(
                        "INSERT INTO NewsUsersDB.preferences (id, preference) VALUES (@Id, @Preference)",
                        new MySqlParameter("@Id", userId),
                        new MySqlParameter("@Preference", string.Join(",", userEmailPreferences.Preferences)));
                }
                else
                {
                    userId = await _dbContext.RunQuerySingleAsync<int>(
                        "SELECT id FROM NewsUsersDB.useremail WHERE email = @Email",
                        new MySqlParameter("@Email", userEmailPreferences.Email));

                    await _dbContext.RunCommandAsync(
                        "UPDATE NewsUsersDB.preferences SET preference = @Preference WHERE id = @Id",
                        new MySqlParameter("@Preference", string.Join(",", userEmailPreferences.Preferences)),
                        new MySqlParameter("@Id", userId));
                }
                return new UserEmailPreferenceResponse
                {
                    Email = userEmailPreferences.Email,
                    Id = userId,
                    Preferences = userEmailPreferences.Preferences
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SetPreferences error");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion

        #region User
         
        [HttpDelete("/User")]
        public async Task<ActionResult<bool>> DeleteUser(UserEmailRequest userEmail)
        {
            try
            {
                var userId = await _dbContext.RunQuerySingleAsync<int>(
                    "SELECT id FROM NewsUsersDB.useremail WHERE email = @Email",
                    new MySqlParameter("@Email", userEmail.Email));

                var query = "DELETE FROM NewsUsersDB.useremail WHERE email = @Email";
                await _dbContext.RunCommandAsync(query, new MySqlParameter("@Email", userEmail.Email));

                await _dbContext.RunCommandAsync(
                    "DELETE FROM NewsUsersDB.preferences WHERE id = @email_id",
                    new MySqlParameter("@email_id", userId));
                return true;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Internal
        private async Task<bool> EmailExistInDB(string email)
        {
            try
            {
                var count = await _dbContext.RunQuerySingleAsync<int>(
                    "SELECT COUNT(*) FROM NewsUsersDB.useremail WHERE email = @Email",
                    new MySqlParameter("@Email", email));
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists");
                throw;
            }
        }
        #endregion
    }
}

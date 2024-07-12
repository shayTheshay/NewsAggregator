using Microsoft.AspNetCore.Mvc;
using UserAccessor.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserAccessor.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserAccessorDbController(ILogger<UserAccessorDbController> logger) : ControllerBase
    {
        private readonly ILogger<UserAccessorDbController> _logger = logger;

        [HttpGet("/preferences")]
        public async Task<ActionResult<string>> SetPreferences()
        {
            try
            {
                //Check if the strings gotten are compliant with what I want like sport, science etc...


                //check if the userEmail doesn't exist, if it does not than make one and then set the preferences. This happens on UserAccessor
                
                return Ok(new List<string> {"Your preferences were set!" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SetPreferences error: " + ex.Message);
                return NotFound("There was a problem");

            }
        }


        // GET: api/<UserAccessorDbController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserAccessorDbController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserAccessorDbController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserAccessorDbController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserAccessorDbController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

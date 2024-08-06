using Microsoft.AspNetCore.Mvc;
using Dapr.Client;
using NewsAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

 
    public class NewsController(ILogger<NewsController> logger, Dapr.Client.DaprClient daprClient) : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<NewsController> logger = logger;
        private readonly DaprClient daprClient = daprClient;
    

        [HttpGet("/LatestNews")]
        public async Task<List<string>> FetchLatestNews(UserPreferencesResponse userPreferences)
        {
            try
            {


                //Send here the request -> Maybe use the Function in another way.
                string newsdataApiKey = "pub_501498fa10d8e6a1229d04a3504e9d644cd39";
                string requestUrl = $"https://newsdata.io/api/1/latest?apikey={newsdataApiKey}&category={userPreferences.Preferences[0]}&language=en";//Needs to make the preferences a single string from a list probably

                using HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                //string url = "shay";//function to arrange the url accordinly
                List<string> list = new List<string>();
                return list;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return new List<string>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /*
         * 
         * https://newsdata.io/api/1/latest?apikey=YOUR_API_KEY&q=donald%20trump&region=washington-united%20states%20of%20america
         */


    }
}

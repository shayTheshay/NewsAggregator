using Microsoft.AspNetCore.Mvc;
using Dapr.Client;
using NewsAPI.Models;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text.Json;

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

        #region News
        [HttpGet("/LatestNews")]
        public async Task<List<string>> FetchLatestNews(UserPreferencesResponse userPreferences)
        {
            try
            {

                //Send here the request -> Maybe use the Function in another way.
                string newsdataApiKey = "pub_501498fa10d8e6a1229d04a3504e9d644cd39";// Find a way to hide it somewhere

                string preferencesString = ListOfStringTostring(userPreferences.Preferences);
                string requestUrl = $"https://newsdata.io/api/1/latest?apikey={newsdataApiKey}&category={preferencesString}&language=en";//Needs to make the preferences a single string from a list probably

                using HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine(responseBody);
                var newsData = JsonSerializer.Deserialize<NewsDataioResponse>(responseBody);
                Console.WriteLine($"NewsDataio response: {newsData}");
                //string url = "shay";//function to arrange the url accordinly
                if (newsData != null)
                {
                    foreach (var item in newsData.results)
                    {
                        Console.WriteLine($"Title: {item.title}");
                        Console.WriteLine($"Content: {item.content}");
                        Console.WriteLine($"Published Date: {item.pubDate}");
                        Console.WriteLine("---");
                    }
                }
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


        #endregion
        /*
         * 
         * https://newsdata.io/api/1/latest?apikey=YOUR_API_KEY&q=donald%20trump&region=washington-united%20states%20of%20america
         */

        #region Internal
        private string ListOfStringTostring(List<string> list)
        {
            int totalCount = list.Count;
            int counter = 0;
            try
            {
                string fullstring = "";
                foreach (string str in list)
                {
                    if (counter != totalCount - 1)
                    {
                        fullstring += str + ",";
                        counter++;
                    }
                    else
                    {
                        fullstring += str;
                    }
                }
                return fullstring;
            }

            catch (Exception ex)
            {
                throw ;
            }
        }


        #endregion

    }
}

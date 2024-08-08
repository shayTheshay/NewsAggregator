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
        private static readonly HttpClient _httpClient = new HttpClient();//Instantiate an Http connection that is used as static as to not create a number of connections
        private readonly ILogger<NewsController> logger = logger;
        private readonly DaprClient daprClient = daprClient;

        #region News
        [HttpGet("/LatestNews")]
        public async Task<List<NewsResponse>> FetchLatestNews(UserPreferencesResponse userPreferences)
        {
            try
            {
                //Send here the request -> Maybe use the Function in another way.
                string newsdataApiKey = "pub_501498fa10d8e6a1229d04a3504e9d644cd39";// Find a way to hide it somewhere

                string preferencesString = ListOfStringTostring(userPreferences.Preferences);
                string requestUrl = $"https://newsdata.io/api/1/latest?apikey={newsdataApiKey}&category={preferencesString}&language=en";//Needs to make the preferences a single string from a list probably

                using HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);//Keeping the status att -> maybe use in future.
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine(responseBody);
                var newsData = JsonSerializer.Deserialize<NewsDataioResponse>(responseBody);
                Console.WriteLine($"NewsDataio response: {newsData}");
                //string url = "shay";//function to arrange the url accordinly
                if (newsData != null)
                {

                    List<NewsResponse> newsList = new List<NewsResponse>();
                    //According to documentation the "Content" attribute is blocked for non paid subscriptions(according https://newsdata.io/changelog in Dec 01, 2023) to  and returns the string "ONLY AVAILABLE IN PAID PLANS"
                    foreach ( var item in newsData.results)
                    {
                        newsList.Add(new NewsResponse
                        {
                            newsContent = "title:" + item.title + " " + "description:" + item.description, 
                            newsLink = item.link
                        });
                    }//Brings 10 latest news(maximum of 10 according to the documentation)
                    return newsList;
                }
                List<NewsResponse> emptyList = new List<NewsResponse>();
                return emptyList;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return new List<NewsResponse>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

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

namespace NewsAPI.Models
{
    public class NewsDataioResponse
    {

        public string status { get; set; }
        public int totalResults { get; set; }
        public List<NewsItem> results { get; set; }
        public string? nextPage { get; set; }
    }
    public class NewsItem
    {
        public string title { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public string? pubDate { get; set; }
        public string? link { get; set; }
        public List<string> category { get; set; }
    }

}

namespace NewsAPI.Models
{
    public class NewsDataioResponse
    {
        public required string status { get; set; }
        public required List<NewsItem> results { get; set; }
    }
    public class NewsItem
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public string? content { get; set; }
        public required string link { get; set; }
    }

    public class NewsResponse
    {
        public required string newsContent { get; set; }
        public required string newsLink { get; set; }
    }
}

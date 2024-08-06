namespace NewsAPI.Models
{
    public class UserPreferencesResponse
    {
        public required int Id { get; set; }
        public required List<string> Preferences { get; set; } = new List<string>();
    }

}

namespace UserAccessor.Models
{
    public class UserEmailPreferenceResponse
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
        public required List<string> Preferences { get; set; } = new List<string>();
    }
    public class UserEmailResponse
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
    }
}

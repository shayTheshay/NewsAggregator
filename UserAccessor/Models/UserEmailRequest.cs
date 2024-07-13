using System.ComponentModel.DataAnnotations;

namespace UserAccessor.Models
{
    public class UserEmailPreferenceRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email is not correct")]
        public required string Email { get; set; }
        [Required]
        public required List<string> Preferences { get; set; } = new List<string>();
    }
    public class UserEmailRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email is not correct")]
        public required string Email { get; set; }
    }
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

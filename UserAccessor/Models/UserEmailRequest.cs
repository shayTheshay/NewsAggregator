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



}

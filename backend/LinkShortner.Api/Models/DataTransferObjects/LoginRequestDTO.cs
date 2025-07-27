using System.ComponentModel.DataAnnotations;

namespace LinkShortner.Api.Models.DataTransferObjects
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LinkShortner.Api.Models.DataTransferObjects
{
    public class RegisterRequestDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

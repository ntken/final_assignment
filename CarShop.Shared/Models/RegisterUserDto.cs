using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}

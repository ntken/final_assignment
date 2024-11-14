using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W)[A-Za-z\d\W]{6,}$/", 
            ErrorMessage = "Password must be at least 6 characters, include at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}

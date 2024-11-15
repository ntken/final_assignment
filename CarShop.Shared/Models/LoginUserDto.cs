using System.ComponentModel.DataAnnotations;

namespace CarShop.Shared.Models
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        public string Password { get; set; }
    }
}

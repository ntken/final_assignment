using System.ComponentModel.DataAnnotations;

namespace CarStore.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}

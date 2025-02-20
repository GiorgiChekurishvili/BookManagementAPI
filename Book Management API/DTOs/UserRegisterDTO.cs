using System.ComponentModel.DataAnnotations;

namespace Book_Management_API.DTOs
{
    public class UserRegisterDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}

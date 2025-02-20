using System.ComponentModel.DataAnnotations;

namespace Book_Management_API.DTOs
{
    public class UserLoginDTO
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

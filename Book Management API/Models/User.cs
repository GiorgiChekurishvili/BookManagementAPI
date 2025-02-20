
using Book_Management_API.Models.Common;

namespace Book_Management_API.Models
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
    }
}

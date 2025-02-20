
using Book_Management_API.Models.Common;

namespace Book_Management_API.Models
{
    public class Book : BaseEntity
    {
        public required string Title { get; set; }
        public int PublicationYear { get; set; }
        public required string AuthorName { get; set; }
        public int ViewsCount { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}

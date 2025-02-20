namespace Book_Management_API.DTOs
{
    public class BookDTO
    {
        public required string Title { get; set; }
        public int PublicationYear { get; set; }
        public required string AuthorName { get; set; }
    }
}

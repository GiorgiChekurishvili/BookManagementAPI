using Book_Management_API.Models;

namespace Book_Management_API.DataAccess
{
    public interface IBookRepository
    {
        Task AddBook(Book book);
        Task AddBulkBooks(List<Book> books);
        Task UpdateBook(Book book);
        Task<IEnumerable<string>> GetBooksTitle();
        Task<Book> GetBookById(int id);
        Task<Book> GetBookByTitle(string title);
        Task DeleteBook(int id);
    }
}

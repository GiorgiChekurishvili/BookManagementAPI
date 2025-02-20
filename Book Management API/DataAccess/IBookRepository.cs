using Book_Management_API.Models;

namespace Book_Management_API.DataAccess
{
    public interface IBookRepository
    {
        Task<int> AddBook(Book book);
        Task<IEnumerable<int>> AddBulkBooks(List<Book> books);
        Task<Book> UpdateBook(Book book);
        Task<IEnumerable<string>> GetBooksTitle();
        Task<Book> GetBookById(int id);
        Task<Book> GetBookByTitle(string title);
        Task DeleteBook(int id);
        Task DeleteBulkBooks(List<int> ids);
    }
}

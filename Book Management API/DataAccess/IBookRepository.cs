using Book_Management_API.Models;

namespace Book_Management_API.DataAccess
{
    public interface IBookRepository
    {
        Task BookAdd(Book book);
        Task BulkBookAdd(Book book);
        Task BookUpdate(Book book);
        Task<string> GetBooksTitle();
        Task<Book> GetBookById(int id);
        Task<Book> GetBookByTitle(string title);
    }
}

using Book_Management_API.Entities.Context;
using Book_Management_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Management_API.DataAccess
{
    public class BookRepository : IBookRepository
    {
        private readonly BookManagementDbContext _context;
        public BookRepository(BookManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task AddBulkBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                await _context.Books.AddAsync(book);
            }
            await _context.SaveChangesAsync();

        }

        public async Task<Book> GetBookById(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            return book;
        }

        public async Task<Book> GetBookByTitle(string title)
        {
            var book = await _context.Books.Where(x => x.Title.ToLower().Contains(title.ToLower())).FirstOrDefaultAsync();
            return book;
        }

        public async Task<IEnumerable<string>> GetBooksTitle()
        {
            var bookstitle = await _context.Books.Select(x=>x.Title).ToListAsync();
            return bookstitle;
        }

        public async Task UpdateBook(Book book)
        {
            _context.Books.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

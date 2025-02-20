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

        public async Task DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            book!.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBulkBooks(List<int> ids)
        {
            var books = await _context.Books.Where(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var book in books) 
            {
                book!.IsDeleted = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            var book = await _context.Books.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            return book;
        }

        public async Task<Book> GetBookByTitle(string title)
        {
            var book = await _context.Books.Where(x => x.Title.ToLower().Contains(title.ToLower()) && x.IsDeleted == false).FirstOrDefaultAsync();
            return book;
        }

        public async Task<IEnumerable<string>> GetBooksTitle()
        {
            var bookstitle = await _context.Books.Where(x=>x.IsDeleted == false).Select(x=>x.Title).ToListAsync();
            return bookstitle;
        }

        public async Task UpdateBook(Book book)
        {
            var bookById = GetBookById(book.Id);
            if (bookById != null)
            {
                _context.Books.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}

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
        public async Task<int> AddBook(Book book)
        {
            if (book.PublicationYear > Convert.ToInt32(DateTime.Now.Year))
            {
                return -1;
            }
            var uniqueBookTitle = await GetBookByTitle(book.Title);
            if (uniqueBookTitle != null) 
            {
                return -1;
            }
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }

        public async Task<IEnumerable<int>> AddBulkBooks(List<Book> books)
        {
            var newBooks = new List<Book>();
            
            foreach (var book in books)
            {
                if (book.PublicationYear > Convert.ToInt32(DateTime.Now.Year))
                {
                    continue;
                }
                var uniqueBookTitle = await GetBookByTitle(book.Title);
                if (uniqueBookTitle == null)
                {
                    newBooks.Add(book);
                }
            }
            await _context.AddRangeAsync(newBooks);
            await _context.SaveChangesAsync();
            var insertedbooks = newBooks.Select(x => x.Id).ToList();
            return insertedbooks;

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

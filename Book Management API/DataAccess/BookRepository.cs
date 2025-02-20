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
        // this method Adds a new book to the database.
        //returns The ID of the added book, or -1 if the book title is not unique
        //or the publication year is invalid.
        public async Task<int> AddBook(Book book)
        {
            if (book.PublicationYear > DateTime.Now.Year)
            {
                return -1;
            }
            var uniqueBookTitle = await _context.Books.Where(x => x.Title.ToLower().Contains(book.Title.ToLower()) && x.IsDeleted == false).FirstOrDefaultAsync();
            if (uniqueBookTitle != null) 
            {
                return -1;
            }
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }
        // this method Adds multiple books to the database.
        // returns A list of IDs of the successfully added books
        public async Task<IEnumerable<int>> AddBulkBooks(List<Book> books)
        {
            var newBooks = new List<Book>();
            
            foreach (var book in books)
            {
                if (book.PublicationYear > DateTime.Now.Year)
                {
                    continue;
                }
                var uniqueBookTitle = await _context.Books.Where(x => x.Title.ToLower().Contains(book.Title.ToLower()) && x.IsDeleted == false).FirstOrDefaultAsync();
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
        // this method Marks a book as deleted by setting the property IsDeleted to true.
        public async Task DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            book!.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        // this method Marks multiple books as deleted by setting IsDeleted property to true.
        public async Task DeleteBulkBooks(List<int> ids)
        {
            var books = await _context.Books.Where(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var book in books) 
            {
                book!.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
        }
        // this method Retrieves a book by its ID.
        // returns The book if found and not deleted; otherwise, it will return null
        public async Task<Book> GetBookById(int id)
        {
            var book = await _context.Books.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            if(book != null)
            {
                book.ViewsCount++;
                await _context.SaveChangesAsync();
            }
            return book;
        }
        // this method Retrieves a book by its title.
        // returns The book if found and not deleted; otherwise, null.
        public async Task<Book> GetBookByTitle(string title)
        {
            var book = await _context.Books.Where(x => x.Title.ToLower().Contains(title.ToLower()) && x.IsDeleted == false).FirstOrDefaultAsync();
            if (book != null)
            {
                book.ViewsCount++;
                await _context.SaveChangesAsync();
            }
            return book;
        }

        // this method Retrieves a list of book titles, ordered by popularity and recency.
        //returns a list of book titles sorted based on the Popularity Score.
        public async Task<IEnumerable<string>> GetBooksTitle()
        {
            var bookstitle = await _context.Books
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x=>x.ViewsCount * 0.5 + (DateTime.Now.Year - x.PublicationYear) * 2)
                .Select(x => x.Title).ToListAsync();
            return bookstitle;
        }
        // this method Updates an existing book in the database.
        //returns The updated book if successful; otherwise, null, but in controller we dont return updated book
        // i only wanted to validate the API by returning nulls if user inputs incorrect data and applying relevant HTTP status codes
        public async Task<Book> UpdateBook(Book book)
        {
            if (book.PublicationYear > DateTime.Now.Year)
            {
                return null;
            }
            var bookById = await _context.Books.AsNoTracking().Where(x => x.Id == book.Id && x.IsDeleted == false).FirstOrDefaultAsync();
            if (bookById != null)
            {
                if(bookById.Title != book.Title)
                {
                    var uniqueBookTitle = await _context.Books.Where(x => x.Title.ToLower().Contains(book.Title.ToLower()) && x.IsDeleted == false).FirstOrDefaultAsync();
                    if (uniqueBookTitle != null)
                    {
                        return null;
                    }
                }
                book.ViewsCount = bookById.ViewsCount;
                _context.Books.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return book;
            }
            return null;
        }
    }
}

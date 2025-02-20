using Book_Management_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Management_API.Entities.Context
{
    public class BookManagementDbContext : DbContext
    {
        public BookManagementDbContext(DbContextOptions<BookManagementDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x=> x.Id);
            modelBuilder.Entity<User>().HasIndex(x=>x.Email).IsUnique();

            modelBuilder.Entity<Book>().HasKey(x=> x.Id);
            modelBuilder.Entity<Book>().HasIndex(x => x.Title).IsUnique();


        }
    }
}

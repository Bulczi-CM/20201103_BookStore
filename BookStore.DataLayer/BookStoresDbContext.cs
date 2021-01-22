using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataLayer
{
    public class BookStoresDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Bookstore> BookStores { get; set; }
        public DbSet<BookStoreBook> BookStoresBooks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql(@"Host=localhost;Database=postgres;Username=postgres;Password=haselko");
            optionsBuilder.UseInMemoryDatabase(databaseName: "InMemoryDb");
        }
    }
}
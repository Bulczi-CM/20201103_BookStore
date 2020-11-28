using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataLayer
{
    public class BookStoresDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Bookstore> BookStores { get; set; }
        public DbSet<BookStoreBook> BookStoresBooks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=CM_20201103_BookStores;Trusted_Connection=True");
        }
    }
}
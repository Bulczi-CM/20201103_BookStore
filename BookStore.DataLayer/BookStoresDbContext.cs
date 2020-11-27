using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataLayer
{
    public class BookStoresDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=CM_20201103_BookStores;Trusted_Connection=True");
        }
    }
}
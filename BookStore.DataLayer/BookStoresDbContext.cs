using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookStore.DataLayer
{
    public interface IBookStoresDbContext : IDisposable
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Book> Books { get; set; }
        DbSet<Bookstore> BookStores { get; set; }
        DbSet<BookStoreBook> BookStoresBooks { get; set; }
        DbSet<User> Users { get; set; }

        int SaveChanges();
    }

    public class BookStoresDbContext : DbContext, IBookStoresDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Bookstore> BookStores { get; set; }
        public DbSet<BookStoreBook> BookStoresBooks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql(@"Host=localhost;Database=postgres;Username=postgres;Password=haselko");
            //optionsBuilder.UseInMemoryDatabase(databaseName: "InMemoryDb");
            optionsBuilder.UseMySql("server=127.0.0.1;port=3306;user=root;password=my-secret-pw;database=database");
        }
    }
}
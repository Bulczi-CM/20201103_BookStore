using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.DataLayer
{
    public interface IBookStoresDbContext : IDisposable
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Book> Books { get; set; }
        DbSet<Bookstore> BookStores { get; set; }
        DbSet<BookStoreBook> BookStoresBooks { get; set; }
        DbSet<User> Users { get; set; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
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
            optionsBuilder.UseSqlServer(@"Server=.;Database=CM_20201103_BookStores;Trusted_Connection=True");
        }
    }
}
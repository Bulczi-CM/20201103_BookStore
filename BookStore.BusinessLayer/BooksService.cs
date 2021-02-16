using System;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer
{
    public interface IBooksService
    {
        void AddBook(Book book);
        Task<List<Book>> GetAllBooksAsync();
        List<Bookstore> GetBookAvailability(int bookId);
        float SellBooks(Dictionary<int, uint> basket);
        bool UpdateBookQuantity(int bookId, uint quantity);
    }

    public class BooksService : IBooksService
    {
        private readonly ILogger _logger;
        private readonly INotifier _notifier;
        private readonly Func<IBookStoresDbContext> _dbContextFactoryMethod;

        public BooksService(
            ILogger logger,
            INotifier notifier,
            Func<IBookStoresDbContext> dbContextFactoryMethod)
        {
            _logger = logger;
            _notifier = notifier;
            _dbContextFactoryMethod = dbContextFactoryMethod;
        }

        public void AddBook(Book book)
        {
            using (var context = _dbContextFactoryMethod())
            {
                context.Books.Add(book);
                context.SaveChanges();

                _logger.Information("Kupiono książkę!");
            }
        }

        //public Task<List<Book>> GetAllBooks()
        //{
        //    var task = new Task<List<Book>>(() =>
        //    {
        //        using (var context = new BookStoresDbContext())
        //        {
        //            Thread.Sleep(20000);

        //            return context.Books
        //                .Include(book => book.Author) //<== to sprawi, ze w ksiazce beda tez dane autora
        //                .ToList();
        //        }
        //    });

        //    task.Start();

        //    return task;
        //}

        public async Task<List<Book>> GetAllBooksAsync()
        {
            using (var context = new BookStoresDbContext())
            {
                return await context.Books
                    .Include(book => book.Author)
                    .ToListAsync();
            }
        }

        public bool UpdateBookQuantity(int bookId, uint quantity)
        {   //TODO - Co sie stanie, jezeli tutaj bedzie Book book a nie int bookId
            using (var context = new BookStoresDbContext())
            {
                var book = context.Books
                    .FirstOrDefault(book => book.Id == bookId);

                if (book == null)
                {
                    return false;
                }

                book.CopiesCount = quantity;
                context.SaveChanges();
            }

            return true;
        }

        public List<Bookstore> GetBookAvailability(int bookId)
        {
            using (var context = new BookStoresDbContext())
            {
                return context.BookStoresBooks
                    .Include(bookStoreBook => bookStoreBook.BookStore)
                    .Where(bookStoreBook => bookStoreBook.BookId == bookId)
                    .Select(bookStoreBook => bookStoreBook.BookStore)
                    .ToList();
            }
        }

        public float SellBooks(Dictionary<int, uint> basket)
        {
            var cost = 0.0f;

            using (var context = _dbContextFactoryMethod())
            {
                foreach (KeyValuePair<int, uint> item in basket)
                {
                    var book = context.Books.FirstOrDefault(x => x.Id == item.Key);
    
                    if (book == null)
                    {
                        continue;
                    }

                    cost += book.Price * item.Value;
                    book.CopiesCount -= item.Value;
                }

                context.SaveChanges();
            }

            if (cost > 0)
            {
                _notifier.Notify("udało się sprzedać książkę!!!! zarobiliśmy całe " + cost + " złotych!");
            }

            return cost;
        }
    }
}
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.BusinessLayer
{
    public class BooksService
    {
        public void AddBook(Book book)
        {
            using (var context = new BookStoresDbContext())
            {
                context.Books.Add(book);
                context.SaveChanges();
            }
        }

        public List<Book> GetAllBooks()
        {
            using (var context = new BookStoresDbContext())
            {
                return context.Books
                    .Include(book => book.Author) //<== to sprawi, ze w ksiazce beda tez dane autora
                    .ToList();
            }
        }

        public bool UpdateBookQuantity(int bookId, uint quantity)
        {   //TODO - Co sie stanie, jezeli tutaj bedzie Book book a nie int bookId
            using(var context = new BookStoresDbContext())
            {
                var book = context.Books
                    .FirstOrDefault(book => book.Id == bookId);

                if(book == null)
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
            if(basket.Count() == 0)
            {
                return 0.0f;
            }

            var cost = 0.0f;

            using(var context = new BookStoresDbContext())
            {
                foreach(var item in basket)
                {
                    var book = context.Books
                        .Where(book => book.Id == item.Key)
                        .FirstOrDefault();

                    if (book == null)
                    {
                        continue;
                    }

                    cost += book.Price * item.Value;
                    book.CopiesCount -= item.Value;

                    context.SaveChanges();
                }
            }

            return cost;
        }
    }
}
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
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
                return context.Books.ToList();
            }
        }

        public bool UpdateBookQuantity(int bookId, uint quantity)
        {   //TODO - to jeszcze nie dziala!
            Book book = GetBook(bookId);

            if(book == null)
            {
                return false;
            }

            book.CopiesCount = quantity;
            return true;
        }

        private Book GetBook(int bookId)
        {
            using (var context = new BookStoresDbContext())
            {
                return context.Books.FirstOrDefault(book => book.Id == bookId);
            }
        }

        public float SellBooks(Dictionary<int, uint> basket)
        {
            if(basket.Count() == 0)
            {
                return 0.0f;
            }

            var cost = 0.0f;

            var bookIds = basket
                .Select(keyValuePair => keyValuePair.Key)
                .ToList();

            using(var context = new BookStoresDbContext())
            {
                //TODO Rozbic na pojedyncze zapytania i pokazac jak to dziala
                return context.Books
                    .Where(book => bookIds.Contains(book.Id))
                    .Select(book => book.Price * basket[book.Id])
                    .Sum();
            }
        }
    }
}
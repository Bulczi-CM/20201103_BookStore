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

        private Book GetBook(int bookId)
        {
            using (var context = new BookStoresDbContext())
            {
                return context.Books
                    .FirstOrDefault(book => book.Id == bookId);
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
                foreach(var item in basket)
                {
                    cost += context.Books
                        .Where(book => book.Id == item.Key)
                        .Select(book => book.Price)
                        .FirstOrDefault() * item.Value;
                }
            }

            return cost;
        }
    }
}
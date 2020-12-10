using System;
using System.Linq;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;

namespace BookStore.BusinessLayer
{
    public interface IBookRepository
    {
        public Book GetBookById(int id);
        void Update(Book book);
    }

    public class BookRepository : IBookRepository
    {
        public Book GetBookById(int id)
        {
            using(var context = new BookStoresDbContext())
            {
                var book = context.Books
                    .Where(book => book.Id == id)
                    .FirstOrDefault();

                return book;
            }
        }

        public void Update(Book book)
        {
            using(var context = new BookStoresDbContext())
            {
                context.Update(book);

                context.SaveChanges();
            }
        }
    }
}

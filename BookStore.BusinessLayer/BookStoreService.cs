using BookStore.BusinessLayer.Models;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.BusinessLayer
{
    public class BookStoreService
    {
        public void Add(Bookstore bookStore)
        {
            using(var context = new BookStoresDbContext())
            {
                context.BookStores.Add(bookStore);
                context.SaveChanges();
            }
        }

        public void AddBookToBookStore(BookStoreBook bookStoreBook)
        {
            using (var context = new BookStoresDbContext())
            {
                context.BookStoresBooks.Add(bookStoreBook);
                context.SaveChanges();
            }
        }

        public List<BookStoreUserAssignment> GetBookStoresAssignedToUsers()
        {
            using (var context = new BookStoresDbContext())
            {
                var result = context.BookStores
                    .Join(
                        context.Users,
                        bookStore => bookStore.Address,
                        user => user.City,
                        (bookStore, user) => new BookStoreUserAssignment
                        {
                            BookStoreName = bookStore.Name,
                            City = user.City,
                            Login = user.Login
                        })
                        .ToList();

                return result;
            }
        }
    }
}
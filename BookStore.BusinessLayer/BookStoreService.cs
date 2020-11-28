using BookStore.DataLayer;
using BookStore.DataLayer.Models;

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
    }
}
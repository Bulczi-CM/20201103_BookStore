using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Business
{
    public class BooksService
    {
        private Stock _stock = new Stock();

        public void AddBook(Book book)
        {
            _stock.Books.Add(book);
        }

        public List<Book> GetAllBooks()
        {
            return _stock.Books.ToList();
        }

        public bool UpdateBookQuantity(int bookId, uint quantity)
        {
            if(_stock.Books.Count >= bookId + 1)
            {
                return false;
            }

            Book book = _stock.Books[bookId];
            book.CopiesCount = quantity;
            return true;
        }
    }
}
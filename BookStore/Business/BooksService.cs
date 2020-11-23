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
            var book = GetBook(bookId);

            if(book == null)
            {
                return false;
            }

            book.CopiesCount = quantity;
            return true;
        }

        private Book GetBook(int bookId)
        {
            if (_stock.Books.Count <= bookId)
            {
                return null;
            }

            Book book = _stock.Books[bookId];
            return book;
        }

        public float SellBook(int bookId)
        {
            var book = GetBook(bookId);

            if (book == null || !book.IsAvailable)
            {
                return 0.0f;
            }

            book.CopiesCount--;
            return book.Price;
        }
    }
}
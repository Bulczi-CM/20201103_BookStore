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
            Stock.Books.Add(book);
        }

        public List<Book> GetAllBooks()
        {
            return Stock.Books.ToList();
        }

        public bool UpdateBookQuantity(int bookId, uint quantity)
        {
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
            if (Stock.Books.Count <= bookId)
            {
                return null;
            }

            Book book = Stock.Books[bookId];
            return book;
        }

        public float SellBooks(Dictionary<int, uint> basket)
        {
            float cost = 0.0f;

            foreach(KeyValuePair<int, uint> item in basket)
            {
                Book book = GetBook(item.Key);

                if (book == null || book.CopiesCount < item.Value)
                {
                    continue;
                }

                book.CopiesCount -= item.Value;
                cost += book.Price * item.Value;
            }

            return cost;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace BookStore
{
    class Stock
    {
        private List<Book> _books = new List<Book>();

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public List<Book> GetAllBooks()
        {
            return _books.ToList();
        }
    }
}
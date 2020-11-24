using BookStore.DataLayer.Models;
using System.Collections.Generic;

namespace BookStore.DataLayer
{
    public static class Stock
    {
        public static List<Book> Books = new List<Book>();
        public static List<Author> Authors = new List<Author>();
    }
}
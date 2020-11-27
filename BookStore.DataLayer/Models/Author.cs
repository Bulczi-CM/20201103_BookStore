using System;
using System.Collections.Generic;

namespace BookStore.DataLayer.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }

        public List<Book> Books { get; set; }
    }
}
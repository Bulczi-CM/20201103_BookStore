using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.BusinessLayer
{
    public class AuthorsService
    {
        public void Add(Author author)
        {
            Stock.Authors.Add(author);
        }

        public List<Author> GetAll()
        {
            return Stock.Authors.ToList();
        }
    }
}
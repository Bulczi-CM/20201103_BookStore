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
            using (var context = new BookStoresDbContext())
            {
                context.Authors.Add(author);
                context.SaveChanges();
            }
        }

        public List<Author> GetAll()
        {
            using (var context = new BookStoresDbContext())
            {
                return context.Authors.ToList();
            }
        }
    }
}
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
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

        public Author Get(int authorId)
        {
            using (var context = new BookStoresDbContext())
            {
                return context.Authors
                    .FirstOrDefault(author => author.Id == authorId);
            }
        }

        public List<Book> GetBooksByAuthoSurname(string surname)
        {
            using (var context = new BookStoresDbContext())
            {
                var books = context.Books
                    .Include(book => book.Author)
                    .Where(book => book.Author.Surname == surname)
                    .ToList();

                return books;
            }
        }

        public void Update(Author author)
        {
            using (var context = new BookStoresDbContext())
            {
                context.Authors.Update(author);
                context.SaveChanges();
            }
        }

        public void Delete(Author author)
        {
            using (var context = new BookStoresDbContext())
            {
                context.Authors.Remove(author);
                context.SaveChanges();
            }
        }
    }
}
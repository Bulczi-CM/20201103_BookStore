using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BookStore.BusinessLayer
{
    public interface IAuthorsService
    {
        void Add(Author author);
        void Delete(Author author);
        Author Get(int authorId);
        List<Author> GetAll();
        List<Book> GetBooksByAuthoSurname(string surname);
        void Update(Author author);
    }

    public class AuthorsService : IAuthorsService
    {
        private readonly Func<IBookStoresDbContext> _bookStoresDbContextFactoryMethod;

        public AuthorsService(
            Func<IBookStoresDbContext> bookStoresDbContextFactoryMethod)
        {
            _bookStoresDbContextFactoryMethod = bookStoresDbContextFactoryMethod;
        }

        public void Add(Author author)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Authors.Add(author);
                context.SaveChanges();
            }

            long ticksElapsed = stopwatch.ElapsedTicks;
            Log.Information("Dodawanie autora zajęło {ticksElapsed} sekund.", ticksElapsed);
        }

        public List<Author> GetAll()
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                return context.Authors.ToList();
            }
        }

        public Author Get(int authorId)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                return context.Authors
                    .FirstOrDefault(author => author.Id == authorId);
            }
        }

        public List<Book> GetBooksByAuthoSurname(string surname)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
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
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Authors.Update(author);
                context.SaveChanges();
            }
        }

        public void Delete(Author author)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Authors.Remove(author);
                context.SaveChanges();
            }
        }
    }
}
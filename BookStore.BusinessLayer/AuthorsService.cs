using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer
{
    public interface IAuthorsService
    {
        Task AddAsync(Author author);
        void Delete(Author author);
        Author Get(int authorId);
        Task<List<Author>> GetAllAsync();
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

        public async Task AddAsync(Author author)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Authors.Add(author);
                await context.SaveChangesAsync();
            }

            long ticksElapsed = stopwatch.ElapsedTicks;
            Log.Information("Dodawanie autora zajęło {ticksElapsed} sekund.", ticksElapsed);
        }

        public async Task<List<Author>> GetAllAsync()
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                return await context.Authors
                    .AsQueryable()
                    .ToListAsync();
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
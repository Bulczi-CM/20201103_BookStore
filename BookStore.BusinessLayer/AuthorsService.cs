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
        Task DeleteAsync(Author author);
        Task<Author> GetAsync(int authorId);
        Task<List<Author>> GetAllAsync();
        Task<List<Book>> GetBooksByAuthoSurnameAsync(string surname);
        Task UpdateAsync(Author author);
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

        public Task<Author> GetAsync(int authorId)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                return context.Authors
                    .AsQueryable()
                    .FirstOrDefaultAsync(author => author.Id == authorId);
            }
        }

        public async Task<List<Book>> GetBooksByAuthoSurnameAsync(string surname)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                var books = await context.Books
                    .Include(book => book.Author)
                    .Where(book => book.Author.Surname == surname)
                    .AsQueryable()
                    .ToListAsync();

                return books;
            }
        }

        public async Task UpdateAsync(Author author)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Authors.Update(author);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Author author)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Authors.Remove(author);
                await context.SaveChangesAsync();
            }
        }
    }
}
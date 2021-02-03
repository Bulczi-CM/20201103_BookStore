﻿using System;
using System.Linq;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;

namespace BookStore.BusinessLayer
{
    public interface IBookRepository
    {
        public Book GetBookById(int id);
        void Update(Book book);
    }

    public class BookRepository : IBookRepository
    {
        private Func<IBookStoresDbContext> _bookStoresDbContextFactoryMethod;

        public BookRepository(Func<IBookStoresDbContext> bookStoresDbContextFactoryMethod)
        {
            _bookStoresDbContextFactoryMethod = bookStoresDbContextFactoryMethod;
        }

        public Book GetBookById(int id)
        {
            using(var context = _bookStoresDbContextFactoryMethod())
            {
                var book = context.Books
                    .Where(book => book.Id == id)
                    .FirstOrDefault();

                return book;
            }
        }

        public void Update(Book book)
        {
            using(var context = _bookStoresDbContextFactoryMethod())
            {
                context.Books.Update(book);

                context.SaveChanges();
            }
        }
    }
}

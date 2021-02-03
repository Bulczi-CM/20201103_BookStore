using BookStore.BusinessLayer.Models;
using BookStore.BusinessLayer.Serializers;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookStore.BusinessLayer
{
    public interface IBookStoreService
    {
        void Add(Bookstore bookStore);
        void AddBookToBookStore(BookStoreBook bookStoreBook);
        List<BookStoreBook> DeserializeOffer(string filePath, SerializationFormat format);
        List<BookStoreUserAssignment> GetBookStoresAssignedToUsers();
        bool SerializeOffer(string targetDirectoryPath, SerializationFormat format);
    }

    public class BookStoreService : IBookStoreService
    {
        private readonly IDataSerializersFactory _dataSerializersFactory;
        private readonly Func<IBookStoresDbContext> _dbContextFactoryMethod;

        public BookStoreService(
            IDataSerializersFactory dataSerializersFactory,
            Func<IBookStoresDbContext> dbContextFactoryMethod)
        {
            _dataSerializersFactory = dataSerializersFactory;
            _dbContextFactoryMethod = dbContextFactoryMethod;
        }

        public void Add(Bookstore bookStore)
        {
            using (var context = _dbContextFactoryMethod())
            {
                context.BookStores.Add(bookStore);
                context.SaveChanges();
            }
        }

        public void AddBookToBookStore(BookStoreBook bookStoreBook)
        {
            using (var context = _dbContextFactoryMethod())
            {
                context.BookStoresBooks.Add(bookStoreBook);
                context.SaveChanges();
            }
        }

        public List<BookStoreUserAssignment> GetBookStoresAssignedToUsers()
        {
            using (var context = _dbContextFactoryMethod())
            {
                var result = context.BookStores
                    .Join(
                        context.Users,
                        bookStore => bookStore.Address,
                        user => user.City,
                        (bookStore, user) => new BookStoreUserAssignment
                        {
                            BookStoreName = bookStore.Name,
                            City = user.City,
                            Login = user.Login
                        })
                        .ToList();

                return result;
            }
        }

        public bool SerializeOffer(string targetDirectoryPath, SerializationFormat format)
        {
            if (!Directory.Exists(targetDirectoryPath))
            {
                return false;
            }

            var serializer = _dataSerializersFactory.Create(format);

            var filePath = Path.Combine(targetDirectoryPath, $"offer.{serializer.FileExtension}");
            List<BookStoreBook> offer;

            using (var context = _dbContextFactoryMethod())
            {
                offer = context.BookStoresBooks
                    .Include(x => x.Book)
                    .Include(x => x.Book.Author)
                    .Include(x => x.BookStore)
                    .ToList();

                offer.ForEach(x => x.Book.Author.Books = null);
            }

            serializer.Serialize(filePath, offer);

            return true;
        }

        public List<BookStoreBook> DeserializeOffer(string filePath, SerializationFormat format)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var serializer = _dataSerializersFactory.Create(format);
            var offer = serializer.Deserialize(filePath);

            return offer;
        }
    }
}
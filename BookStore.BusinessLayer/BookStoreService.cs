using BookStore.BusinessLayer.Models;
using BookStore.BusinessLayer.Serializers;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer
{
    public interface IBookStoreService
    {
        Task AddAsync(Bookstore bookStore);
        Task AddBookToBookStoreAsync(BookStoreBook bookStoreBook);
        List<int> GetAllBookStoresIds();
        Task <List<BookStoreBook>> DeserializeOfferAsync(string filePath, SerializationFormat format);
        //List<BookStoreUserAssignment> GetBookStoresAssignedToUsers();
        Task <bool> SerializeOfferAsync(string targetDirectoryPath, SerializationFormat format);
        List<BookStoreBook> GetOffer(int bookStoreId);
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

        public async Task AddAsync(Bookstore bookStore)
        {
            using (var context = _dbContextFactoryMethod())
            {
                context.BookStores.Add(bookStore);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddBookToBookStoreAsync(BookStoreBook bookStoreBook)
        {
            using (var context = _dbContextFactoryMethod())
            {
                context.BookStoresBooks.Add(bookStoreBook);
                await context.SaveChangesAsync();
            }
        }

        public List<BookStoreBook> GetOffer(int bookStoreId)
        {
            List<BookStoreBook> offer;

            using (var context = _dbContextFactoryMethod())
            {
                offer = context.BookStoresBooks
                    .Include(x => x.Book)
                    .Include(x => x.Book.Author)
                    .Include(x => x.BookStore)
                    .Where(x => x.BookStore.Id == bookStoreId)
                    .ToList();

                offer.ForEach(x => x.Book.Author.Books = null);
            }

            return offer;
        }

        //public List<BookStoreUserAssignment> GetBookStoresAssignedToUsers()
        //{
        //    using (var context = _dbContextFactoryMethod())
        //    {
        //        var result = context.BookStores
        //            .Join(
        //                context.Users,
        //                bookStore => bookStore.Address,
        //                user => user.City,
        //                (bookStore, user) => new BookStoreUserAssignment
        //                {
        //                    BookStoreName = bookStore.Name,
        //                    City = user.City,
        //                    Login = user.Login
        //                })
        //                .ToList();

        //        return result;
        //    }
        //}

        public async Task<bool> SerializeOfferAsync(string targetDirectoryPath, SerializationFormat format)
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

            await serializer.SerializeAsync(filePath, offer);

            return true;
        }

        public async Task<List<BookStoreBook>> DeserializeOfferAsync(string filePath, SerializationFormat format)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var serializer = _dataSerializersFactory.Create(format);
            var offer = await serializer.DeserializeAsync(filePath);

            return offer;
        }

        public List<int> GetAllBookStoresIds()
        {
            using (var context = _dbContextFactoryMethod())
            {
                return context.BookStores
                    .AsQueryable()
                    .Select(x => x.Id)
                    .ToList();
            }
        }
    }
}
using BookStore.BusinessLayer.Models;
using BookStore.BusinessLayer.Serializers;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookStore.BusinessLayer
{
    public class BookStoreService
    {
        private DataSerializersFactory _serializersFactory = new DataSerializersFactory();

        public void Add(Bookstore bookStore)
        {
            using(var context = new BookStoresDbContext())
            {
                context.BookStores.Add(bookStore);
                context.SaveChanges();
            }
        }

        public void AddBookToBookStore(BookStoreBook bookStoreBook)
        {
            using (var context = new BookStoresDbContext())
            {
                context.BookStoresBooks.Add(bookStoreBook);
                context.SaveChanges();
            }
        }

        public List<BookStoreUserAssignment> GetBookStoresAssignedToUsers()
        {
            using (var context = new BookStoresDbContext())
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
            if(!Directory.Exists(targetDirectoryPath))
            {
                return false;
            }

            var serializer = _serializersFactory.Create(format);

            var filePath = Path.Combine(targetDirectoryPath, $"offer.{serializer.FileExtension}");
            List<BookStoreBook> offer;

            using (var context = new BookStoresDbContext())
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

            var serializer = _serializersFactory.Create(format);
            var offer = serializer.Deserialize(filePath);

            return offer;
        }
    }
}
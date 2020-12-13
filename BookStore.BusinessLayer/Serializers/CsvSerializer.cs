using BookStore.DataLayer.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BookStore.BusinessLayer.Serializers
{
    public class CsvSerializer : IDataSerializer
    {
        public string FileExtension => "csv";

        public void Serialize(string path, List<BookStoreBook> offer)
        {
            var serializedOfferBuilder = new StringBuilder();

            foreach(var item in offer)
            {
                serializedOfferBuilder.AppendLine(
                    $"{item.BookStore.Name},{item.BookStore.Address},{item.Book.Title}," +
                    $"{item.Book.Author.Name},{item.Book.Author.Surname}");
            }

            File.WriteAllText(path, serializedOfferBuilder.ToString());
        }

        public List<BookStoreBook> Deserialize(string path)
        {
            var serializedOfferLines = File.ReadAllLines(path);
            var offer = new List<BookStoreBook>();

            foreach (var line in serializedOfferLines)
            {
                if(string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var data = line.Split(",");

                var bookStoreBook = new BookStoreBook
                {
                    Book = new Book
                    {
                        Title = data[2],
                        Author = new Author
                        {
                            Name = data[3],
                            Surname = data[4]
                        }
                    },
                    BookStore = new Bookstore
                    {
                        Name = data[0],
                        Address = data[1]
                    }
                };

                offer.Add(bookStoreBook);
            }

            return offer;
        }
    }
}
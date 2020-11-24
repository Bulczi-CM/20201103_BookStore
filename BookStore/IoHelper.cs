using BookStore.DataLayer.Models;
using System;
using System.Globalization;

namespace BookStore
{
    public class IoHelper
    {
        public string GetTextFromUser(string message)
        {
            Console.Write($"{message}: ");
            return Console.ReadLine();
        }

        public uint GetUintFromUser(string message)
        {
            uint result;

            while (!uint.TryParse(GetTextFromUser(message), out result))
            {
                Console.WriteLine("Not an unsinged integer. Try again...");
            }

            return result;
        }

        public float GetFloatFromUser(string message)
        {
            //return float.Parse(GetTextFromUser(message));

            float result;

            while (!float.TryParse(GetTextFromUser(message), out result))
            {
                Console.WriteLine("Not an float. Try again...");
            }

            return result;
        }

        public bool GetBoolFromUser(string message)
        {
            bool result;

            while (!bool.TryParse(GetTextFromUser($"{message} [true/false]"), out result))
            {
                Console.WriteLine("Not an bool. Try again...");
            }

            return result;
        }

        public int GetIntFromUser(string message)
        {
            int number;
            while (!int.TryParse(GetTextFromUser(message), out number))
            {
                Console.WriteLine("Not na integer - try again...");
            }

            return number;
        }

        public DateTime GetDateTimeFromUser(string message)
        {
            string format = "dd/MM/yyyy";
            DateTime result;

            while (!DateTime.TryParseExact(
                GetTextFromUser($"{message} [{format}]"),
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result))
            {
                Console.WriteLine("Not an valid date. Try again...");
            }

            return result;
        }

        public BookGenre GetBookGenreFromUser(string message)
        {
            var correctValues = "";

            foreach (var bookGenre in (BookGenre[])Enum.GetValues(typeof(BookGenre)))
            {
                correctValues += $"{bookGenre},";
            }

            object result;
            while (!Enum.TryParse(typeof(BookGenre), GetTextFromUser($"{message} ({correctValues}):"), out result))
            {
                Console.WriteLine("Not a correct value - use one from the brackets. Try again...");
            }

            return (BookGenre)result;
        }

        public void PrintBook(Book book, int index)
        {
            Console.WriteLine($"{index}. {BuildBookString(book)}");
        }

        public void PrintBook(Book book)
        {
            Console.WriteLine(BuildBookString(book));
        }

        private string BuildBookString(Book book)
        {
            return $"{book.Author.Surname} - {book.Title} ({book.PublishDate.Year}): {book.Price} (Av. copies: {book.CopiesCount})";
        }

        public void PrintAuthor(Author author, int index)
        {
            Console.WriteLine($"{index}. {BuildAuthorString(author)}");
        }

        public void PrintAuthor(Author author)
        {
            Console.WriteLine(BuildAuthorString(author));
        }

        private string BuildAuthorString(Author author)
        {
            return $"{author.Surname} {author.Name} ({author.BirthDate})";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BookStore
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        private Stock _stock = new Stock();

        void Run()
        {
            do
            {
                Console.WriteLine();
                Console.WriteLine("Choose action: ");
                Console.WriteLine("Press 1 to Add book");
                Console.WriteLine("Press 2 to Print books");
                Console.WriteLine("Press 3 to Change stock for book");

                int userChoice = GetIntFromUser("Select option");

                switch (userChoice)
                {
                    case 1:
                        AddBook();
                        break;
                    case 2:
                        PrintAllBooks();
                        break;
                    case 3:
                        ChangeStockForBook();
                        break;
                    default:
                        Console.WriteLine("Unknown option");
                        break;
                }
            }
            while (true);
        }

        private void PrintAllBooks()
        {
            PrintBooks(_stock.GetAllBooks());
        }

        private void ChangeStockForBook()
        {
            List<Book> books = _stock.GetAllBooks();

            PrintBooks(books, true);

            int index = GetIntFromUser("Select book id");

            while (index < 1 || index > books.Count)
            {
                Console.WriteLine("Invalid index - try again...");
                index = GetIntFromUser("Select book id");
            }

            Book book = books[index - 1];
            book.CopiesCount = GetUintFromUser($"Enter new copies count (current: {book.CopiesCount})");
        }

        private void PrintBooks(List<Book> books, bool printIndex = false)
        {
            for (int i = 0; i < books.Count; i++)
            {
                Book book = books[i];

                string text = $"{book.Author} - {book.Title} ({book.PublishDate.Year}): {book.Price} (Av. copies: {book.CopiesCount})";

                if(printIndex)
                {
                    text = $"{i + 1}. {text}";
                }

                Console.WriteLine(text);
            }
        }

        void AddBook()
        {
            Console.WriteLine("Creating a book.");

            Book newBook = new Book()
            {
                Author = GetTextFromUser("Enter author"),
                Title = GetTextFromUser("Enter title"),
                Description = GetTextFromUser("Enter description"),
                Genre = GetTextFromUser("Enter genre"),
                HardCover = GetBoolFromUser("Is in hard cover"),
                PagesCount = GetUintFromUser("Enter pages count"),
                Price = GetFloatFromUser("Enter price"),
                PublishDate = GetDateTimeFromUser("Publish date"),
                Publisher = GetTextFromUser("Enter publisher"),
                CopiesCount = GetUintFromUser("Enter amount of copies")
            };

            _stock.AddBook(newBook);
            Console.WriteLine("Book added successfully");
        }

        string GetTextFromUser(string message)
        {
            Console.Write($"{message}: ");
            return Console.ReadLine();
        }

        uint GetUintFromUser(string message)
        {
            uint result;

            while(!uint.TryParse(GetTextFromUser(message), out result))
            {
                Console.WriteLine("Not an unsinged integer. Try again...");
            }

            return result;
        }

        float GetFloatFromUser(string message)
        {
            //return float.Parse(GetTextFromUser(message));

            float result;

            while (!float.TryParse(GetTextFromUser(message), out result))
            {
                Console.WriteLine("Not an float. Try again...");
            }

            return result;
        }

        bool GetBoolFromUser(string message)
        {
            bool result;

            while (!bool.TryParse(GetTextFromUser($"{message} [true/false]"), out result))
            {
                Console.WriteLine("Not an bool. Try again...");
            }

            return result;
        }

        private int GetIntFromUser(string message)
        {
            int number;
            while (!int.TryParse(GetTextFromUser(message), out number))
            {
                Console.WriteLine("Not na integer - try again...");
            }

            return number;
        }

        DateTime GetDateTimeFromUser(string message)
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
    }
}
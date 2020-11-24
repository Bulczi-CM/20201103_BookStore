using BookStore.BusinessLayer;
using BookStore.DataLayer.Models;
using System;
using System.Collections.Generic;

namespace BookStore
{
    class Program
    {
        private IoHelper _ioHelper = new IoHelper();
        private BooksService _booksService = new BooksService();
        private AuthorsService _authorsService = new AuthorsService();

        static void Main(string[] args)
        {
            new Program().Run();
        }

        void Run()
        {
            do
            {
                Console.WriteLine();
                Console.WriteLine("Choose action: ");
                Console.WriteLine("Press 1 to Add Author");
                Console.WriteLine("Press 2 to Add book");
                Console.WriteLine("Press 3 to Print books");
                Console.WriteLine("Press 4 to Change stock for book");
                Console.WriteLine("Press 5 to Sell books");

                int userChoice = _ioHelper.GetIntFromUser("Select option");

                switch (userChoice)
                {
                    case 1:
                        AddAuthor();
                        break;
                    case 2:
                        AddBook();
                        break;
                    case 3:
                        PrintAllBooks();
                        break;
                    case 4:
                        ChangeStockForBook();
                        break;
                    case 5:
                        SellBooks();
                        break;
                    default:
                        Console.WriteLine("Unknown option");
                        break;
                }
            }
            while (true);
        }

        void AddAuthor()
        {
            Console.WriteLine("Creating an author.");

            var name    = _ioHelper.GetTextFromUser("Enter author name");
            var surname = _ioHelper.GetTextFromUser("Enter author surname");

            var bday    = _ioHelper.GetDateTimeFromUser("Enter author's bday date");
            while (bday > DateTime.Now.AddYears(-18))
            {
                Console.WriteLine("Author is to young (18+). Try again...");
                bday = _ioHelper.GetDateTimeFromUser("Enter author's bday date");
            }

            var newAuthor = new Author
            {
                Name = name,
                Surname = surname,
                BirthDate = bday
            };

            _authorsService.Add(newAuthor);
            Console.WriteLine("Book added successfully");
        }

        void AddBook()
        {
            Console.WriteLine("Creating a book.");

            var authors = _authorsService.GetAll();

            if(authors.Count == 0)
            {
                Console.WriteLine("There are no authors to choose from. Add them first!");
                return;
            }

            PrintAuthors(authors, true);

            int index = _ioHelper.GetIntFromUser("Select author id") - 1;

            if (index >= authors.Count || index < 0)
            {
                Console.WriteLine("Incorrect author Id!");
                return;
            }

            var newBook = new Book()
            {
                Author =      authors[index],
                Title =       _ioHelper.GetTextFromUser("Enter title"),
                Description = _ioHelper.GetTextFromUser("Enter description"),
                Genre =       _ioHelper.GetBookGenreFromUser("Enter genre"),
                HardCover =   _ioHelper.GetBoolFromUser("Is in hard cover"),
                PagesCount =  _ioHelper.GetUintFromUser("Enter pages count"),
                Price =       _ioHelper.GetFloatFromUser("Enter price"),
                PublishDate = _ioHelper.GetDateTimeFromUser("Publish date"),
                Publisher =   _ioHelper.GetTextFromUser("Enter publisher"),
                CopiesCount = _ioHelper.GetUintFromUser("Enter amount of copies")
            };

            _booksService.AddBook(newBook);
            Console.WriteLine("Book added successfully");
        }

        private void PrintAllBooks()
        {
            PrintBooks(_booksService.GetAllBooks());
        }

        private void PrintBooks(List<Book> books, bool printIndex = false)
        {
            for (var i = 0; i < books.Count; i++)
            {
                var book = books[i];

                if (printIndex)
                {
                    _ioHelper.PrintBook(book, i + 1);
                }
                else
                {
                    _ioHelper.PrintBook(book);
                }
            }
        }

        private void PrintAuthors(List<Author> authors, bool printIndex = false)
        {
            for (var i = 0; i < authors.Count; i++)
            {
                var author = authors[i];

                if (printIndex)
                {
                    _ioHelper.PrintAuthor(author, i + 1);
                }
                else
                {
                    _ioHelper.PrintAuthor(author);
                }
            }
        }

        private void ChangeStockForBook()
        {
            int index = GetBookIndexFromUser();
            uint quantity = _ioHelper.GetUintFromUser("Enter new copies count");

            bool success = _booksService.UpdateBookQuantity(index, quantity);
            Console.WriteLine(success ? "Book added successfully" : "Book not added");

            //Ternary if
            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
            //uint i = GetUintFromUser("Provide some number");
            //string message = i > 1000
            //    ? "Number is larger than 1000"
            //    : "Number is smaller or equal 1000";
        }

        private int GetBookIndexFromUser()
        {
            List<Book> books = _booksService.GetAllBooks();

            PrintBooks(books, true);

            int index = _ioHelper.GetIntFromUser("Select book id");
            return index - 1;
        }

        private void SellBooks()
        {
            bool exit = false;
            Dictionary<int, uint> basket = new Dictionary<int, uint>();

            while (!exit)
            {
                int index = GetBookIndexFromUser();
                uint quantity = _ioHelper.GetUintFromUser("How many copies");

                if (basket.ContainsKey(index))
                {
                    basket[index] += quantity;
                }
                else
                {
                    basket[index] = quantity;
                }

                exit = !_ioHelper.GetBoolFromUser("Do you want any other book?");
            }

            float cost = _booksService.SellBooks(basket);

            Console.WriteLine($"Recipt: ${cost}");
        }
    }
}
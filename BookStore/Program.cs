using BookStore.Business;
using System;
using System.Collections.Generic;

namespace BookStore
{
    class Program
    {
        private IoHelper _ioHelper = new IoHelper();
        private BooksService _booksService = new BooksService();

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
                Console.WriteLine("Press 1 to Add book");
                Console.WriteLine("Press 2 to Print books");
                Console.WriteLine("Press 3 to Change stock for book");

                int userChoice = _ioHelper.GetIntFromUser("Select option");

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
            PrintBooks(_booksService.GetAllBooks());
        }

        private void ChangeStockForBook()
        {
            List<Book> books = _booksService.GetAllBooks();

            PrintBooks(books, true);

            int index = _ioHelper.GetIntFromUser("Select book id");
            uint quantity = _ioHelper.GetUintFromUser("Enter new copies count");

            bool success = _booksService.UpdateBookQuantity(index - 1, quantity);
            Console.WriteLine(success ? "Book added successfully" : "Book not added");

            //Ternary if
            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
            //uint i = GetUintFromUser("Provide some number");
            //string message = i > 1000
            //    ? "Number is larger than 1000"
            //    : "Number is smaller or equal 1000";
        }

        private void PrintBooks(List<Book> books, bool printIndex = false)
        {
            for (int i = 0; i < books.Count; i++)
            {
                Book book = books[i];

                if(printIndex)
                {
                    _ioHelper.PrintBook(book, i + 1);
                }
                else
                {
                    _ioHelper.PrintBook(book);
                }
            }
        }

        void AddBook()
        {
            Console.WriteLine("Creating a book.");

            Book newBook = new Book()
            {
                Author =      _ioHelper.GetTextFromUser("Enter author"),
                Title =       _ioHelper.GetTextFromUser("Enter title"),
                Description = _ioHelper.GetTextFromUser("Enter description"),
                Genre =       _ioHelper.GetTextFromUser("Enter genre"),
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
    }
}
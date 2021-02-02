using BookStore.BusinessLayer;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace BookStore
{
    class Program
    {
        private Menu                      _menu                      = new Menu();
        private IoHelper                  _ioHelper                  = new IoHelper();
        private BooksService              _booksService;//              = new BooksService(new BookRepository(), new Notifier(), () => new BookStoresDbContext());
        private AuthorsService            _authorsService            = new AuthorsService(() => new BookStoresDbContext());
        private UsersService              _usersService              = new UsersService();
        private NotificationsService      _notificationService       = new NotificationsService();
        private DatabaseManagementService _databaseManagementService = new DatabaseManagementService();
        private BookStoreService          _bookStoreService          = new BookStoreService();

        private bool _exit = false;

        static void Main()
        {
             new Program().Run();
        }

        void Run()
        {
            var container = new UnityDiContainerProvider().GetContainer();
            _booksService = container.Resolve<BooksService>();

            var logConfiguration = new LoggerConfiguration();
            
            //string relativePath = Path.Combine(Environment.CurrentDirectory, "mojelogi.txt");
            string absolutePath = "C:/logi/log.txt";

            logConfiguration
                .WriteTo.File(absolutePath, Serilog.Events.LogEventLevel.Debug)
                .WriteTo.Console(new CompactJsonFormatter())
                ;
            
            Serilog.Core.Logger logger = logConfiguration.CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

            Log.Logger = logger; // setting as global logger - now it'll be used whenever somebody calls Log.Error(), Log.Debug() etc.
            
            Log.Information("Aplikacja uruchomiona!");

            _databaseManagementService.EnsureDatabaseCreation();
            RegisterMenuOptions();

            do
            {
                _menu.PrintAvailableOptions();

                int userChoice = _ioHelper.GetIntFromUser("Select option");

                _menu.ExecuteOption(userChoice);
            }
            while (!_exit);
        }

        private void RegisterMenuOptions()
        {
            _menu.AddOption(new MenuItem { Key =  1, Action = AddAuthor,                Description = "Add new author" });
            _menu.AddOption(new MenuItem { Key =  2, Action = AddBook,                  Description = "Add new book" });
            _menu.AddOption(new MenuItem { Key =  3, Action = AddBookStore,             Description = "Add new bookstore" });
            _menu.AddOption(new MenuItem { Key =  4, Action = PrintAllBooks,            Description = "Print all books" });
            _menu.AddOption(new MenuItem { Key =  5, Action = ChangeStockForBook,       Description = "Change stock for book" });
            _menu.AddOption(new MenuItem { Key =  6, Action = SellBooks,                Description = "Sell some books" });
            _menu.AddOption(new MenuItem { Key =  7, Action = FindBooksByAuthorSurname, Description = "Find books by author surname" });
            _menu.AddOption(new MenuItem { Key =  8, Action = AddBookToBookStore,       Description = "Add book to bookstore" });
            _menu.AddOption(new MenuItem { Key =  9, Action = FindBookInBookStores,     Description = "Find book in bookstores" });
            _menu.AddOption(new MenuItem { Key = 10, Action = UpdateAuthor,             Description = "Update author" });
            _menu.AddOption(new MenuItem { Key = 11, Action = DeleteAuthor,             Description = "Delete author" });
            _menu.AddOption(new MenuItem { Key = 12, Action = AddUser,                  Description = "Add new user" });
            _menu.AddOption(new MenuItem { Key = 13, Action = GetRecommendedBookStores, Description = "Get bookstores recommended for users" });

            _menu.AddOption(new MenuItem { Key = 20, Action = BookArrivalNotification,  Description = "Post notification when new book arrives" });

            _menu.AddOption(new MenuItem { Key = 30, Action = ExportOfferToFile,        Description = "Export offer to file" });
            _menu.AddOption(new MenuItem { Key = 31, Action = ImportOfferFromFile,      Description = "Import offer from file" });

            _menu.AddOption(new MenuItem { Key = 99, Action = () => { _exit = true; },  Description = "Close the application" });
        }

        private void ImportOfferFromFile()
        {
            var filePath = _ioHelper.GetTextFromUser("Enter file path");
            var format = _ioHelper.GetSerializationFormatFromUser("Choose file format");

            var offer = _bookStoreService.DeserializeOffer(filePath, format);

            foreach(var item in offer)
            {
                //...
            }
        }

        private void ExportOfferToFile()
        {
            var targetPath = _ioHelper.GetTextFromUser("Enter target path");
            var format = _ioHelper.GetSerializationFormatFromUser("Choose file format");

            if (_bookStoreService.SerializeOffer(targetPath, format))
            {
                Console.WriteLine("Offer exported successfully");
            }
            else
            {
                Console.WriteLine("Error during offer export");
            }
        }

        private void GetRecommendedBookStores()
        {
            var recommendations = _bookStoreService.GetBookStoresAssignedToUsers();

            foreach(var recommendation in recommendations)
            {
                Console.WriteLine($"{recommendation.Login} {recommendation.City} {recommendation.BookStoreName}");
            }
        }

        private void AddUser()
        {
            var newUser = new User
            {
                Login = _ioHelper.GetTextFromUser("Login"),
                Password = _ioHelper.GetTextFromUser("Password"),
                PhoneNumber = _ioHelper.GetTextFromUser("Phone number with country prefix"),
                City = _ioHelper.GetTextFromUser("City")
            };

            _usersService.Add(newUser);
            Console.WriteLine("User added successfully");
        }

        private void DeleteAuthor()
        {
            var id = _ioHelper.GetIntFromUser("Provide author id");

            //var author = _authorsService.Get(id);
            //_authorsService.Delete(author);

            _authorsService.Delete(new Author { Id = id });
        }

        private void UpdateAuthor()
        {
            var id = _ioHelper.GetIntFromUser("Provide author id");

            var author = _authorsService.Get(id);

            author.Name    = _ioHelper.GetTextFromUser($"Privde new name [current: {author.Name}]:");
            author.Surname = _ioHelper.GetTextFromUser($"Privde new surname [current: {author.Surname}]:");

            _authorsService.Update(author);
        }

        private void FindBookInBookStores()
        {
            var bookId = _ioHelper.GetIntFromUser("Provide Book id");

            var bookStores = _booksService.GetBookAvailability(bookId);

            foreach(var bookStore in bookStores)
            {
                _ioHelper.PrintBookStore(bookStore);
            }
        }

        private void AddBookToBookStore()
        {
            var bookStoreBook = new BookStoreBook
            {
                BookId = _ioHelper.GetIntFromUser("Provide Book id"),
                BookStoreId = _ioHelper.GetIntFromUser("Provide BookStore id")
            };

            _bookStoreService.AddBookToBookStore(bookStoreBook);
        }

        private void AddBookStore()
        {
            var bookstore = new Bookstore
            {
                Name = _ioHelper.GetTextFromUser("Enter bookstore name"),
                Address = _ioHelper.GetTextFromUser("Enter bookstore address")
            };

            _bookStoreService.Add(bookstore);
            Console.WriteLine("BookStore added successfully");
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
            Console.WriteLine("Author added successfully");
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

            int authorId = _ioHelper.GetIntFromUser("Select author id");

            if (!authors.Any(author => author.Id == authorId))
            {
                Console.WriteLine("Incorrect author Id!");
                return;
            }

            var newBook = new Book()
            {
                AuthorId =    authorId,
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

            if (_notificationService.SubscribedNotification != null)
            {
                _notificationService.SubscribedNotification(this, new NewBookEventArgs { Book = newBook });
            }
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
                    _ioHelper.PrintBook(book, book.Id);
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
                    _ioHelper.PrintAuthor(author, i);
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
            return index;
        }

        private void FindBooksByAuthorSurname()
        {
            var surname = _ioHelper.GetTextFromUser("Enter author surname");
            var books = _authorsService.GetBooksByAuthoSurname(surname);

            foreach (var book in books)
            {
                _ioHelper.PrintBook(book);
            }
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

        private void BookArrivalNotification()
        {
            var authorSurname = _ioHelper.GetTextFromUser("Enter author surname");
            var communicationChannel = _ioHelper.GetCommunicationChannelFromUser("Enter communication channel");

            _notificationService.AddNotification(authorSurname, communicationChannel);
        }
    }
}
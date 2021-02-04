//using System;
//using System.Collections.Generic;
//using System.Threading;
//using BookStore.BusinessLayer;
//using BookStore.DataLayer;
//using BookStore.DataLayer.Models;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using NUnit.Framework;

//namespace BookStore.Tests.BusinessLayer
//{
//    /// Nasz rêcznie robiony mock
//    internal class BookRepositoryMock : IBookRepository
//    {
//        public Book GetBookById(int id)
//        {
//            if(id == 1)
//                return new Book(null, "pierwsza", 10);
//            if(id == 2)
//                return new Book(null, "druga", 15);
//            if(id == 3)
//                return new Book(null, "trzeca", 20);
//            else
//                return new Book(null, "czwarta", 25);
//        }

//        public void Update(Book book)
//        {
            
//        }
//    }

//    internal class BookStoreDbContextMock : DbContext, IBookStoresDbContext
//    {
//        public DbSet<User> Users { get; set; }
//        public DbSet<Author> Authors { get; set; }
//        public DbSet<Book> Books { get; set; }
//        public DbSet<Bookstore> BookStores { get; set; }
//        public DbSet<BookStoreBook> BookStoresBooks { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseInMemoryDatabase("BookStoreDb");
//        }
//    }
    
//    /// pusta implementacja — równowa¿na z new Mock<IBookRepository>()
//    internal class BookRepositoryMockSameAsGeneratedByMoq : IBookRepository 
//    {
//        public Book GetBookById(int id)
//        {
//            return null;
//        }

//        public void Update(Book book)
//        {
            
//        }
//    }

//    public class BookServiceTests
//    {
//        [TestCase(0, 0)]
//        [TestCase(2, 20)]
//        [TestCase(3, 30)]
//        [TestCase(500, 5000)]
//        public void SellBooks_MultipleCopiesOfOneBook_ReturnsCorrectCost(int quantity, double expectedCost)
//        {
//            Dictionary<int, uint> basket = new Dictionary<int, uint>();
//            basket.Add(1, (uint)quantity);

//            var mock = new Mock<INotifier>();
//            var service = new BooksService(new BookRepositoryMock(), mock.Object, () => new BookStoreDbContextMock());

//            double cost = service.SellBooks(basket);

//            cost.Should().Be(expectedCost);
//        }

//        [Test]
//        public void SellBooks_ThreeDifferentBooks_ReturnsCorrectCost()
//        {
//            Dictionary<int, uint> basket = new Dictionary<int, uint>();
//            basket.Add(1, 1);
//            basket.Add(2, 1);
//            basket.Add(3, 1);

//            var mock = new Mock<INotifier>();
//            var service = new BooksService(new BookRepositoryMock(), mock.Object, () => new BookStoreDbContextMock());

//            double cost = service.SellBooks(basket);

//            cost.Should().Be(45);
//        }

        
//        [TestCase(0, 0)]
//        [TestCase(2, 20)]
//        [TestCase(3, 30)]
//        [TestCase(500, 5000)]
//        public void SellBooks_WithMock_MultipleCopiesOfOneBook_ReturnsCorrectCost(int quantity, double expectedCost)
//        {
//            Dictionary<int, uint> basket = new Dictionary<int, uint>();
//            basket.Add(1, (uint)quantity);

//            var bookRepositoryMock = new Mock<IBookRepository>();
//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(1))
//                .Returns(new Book(null, "pierwsza", 10));
//            IBookRepository bookRepository = bookRepositoryMock.Object;

//            var notifierMock = new Mock<INotifier>();
//            var service = new BooksService(bookRepository, notifierMock.Object, () => new BookStoreDbContextMock());

//            double cost = service.SellBooks(basket);

//            cost.Should().Be(expectedCost);
//        }

//        [Test]
//        public void SellBooks_WithMock_ThreeDifferentBooks_ReturnsCorrectCost()
//        {
//            Dictionary<int, uint> basket = new Dictionary<int, uint>();
//            basket.Add(1, 1);
//            basket.Add(2, 1);
//            basket.Add(3, 1);

//            var bookRepositoryMock = new Mock<IBookRepository>();
//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(1))
//                .Returns(new Book(null, "pierwsza", 10));
//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(2))
//                .Returns(new Book(null, "druga", 15));
//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(3))
//                .Returns(new Book(null, "trzecia", 20));

//            var notifierMock = new Mock<INotifier>();
//            var service = new BooksService(bookRepositoryMock.Object, notifierMock.Object, () => new BookStoreDbContextMock());

//            double cost = service.SellBooks(basket);

//            cost.Should().Be(45);
//        }

//        [Test]
//        public void SellBooks_TwoBooksWereSold_SendsNotificationOnce()
//        {
//            Dictionary<int, uint> basket = new Dictionary<int, uint>();
//            basket.Add(1, 1);
//            basket.Add(2, 1);
            
//            var bookRepositoryMock = new Mock<IBookRepository>();
//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(1))
//                .Returns(new Book(null, "pierwsza", 10));
//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(2))
//                .Returns(new Book(null, "pierwsza", 10));

//            var notifierMock = new Mock<INotifier>();
//            var service = new BooksService(bookRepositoryMock.Object, notifierMock.Object, () => new BookStoreDbContextMock());

//            service.SellBooks(basket);

//            // assert - chce sie upewnic, ze Notify() zostaje wywolane raz
//            notifierMock.Verify(notifier => notifier.Notify(It.IsAny<string>()), Times.Once);
//        }

//        [Test]
//        public void SellBooks_NoneBooksWereSold_DoesNotSendNotification()
//        {
//            Dictionary<int, uint> basket = new Dictionary<int, uint>();
            
//            var bookRepositoryMock = new Mock<IBookRepository>();
//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(1))
//                .Returns(new Book(null, "pierwsza", 10));

//            var notifierMock = new Mock<INotifier>();
//            var service = new BooksService(bookRepositoryMock.Object, notifierMock.Object, () => new BookStoreDbContextMock());

//            service.SellBooks(basket);

//            // assert - chce sie upewnic, ze Notify() NIE zostaje wywolane
//            notifierMock.Verify(notifier => notifier.Notify(It.IsAny<string>()), Times.Never);
//        }

//        [Test]
//        public void MockingPlayground()
//        {
//            var bookRepositoryMock = new Mock<IBookRepository>();
            
//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(1))
//                .Returns(new Book(null, "pierwsza", 10));

//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(2))
//                .Throws(new AbandonedMutexException("asd"));

//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(3))
//                .Callback(() => Console.WriteLine("ktoœ pobiera ksi¹¿kê 3!"));

//            bookRepositoryMock
//                .Setup(repo => repo.GetBookById(It.IsAny<int>()))
//                .Returns(new Book(null, "jakaœ inna", 50));

//            Book book3 = bookRepositoryMock.Object.GetBookById(3);

//            Book book1 = bookRepositoryMock.Object.GetBookById(1);

//            Book book500 = bookRepositoryMock.Object.GetBookById(500);

//            book500.Title.Should().Be("jakaœ inna");
//            // Book book2 = bookRepositoryMock.Object.GetBookById(2);
//        }
//    }
//}
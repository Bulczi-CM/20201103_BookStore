using BookStore.BusinessLayer;
using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Serilog;
using System.Linq;

namespace BookStore.Tests.BusinessLayer
{
    public class BookServiceTests
    {
        private readonly Func<IBookStoresDbContext> _contextFactoryMethod = () => new BookStoresInMemoryDbContext();

        [SetUp]
        public void SetUp()
        {
            using (var context = _contextFactoryMethod())
            {
                var books = context.Books
                    .ToList();

                context.Books.RemoveRange(books);

                var book1 = new Book
                {
                    Id = 1,
                    CopiesCount = 10,
                    Price = 100
                };

                var book2 = new Book
                {
                    Id = 2,
                    CopiesCount = 10,
                    Price = 150
                };

                context.Books.Add(book1);
                context.Books.Add(book2);
                context.SaveChanges();
            }
        }

        [Test]
        public void SellBooks_BasketWithExistingBook_CorrectCostCalculated()
        {
            //Arrange
            var loggerMock = new Mock<ILogger>();
            var notifierMock = new Mock<INotifier>();

            var basket = new Dictionary<int, uint>();

            basket.Add(1, 2);
            basket.Add(2, 1);

            //Act
            var sut = new BooksService(
                loggerMock.Object,
                notifierMock.Object,
                _contextFactoryMethod);

            var result = sut.SellBooks(basket);

            //Arrange
            Assert.AreEqual(350, result);

            using (var context = _contextFactoryMethod())
            {
                var book1 = context.Books.First(x => x.Id == 1);
                Assert.AreEqual(8, book1.CopiesCount);

                var book2 = context.Books.First(x => x.Id == 2);
                Assert.AreEqual(9, book2.CopiesCount);
            }

            notifierMock.Verify(x => x.Notify(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SellBooks_EmptyBasket_ZeroCostNotNotification()
        {
            //Arrange
            var loggerMock = new Mock<ILogger>();
            var notifierMock = new Mock<INotifier>();

            var basket = new Dictionary<int, uint>();

            //Act
            var sut = new BooksService(
                loggerMock.Object,
                notifierMock.Object,
                _contextFactoryMethod);

            var result = sut.SellBooks(basket);

            //Arrange
            Assert.AreEqual(0, result);

            using (var context = _contextFactoryMethod())
            {
                var book1 = context.Books.First(x => x.Id == 1);
                Assert.AreEqual(10, book1.CopiesCount);

                var book2 = context.Books.First(x => x.Id == 2);
                Assert.AreEqual(10, book2.CopiesCount);
            }

            notifierMock.Verify(x => x.Notify(It.IsAny<string>()), Times.Never);
        }
    }
}
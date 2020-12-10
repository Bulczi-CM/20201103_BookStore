using System;
using System.Collections.Generic;
using System.Threading;
using BookStore.BusinessLayer;
using BookStore.DataLayer.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookStore.Tests.BusinessLayer
{
    internal class BookRepositoryMock : IBookRepository
    {
        public Book GetBookById(int id)
        {
            if(id == 1)
                return new Book(null, "pierwsza", 10);
            if(id == 2)
                return new Book(null, "druga", 15);
            if(id == 3)
                return new Book(null, "trzeca", 20);
            else
                return new Book(null, "czwarta", 25);
        }

        public void Update(Book book)
        {
            
        }
    }
    
    internal class BookRepositoryMockSameAsGeneratedByMoq : IBookRepository // new Mock<IBookRepository>()
    {
        public Book GetBookById(int id)
        {
            return null;
        }

        public void Update(Book book)
        {
            
        }
    }

    public class BookServiceTests
    {
        [TestCase(0, 0)]
        [TestCase(2, 20)]
        [TestCase(3, 30)]
        [TestCase(500, 5000)]
        public void SellBooks_MultipleCopiesOfOneBookCosting_ReturnsCorrectCost(int quantity, double expectedCost)
        {
            Dictionary<int, uint> basket = new Dictionary<int, uint>();
            basket.Add(1, (uint)quantity);
            var service = new BooksService(new BookRepositoryMock());

            double cost = service.SellBooks(basket);

            cost.Should().Be(expectedCost);
        }

        [Test]
        public void SellBooks_ThreeDifferentBooks_ReturnsCorrectCost()
        {
            Dictionary<int, uint> basket = new Dictionary<int, uint>();
            basket.Add(1, 1);
            basket.Add(2, 1);
            basket.Add(3, 1);
            var service = new BooksService(new BookRepositoryMock());

            double cost = service.SellBooks(basket);

            cost.Should().Be(45);
        }

        
        [TestCase(0, 0)]
        [TestCase(2, 20)]
        [TestCase(3, 30)]
        [TestCase(500, 5000)]
        public void SellBooks_MultipleCopiesOfOneBookCosting_ReturnsCorrectCost_WithMock(int quantity, double expectedCost)
        {
            Dictionary<int, uint> basket = new Dictionary<int, uint>();
            basket.Add(1, (uint)quantity);

            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock
                .Setup(repo => repo.GetBookById(1))
                .Returns(new Book(null, "pierwsza", 10));

            IBookRepository bookRepository = bookRepositoryMock.Object;

            var service = new BooksService(bookRepository);

            double cost = service.SellBooks(basket);

            cost.Should().Be(expectedCost);
        }

        [Test]
        public void SellBooks_ThreeDifferentBooks_ReturnsCorrectCost_WithMock()
        {
            Dictionary<int, uint> basket = new Dictionary<int, uint>();
            basket.Add(1, 1);
            basket.Add(2, 1);
            basket.Add(3, 1);
            var service = new BooksService(new BookRepositoryMock());

            double cost = service.SellBooks(basket);

            cost.Should().Be(45);
        }

        [Test]
        public void MockingPlayground()
        {
            var bookRepositoryMock = new Mock<IBookRepository>();
            
            bookRepositoryMock
                .Setup(repo => repo.GetBookById(1))
                .Returns(new Book(null, "pierwsza", 10));

            bookRepositoryMock
                .Setup(repo => repo.GetBookById(2))
                .Throws(new AbandonedMutexException("asd"));

            bookRepositoryMock
                .Setup(repo => repo.GetBookById(3))
                .Callback(() => Console.WriteLine("kto� pobiera ksi��k� 3!"));

            bookRepositoryMock
                .Setup(repo => repo.GetBookById(It.IsAny<int>()))
                .Returns(new Book(null, "jaka� inna", 50));

            Book book3 = bookRepositoryMock.Object.GetBookById(3);

            Book book1 = bookRepositoryMock.Object.GetBookById(1);

            Book book500 = bookRepositoryMock.Object.GetBookById(500);

            book500.Title.Should().Be("jaka� inna");
            // Book book2 = bookRepositoryMock.Object.GetBookById(2);
        }
    }
}
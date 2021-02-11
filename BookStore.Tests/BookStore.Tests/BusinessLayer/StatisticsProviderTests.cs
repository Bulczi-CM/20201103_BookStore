using BookStore.BusinessLayer;
using BookStore.DataLayer.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BookStore.Tests.BusinessLayer
{
    public class StatisticsProviderTests
    {
        private Mock<IBookStoreService> _bookStoreServiceMock;
        private StatisticsProvider _sut;

        [SetUp]
        public void SetUp()
        {
            //Arrange
            var booksOffer1 = new List<BookStoreBook>
            {
                new BookStoreBook { Book = new Book { Genre = BookGenre.Drama } },
                new BookStoreBook { Book = new Book { Genre = BookGenre.Drama } },
                new BookStoreBook { Book = new Book { Genre = BookGenre.Bio   } }
            };

            var booksOffer3 = new List<BookStoreBook>
            {
                new BookStoreBook { Book = new Book { Genre = BookGenre.CrimeStory } },
                new BookStoreBook { Book = new Book { Genre = BookGenre.CrimeStory } },
                new BookStoreBook { Book = new Book { Genre = BookGenre.CrimeStory } }
            };

            _bookStoreServiceMock = new Mock<IBookStoreService>();

            _bookStoreServiceMock
                .Setup(x => x.GetAllBookStoresIds())
                .Returns(new List<int>() { 1, 3 });

            _bookStoreServiceMock
                .Setup(x => x.GetOffer(1))
                .Returns(booksOffer1);

            _bookStoreServiceMock
                .Setup(x => x.GetOffer(3))
                .Returns(booksOffer3);
            
            _sut = new StatisticsProvider(_bookStoreServiceMock.Object);
        }

        [Test]
        public void MostPopularGenre_BookStoreWithBooksFromManyGenres_ValidGenreReturned()
        {
            //Act
            var result = _sut.GetMostPopularGenre();

            //Assert
            Assert.AreEqual(BookGenre.CrimeStory, result);
        }
    }
}
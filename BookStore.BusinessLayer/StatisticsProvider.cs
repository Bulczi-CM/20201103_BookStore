using System.Collections.Generic;
using System.Linq;
using BookStore.DataLayer.Models;

namespace BookStore.BusinessLayer
{
    public class StatisticsProvider
    {
        private readonly IBookStoreService _bookStoreService;

        public StatisticsProvider(IBookStoreService bookStoreService)
        {
            _bookStoreService = bookStoreService;
        }

        public BookGenre GetMostPopularGenre()
        {
            var bookStoresIds = _bookStoreService.GetAllBookStoresIds();

            var genresBooksCount = new Dictionary<BookGenre, int>();

            foreach (int bookStoreId in bookStoresIds)
            {
                var offer = _bookStoreService.GetOffer(bookStoreId);

                var groupedBooks = offer.GroupBy(x => x.Book.Genre);

                foreach (var group in groupedBooks)
                {
                    if(genresBooksCount.ContainsKey(group.Key))
                    {
                        genresBooksCount[group.Key] += group.Count();
                    }
                    else
                    {
                        genresBooksCount[group.Key] = group.Count();
                    }
                }
            }

            KeyValuePair<BookGenre, int> mostPopularGenreGroup = new KeyValuePair<BookGenre, int>();

            foreach (var group in genresBooksCount)
            {
                if (mostPopularGenreGroup.Value < group.Value)
                {
                    mostPopularGenreGroup = group;
                }
            }

            return mostPopularGenreGroup.Key;
        }
    }
}
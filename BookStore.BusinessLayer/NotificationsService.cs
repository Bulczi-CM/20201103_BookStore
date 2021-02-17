using BookStore.DataLayer.Models;
using EventStore.Client;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer
{
    public interface INotificationsService
    {
        Task NotifyNewBookArrivalAsync(Book newBook);
    }

    public class NotificationsService : INotificationsService
    {
        public async Task NotifyNewBookArrivalAsync(Book newBook)
        {
            const string stream = "bookstore-newbook-stream";
            const int defaultPort = 2113;

            var settings = EventStoreClientSettings.Create($"esdb://127.0.0.1:{defaultPort}?Tls=false");

            using (var client = new EventStoreClient(settings))
            {
                await client.AppendToStreamAsync(
                    stream,
                    StreamState.Any,
                    new[] { GetEventDataFor(newBook) });
            }
        }

        private static EventData GetEventDataFor(Book data)
        {
            return new EventData(
                Uuid.NewUuid(),
                "new-book-arrival",
                JsonSerializer.SerializeToUtf8Bytes(data));
        }
    }
}
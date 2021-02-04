using BookStore.DataLayer.Models;
using EventStore.Client;
using System.Text.Json;

namespace BookStore.BusinessLayer
{
    public interface INotificationsService
    {
        void NotifyNewBookArrival(Book newBook);
    }

    public class NotificationsService : INotificationsService
    {
        public void NotifyNewBookArrival(Book newBook)
        {
            const string stream = "bookstore-newbook-stream";
            const int defaultPort = 2113;

            var settings = EventStoreClientSettings.Create($"esdb://127.0.0.1:{defaultPort}?Tls=false");

            using (var client = new EventStoreClient(settings))
            {
                client.AppendToStreamAsync(
                    stream,
                    StreamState.Any,
                    new[] { GetEventDataFor(newBook) }).Wait();
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
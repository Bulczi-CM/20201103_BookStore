using BookStore.NotficationsSender.Models;
using EventStore.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.NotficationsSender
{
    class Program
    {
        //TODO Add possiblity to register notification for given users for selected authors and save it to database
        //TODO When new book arived, check for notifications registrations in databse and send notificcation using appropriate channel
        static void Main(string[] args)
        {
            const string stream = "bookstore-newbook-stream";
            const int defaultPort = 2113;

            var settings = EventStoreClientSettings.Create($"esdb://127.0.0.1:{defaultPort}?Tls=false");

            using (var client = new EventStoreClient(settings))
            {
                client.SubscribeToStreamAsync(
                    stream,
                    StreamPosition.End,
                    EventArrived);

                Console.WriteLine("Press any key to close...");
                Console.ReadLine();
            }
        }

        private async static Task EventArrived(
            StreamSubscription subscription,
            ResolvedEvent resolvedEvent,
            CancellationToken cancellationToken)
        {
            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());

            var book = JsonConvert.DeserializeObject<Book>(jsonData);

            Console.WriteLine(jsonData);
        }
    }
}
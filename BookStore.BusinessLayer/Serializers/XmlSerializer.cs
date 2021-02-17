using BookStore.DataLayer.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookStore.BusinessLayer.Serializers
{
    public class XmlDataSerializer : IDataSerializer
    {
        public string FileExtension => "xml";

        public async Task SerializeAsync(string filePath, List<BookStoreBook> dataSet)
        {
            var task = new Task(() =>
            {
                var serializer = new XmlSerializer(typeof(List<BookStoreBook>));

                using (TextWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, dataSet);
                }
            });

            task.Start();
            await task;
        }

        public async Task<List<BookStoreBook>> DeserializeAsync(string filePath)
        {
            var task = new Task<List<BookStoreBook>>(() =>
            {
                var serializer = new XmlSerializer(typeof(List<BookStoreBook>));
                List<BookStoreBook> peopleSet;

                using (TextReader reader = new StreamReader(filePath))
                {
                    var dataObj = serializer.Deserialize(reader);
                    peopleSet = (List<BookStoreBook>)dataObj;
                }

                return peopleSet;
            });

            task.Start();
            return await task;
        }
    }
}
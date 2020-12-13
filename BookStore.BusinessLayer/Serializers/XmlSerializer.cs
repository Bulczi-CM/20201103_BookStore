using BookStore.DataLayer.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BookStore.BusinessLayer.Serializers
{
    public class XmlDataSerializer : IDataSerializer
    {
        public string FileExtension => "xml";

        public void Serialize(string filePath, List<BookStoreBook> dataSet)
        {
            var serializer = new XmlSerializer(typeof(List<BookStoreBook>));

            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, dataSet);
            }
        }

        public List<BookStoreBook> Deserialize(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<BookStoreBook>));
            List<BookStoreBook> peopleSet;

            using (TextReader reader = new StreamReader(filePath))
            {
                var dataObj = serializer.Deserialize(reader);
                peopleSet = (List<BookStoreBook>)dataObj;
            }

            return peopleSet;
        }
    }
}
using BookStore.DataLayer.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BookStore.BusinessLayer.Serializers
{
    public class JsonSerializer : IDataSerializer
    {
        public string FileExtension => "json";

        public void Serialize(string filePath, List<BookStoreBook> dataSet)
        {
            var jsonData = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }

        public List<BookStoreBook> Deserialize(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var offer = JsonConvert.DeserializeObject<List<BookStoreBook>>(jsonData);

            return offer;
        }
    }
}
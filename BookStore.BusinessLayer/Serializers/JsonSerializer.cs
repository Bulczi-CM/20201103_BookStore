using BookStore.DataLayer.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer.Serializers
{
    public class JsonSerializer : IDataSerializer
    {
        public string FileExtension => "json";

        public async Task SerializeAsync(string filePath, List<BookStoreBook> dataSet)
        {
            var jsonData = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, jsonData);
        }

        public async Task<List<BookStoreBook>> DeserializeAsync(string filePath)
        {
            var jsonData = await File.ReadAllTextAsync(filePath);
            var offer = JsonConvert.DeserializeObject<List<BookStoreBook>>(jsonData);

            return offer;
        }
    }
}
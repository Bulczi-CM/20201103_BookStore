using BookStore.DataLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer.Serializers
{
    public interface IDataSerializer
    {
        string FileExtension { get; }
        Task SerializeAsync(string filePath, List<BookStoreBook> dataSet);
        Task <List<BookStoreBook>> DeserializeAsync(string filePath);
    }
}
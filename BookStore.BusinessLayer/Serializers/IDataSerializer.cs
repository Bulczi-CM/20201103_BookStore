using BookStore.DataLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLayer.Serializers
{
    public interface IDataSerializer
    {
        string FileExtension { get; }
        void Serialize(string filePath, List<BookStoreBook> dataSet);
        List<BookStoreBook> Deserialize(string filePath);
    }
}
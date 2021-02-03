using System;

namespace BookStore.BusinessLayer.Serializers
{
    public enum SerializationFormat
    {
        Json,
        Xml,
        Csv
    }

    public interface IDataSerializersFactory
    {
        IDataSerializer Create(SerializationFormat format);
    }

    public class DataSerializersFactory : IDataSerializersFactory
    {
        public IDataSerializer Create(SerializationFormat format)
        {
            switch (format)
            {
                case SerializationFormat.Json:
                    return new JsonSerializer();
                case SerializationFormat.Xml:
                    return new XmlDataSerializer();
                case SerializationFormat.Csv:
                    return new CsvSerializer();
                default:
                    throw new Exception("Unknown serialization format");
            }
        }
    }
}
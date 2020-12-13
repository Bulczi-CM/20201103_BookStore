using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookStore.DataLayer.Models
{
    public class Bookstore
    {
        public int Id { get; set; }
        [XmlElement("StoreName")]
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore
{
    public class Book
    {
        public string Author;
        public string Title;
        public bool HardCover;
        public string Description;
        public uint PagesCount;
        public string Genre; //zamienic na enum potem
        public DateTime PublishDate;
        public string Publisher;
        public float Price;
        public uint CopiesCount;

        public bool IsAvailable
        {
            get
            {
                return CopiesCount > 0;
            }
        }

        public Book()
        {
        }

        public Book(string author, string title, float price)
        {
            Author = author;
            Title = title;
            Price = price;
        }
    }
}

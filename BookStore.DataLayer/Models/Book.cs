using System;

namespace BookStore.DataLayer.Models
{
    public enum BookGenre
    {
        Drama = 0,
        SciFi = 1,
        CrimeStory = 2,
        Horror = 3,
        Bio = 4
    }

    public class Book
    {

        public Author Author;
        public string Title;
        public bool HardCover;
        public string Description;
        public uint PagesCount;
        public BookGenre Genre; //zamienic na enum potem
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

        public Book(Author author, string title, float price)
        {
            Author = author;
            Title = title;
            Price = price;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.NotficationsSender.Models
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
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string Title { get; set; }
        public bool HardCover { get; set; }
        public string Description { get; set; }
        public uint PagesCount { get; set; }
        public BookGenre Genre { get; set; }
        public DateTime PublishDate { get; set; }
        public string Publisher { get; set; }
        public float Price { get; set; }
        public uint CopiesCount { get; set; }
    }
}
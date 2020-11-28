namespace BookStore.DataLayer.Models
{
    public class BookStoreBook
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int BookStoreId { get; set; }
        public Bookstore BookStore { get; set; }
    }
}
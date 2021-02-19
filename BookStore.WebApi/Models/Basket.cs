using System.Collections.Generic;

namespace BookStore.WebApi.Models
{
    public class Basket
    {
        public List<SellItem> Items { get; set; }
    }
}
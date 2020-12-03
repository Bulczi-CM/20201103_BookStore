using BookStore.DataLayer;
using BookStore.DataLayer.Models;

namespace BookStore.BusinessLayer
{
    public class UsersService
    {
        public void Add(User user)
        {
            using(var context = new BookStoresDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
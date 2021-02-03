using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using System;

namespace BookStore.BusinessLayer
{
    public interface IUsersService
    {
        void Add(User user);
    }

    public class UsersService : IUsersService
    {
        private Func<IBookStoresDbContext> _bookStoresDbContextFactoryMethod;

        public UsersService(
            Func<IBookStoresDbContext> bookStoresDbContextFactoryMethod)
        {
            _bookStoresDbContextFactoryMethod = bookStoresDbContextFactoryMethod;
        }

        public void Add(User user)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
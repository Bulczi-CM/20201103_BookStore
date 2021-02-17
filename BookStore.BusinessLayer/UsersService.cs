using BookStore.DataLayer;
using BookStore.DataLayer.Models;
using System;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer
{
    public interface IUsersService
    {
        Task AddAsync(User user);
    }

    public class UsersService : IUsersService
    {
        private Func<IBookStoresDbContext> _bookStoresDbContextFactoryMethod;

        public UsersService(
            Func<IBookStoresDbContext> bookStoresDbContextFactoryMethod)
        {
            _bookStoresDbContextFactoryMethod = bookStoresDbContextFactoryMethod;
        }

        public async Task AddAsync(User user)
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
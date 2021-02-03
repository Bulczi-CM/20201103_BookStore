using BookStore.DataLayer;
using System;

namespace BookStore.BusinessLayer
{
    public interface IDatabaseManagementService
    {
        void EnsureDatabaseCreation();
    }

    public class DatabaseManagementService : IDatabaseManagementService
    {
        private Func<IBookStoresDbContext> _bookStoresDbContextFactoryMethod;

        public DatabaseManagementService(Func<IBookStoresDbContext> bookStoresDbContextFactoryMethod)
        {
            _bookStoresDbContextFactoryMethod = bookStoresDbContextFactoryMethod;
        }

        public void EnsureDatabaseCreation()
        {
            using (var context = _bookStoresDbContextFactoryMethod())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
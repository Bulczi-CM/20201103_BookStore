using BookStore.DataLayer;

namespace BookStore.BusinessLayer
{
    public class DatabaseManagementService
    {
        public void EnsureDatabaseCreation()
        {
            using (var context = new BookStoresDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
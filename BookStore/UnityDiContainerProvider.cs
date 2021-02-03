using BookStore.BusinessLayer;
using BookStore.BusinessLayer.Serializers;
using BookStore.DataLayer;
using Serilog;
using System;
using Unity;
using Unity.Injection;

namespace BookStore
{
    public class UnityDiContainerProvider
    {
        public IUnityContainer GetContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IIoHelper, IoHelper>();
            container.RegisterType<IMenu, Menu>();

            container.RegisterType<IBooksService, BooksService>();
            container.RegisterType<IAuthorsService, AuthorsService>();
            container.RegisterType<IBookStoreService, BookStoreService>();
            container.RegisterType<IDatabaseManagementService, DatabaseManagementService>();
            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<INotificationsService, NotificationsService>();

            container.RegisterType<INotifier, Notifier>();
            container.RegisterType<IDataSerializersFactory, DataSerializersFactory>();

            container.RegisterType<IBookRepository, BookRepository>();
            container.RegisterType<Func<IBookStoresDbContext>>(
                new InjectionFactory(ctx => new Func<IBookStoresDbContext>(() => new BookStoresDbContext())));

            container.RegisterInstance<ILogger>(Log.Logger);

            return container;
        }
    }
}
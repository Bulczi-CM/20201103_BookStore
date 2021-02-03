using BookStore.BusinessLayer;
using BookStore.BusinessLayer.Serializers;
using BookStore.DataLayer;
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

            container.RegisterType<IBookRepository, BookRepository>();
            container.RegisterType<INotifier, Notifier>();
            container.RegisterType<Func<IBookStoresDbContext>>(
                new InjectionFactory(ctx => new Func<IBookStoresDbContext>(() => new BookStoresDbContext())));
            container.RegisterType<IDataSerializersFactory, DataSerializersFactory>();

            return container;
        }
    }
}
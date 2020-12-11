using System;

namespace BookStore.BusinessLayer
{
    public interface INotifier
    {
        public void Notify(string s);
    }

    public class Notifier : INotifier
    {
        public void Notify(string s)
        {
            Console.WriteLine(s);
        }
    }
}

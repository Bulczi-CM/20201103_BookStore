﻿using BookStore.DataLayer.Models;
using System;

namespace BookStore.BusinessLayer
{
    public enum CommunicationChannel
    {
        Email,
        PhoneNotification
    }

    public class NotificationsService
    {
        public EventHandler<NewBookEventArgs> SubscribedNotification;

        public void AddNotification(string authorSurname, CommunicationChannel channel)
        {
            switch (channel)
            {
                case CommunicationChannel.Email:
                    SubscribedNotification += SendEmailChannelNotification;
                    break;
                case CommunicationChannel.PhoneNotification:
                    SubscribedNotification += PopPhoneNotification;
                    break;
                default:
                    break;
            }
        }

        private void SendEmailChannelNotification(object sender, NewBookEventArgs e)
        {
            Console.WriteLine($"Sending e-mail notification about new books to users: {e.Book.Title}");
        }

        private void PopPhoneNotification(object sender, NewBookEventArgs e)
        {
            Console.WriteLine($"Poping up phone notification about new books to users: {e.Book.Title}");
        }

        public void ClearNotifications()
        {
            SubscribedNotification = null;
        }
    }

    public class NewBookEventArgs
    {
        public Book Book;
    }
}
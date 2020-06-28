using System;

namespace ShoppingCart.Api.Infrastructures.Exceptions
{
    public class NotificationException : Exception
    {
        public string Notification { get; set; }
        public string Subject { get; private set; }
        /// <param name="notification">Bilgilendirme Mesajı</param>
        /// <param name="subject">Konu</param>
        /// <returns></returns>
        public NotificationException(string notification, string subject = "Warning") : base(notification)
        {
            this.Notification = notification;
            this.Subject = subject;
        }

        public NotificationException(Exception innerException, string notification, params object[] args)
            : base(string.Format(notification, args), innerException)
        {
            Notification = notification;
        }
    }
}
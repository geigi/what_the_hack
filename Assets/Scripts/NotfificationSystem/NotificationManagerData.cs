using System;
using System.Collections.Generic;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Data class for NotificationManager
    /// </summary>
    [Serializable]
    public class NotificationManagerData
    {
        /// <summary>
        /// List of all notifications
        /// </summary>
        public List<Notification> Notifications;

        /// <summary>
        /// List of all displayed notification
        /// </summary>
        public List<Notification> DisplayedNotifications =>
            Notifications.FindAll(notification => notification.Displayed);

        public NotificationManagerData()
        {
            Notifications = new List<Notification>();
        }
    }
}

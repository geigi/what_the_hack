using System;
using System.Collections.Generic;

namespace Assets.Scripts.NotificationSystem
{
    [Serializable]
    public class NotificationManagerData
    {
        public List<Notification> Notifications;

        public List<Notification> DisplayedNotifications =>
            Notifications.FindAll(notification => notification.Displayed);

        public NotificationManagerData()
        {
            Notifications = new List<Notification>();
        }
    }
}

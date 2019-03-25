using System;
using System.Collections.Generic;

namespace Assets.Scripts.NotificationSystem
{
    [Serializable]
    public class NotificationCenterData
    {
        public List<Notification> Notifications;

        public List<Notification> DisplayedNotifications =>
            Notifications.FindAll(notification => notification.Displayed);

        public NotificationCenterData()
        {
            Notifications = new List<Notification>();
        }
    }
}

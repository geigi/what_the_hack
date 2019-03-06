using System;
using GameTime;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class which holds all data for a Notification.
    /// </summary>
    [Serializable]
    public class Notification
    {
        /// <summary>
        /// Enum for the different types of notification.
        /// </summary>
        public enum NotificationType
        {
            Info,
            Warning,
            Fail,
            Success
        }

        private readonly string message;

        /// <summary>
        /// Message the notification should display.
        /// </summary>
        public string Message => message;

        private readonly NotificationType category;

        /// <summary>
        /// The type of notification.
        /// </summary>
        public NotificationType Category => category;

        private readonly GameDate date;

        /// <summary>
        /// Date for this notification.
        /// </summary>
        public GameDate Date => date;

        /// <summary>
        /// True iff this notification has already been displayed.
        /// </summary>
        public bool Displayed = false;

        /// <summary>
        /// Creates a new Notification Object.
        /// </summary>
        /// <param name="message">Message of the new Notification</param>
        /// <param name="category">Category of the new Notification</param>
        /// <param name="date">Date of the new Notification. (Will be cloned for the Object.)</param>
        public Notification(string message, NotificationType category, GameDate date)
        {
            this.message = message;
            this.category = category;
            this.date = date.Clone() as GameDate;
        }
    }
}

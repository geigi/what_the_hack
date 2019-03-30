using Base;
using GameSystem;
using Interfaces;
using SaveGame;
using UE.Events;
using NotificationType = Assets.Scripts.NotificationSystem.Notification.NotificationType;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class for managing Notifications
    /// </summary>
    public class NotificationManager : Singleton<NotificationManager>, ISaveable<NotificationManagerData>
    {
        /// <summary>
        /// Object which holds data.
        /// </summary>
        private NotificationManagerData notificationData;

        public NetObjectEvent NewNotification;

        /// <summary>
        /// Initialize the notification system.
        /// Restores the notifications if a savegame is present.
        /// </summary>
        public void Awake()
        {
            if (GameSettings.NewGame)
                Initialize();
            else
                LoadState();
        }

        /// <summary>
        /// Initialize the default state.
        /// </summary>
        private void Initialize()
        {
            notificationData = new NotificationManagerData();
        }

        /// <summary>
        /// Restore the state of this object from the current savegame.
        /// </summary>
        private void LoadState()
        {
            var saveGame = SaveGameSystem.Instance.GetCurrentSaveGame();
            notificationData = saveGame.NotificationManagerData;
        }

        /// <summary>
        /// Send an info notification
        /// </summary>
        /// <param name="message">Message of the notification</param>
        public virtual void Info(string message) => newNotification(message, NotificationType.Info);

        /// <summary>
        /// Sends a success notification
        /// </summary>
        /// <param name="message">Message of the notification</param>
        public void Success(string message) => newNotification(message, NotificationType.Success);
        
        /// <summary>
        /// Sends a fail notification
        /// </summary>
        /// <param name="message">Message of the notification</param>
        public void Fail(string message) => newNotification(message, NotificationType.Fail);
        
        /// <summary>
        /// Sends a warning notification
        /// </summary>
        /// <param name="message">Message of the notification</param>
        public void Warning(string message) => newNotification(message, NotificationType.Warning);
        
        /// <summary>
        /// Sends a notification 
        /// </summary>
        /// <param name="message">Message of the notification</param>
        /// <param name="type">notification type</param>
        private void newNotification(string message, NotificationType type)
        {
            Notification notification = new Notification(message, type, GameTime.GameTime.Instance.GetData().Date);
            notificationData.Notifications.Add(notification);
            NewNotification.Raise(notification);
        }

        /// <inheritdoc />
        public NotificationManagerData GetData() => notificationData;
    }
}

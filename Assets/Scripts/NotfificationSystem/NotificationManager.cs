using Base;
using GameSystem;
using Interfaces;
using SaveGame;
using UE.Events;
using NotificationType = Assets.Scripts.NotificationSystem.Notification.NotificationType;

namespace Assets.Scripts.NotificationSystem
{

    public class NotificationManager : Singleton<NotificationManager>, ISaveable<NotificationManagerData>
    {

        private NotificationManagerData notificationData;

        public NetObjectEvent NewNotification;

        /// <summary>
        /// Initialize the game time system.
        /// Restores the game time if a savegame is present.
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

        public virtual void Info(string message)
        {
            newNotification(message, NotificationType.Info);
        }

        public void Success(string message)
        {
            newNotification(message, NotificationType.Success);
        }

        public void Fail(string message)
        {
            newNotification(message, NotificationType.Fail);
        }

        public void Warning(string message)
        {
            newNotification(message, NotificationType.Warning);
        }

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

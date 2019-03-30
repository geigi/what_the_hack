using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class which holds logic for NotificationView GameObject
    /// </summary>
    public class NotificationView : MonoBehaviour
    {
        public GameObject NotificationUIPrefab;
        public GameObject Content;
        /// <summary>
        /// List of all notifications to display.
        /// </summary>
        private List<Notification> displayedNotifications = new List<Notification>();

        /// <summary>
        /// Shows all notifications of the displayedNotification list
        /// </summary>
        public void Show()
        {
            foreach (var current in displayedNotifications)
            {
                var element = Instantiate(NotificationUIPrefab);
                element.GetComponent<NotificationUIElement>()
                    .SetNotification(current.Message, current.Date, current.GetCorrespondingIcon());
                element.transform.parent = Content.transform;
                element.transform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// Only show all already displayed notification.
        /// Show() needs to be called afterwards.
        /// </summary>
        public void DisplayedNotifications() =>
            displayedNotifications = NotificationManager.Instance.GetData().DisplayedNotifications;

        /// <summary>
        /// Show a specific list of notification.
        /// Show() needs to be called afterwards.
        /// </summary>
        /// <param name="notifications"></param>
        public void SpecificNotifications(List<Notification> notifications) =>
            displayedNotifications = notifications;

        /// <summary>
        /// Destroys all child gameObject of content.
        /// </summary>
        public void OnDisable()
        {
            for (var i = 0; i < Content.transform.childCount; i++)
            {
                GameObject.Destroy(Content.transform.GetChild(i).gameObject);
            }
        }
    }
}

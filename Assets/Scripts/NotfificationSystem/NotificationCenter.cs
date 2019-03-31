using UnityEngine;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class which holds logic, for the NotificationCenter GameObject.
    /// </summary>
    public class NotificationCenter : MonoBehaviour
    {
        private RectTransform rectTrans;
        private NotificationView view;

        void Awake()
        {
            rectTrans = gameObject.GetComponent<RectTransform>();
            view = transform.GetChild(0).GetComponent<NotificationView>();
        }

        /// <summary>
        /// Tells NotificationView, to show all already displayed Notifications.
        /// </summary>
        private void OnEnable()
        {
            view.DisplayedNotifications();
            view.Show();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class which holds logic, for the NotificationCenter GameObject.
    /// </summary>
    public class NotificationCenter : MonoBehaviour
    {
        private RectTransform rectTrans;
        private NotificationView view;
        private ScrollRect scrollRect;

        void Awake()
        {
            rectTrans = gameObject.GetComponent<RectTransform>();
            view = transform.GetChild(0).GetComponent<NotificationView>();
            scrollRect = GetComponentInChildren<ScrollRect>();
        }

        /// <summary>
        /// Tells NotificationView, to show all already displayed Notifications.
        /// </summary>
        private void OnEnable()
        {
            view.DisplayedNotifications();
            view.Show();
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
}

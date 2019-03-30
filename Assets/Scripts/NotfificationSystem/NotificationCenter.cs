using UnityEngine;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class which holds logic, for the NotificationCenter GameObject.
    /// </summary>
    public class NotificationCenter : MonoBehaviour
    {

        private RectTransform rectTrans;

        void Awake()
        {
            rectTrans = gameObject.GetComponent<RectTransform>();
        }
        /// <summary>
        /// Checks if the mouse was clicked outside of this GameObject and closes the NotifiactionCenter
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!rectTrans.rect.Contains(rectTrans.InverseTransformPoint(Input.mousePosition)))
                {
                    gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Tells NotificationView, to show all already displayed Notifications.
        /// </summary>
        private void OnEnable()
        {
            transform.GetChild(0).GetComponent<NotificationView>().DisplayedNotifications();
            transform.GetChild(0).GetComponent<NotificationView>().Show();
        }
    }
}

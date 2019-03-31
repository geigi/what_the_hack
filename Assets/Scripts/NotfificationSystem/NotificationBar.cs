using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class that holds the logic for NotificationBar GameObject.
    /// </summary>
    public class NotificationBar : MonoBehaviour, IPointerDownHandler
    {
        /// <summary>
        /// NotificationCenter GameObject
        /// </summary>
        public GameObject NotificationCenter;

        public SingleNotification SingleNotification;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            SingleNotification.Stop();
            NotificationCenter.SetActive(true);
        }
    }
}

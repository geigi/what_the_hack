using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class which holds the logic for NotificationUIElement GameObject.
    /// </summary>
    public class NotificationUIElement : MonoBehaviour
    {
        public GameObject Icon;
        public GameObject Date;
        public GameObject NotificationText;

        /// <summary>
        /// Displays a Notification
        /// </summary>
        /// <param name="notificationText">Message of the notification</param>
        /// <param name="date">Date of the notification</param>
        /// <param name="icon">Icon of the notification</param>
        public void SetNotification(string notificationText, GameTime.GameDate date, Sprite icon)
        {
            NotificationText.GetComponent<Text>().text = notificationText;
            if (Date != null)
                Date.GetComponent<Text>().text = date.DateTime.ToShortDateString();
            Icon.GetComponent<Image>().sprite = icon;
        }
    }
}

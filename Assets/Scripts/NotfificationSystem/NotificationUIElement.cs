using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.NotfificationSystem
{
    public class NotificationUIElement : MonoBehaviour
    {
        public GameObject Icon;
        public GameObject Date;
        public GameObject NotificationText;

        public void SetNotification(string notificationText, GameTime.GameDate date, Sprite icon)
        {
            NotificationText.GetComponent<Text>().text = notificationText;
            if(Date != null)
                Date.GetComponent<Text>().text = date.DateTime.ToShortDateString();
            Icon.GetComponent<Image>().sprite = icon;
        }
    }
}

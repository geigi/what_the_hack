using System;
using System.Collections;
using System.Collections.Generic;
using UE.Events;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class which holds logic, for SingleNotification Prefab
    /// </summary>
    public class SingleNotification : MonoBehaviour
    {
        public Text text;
        public Image icon;
        public NetObjectEvent newNotification;
        public GameObject NotificationBar;
        /// <summary>
        /// How long to show the notification, after scrolling has finished.
        /// </summary>
        public float xSeconds = 1f;
        /// <summary>
        /// Time for the notificationBar animation
        /// </summary>
        public float animationTime = 2f;
        /// <summary>
        /// Height of the fully extended notificationBar
        /// </summary>
        public int maxHeight = 40;

        /// <summary>
        /// Queue to hold all incoming notifications.
        /// </summary>
        internal Queue<Notification> notifications = new Queue<Notification>();
        private UnityAction<object> notificationAction;
        internal Rect notificationBarRect;
        internal bool notificationBarUp = false;
        private UnityAction evtAction;

        private bool notificationBeingDisplayed = false;
        private bool scrollingFinished = true;

        private Coroutine displayRoutine;

        private TextBanner banner => text.GetComponent<TextBanner>();

        void Awake()
        {
            evtAction += ScrollingFinished;
            text.GetComponent<TextBanner>().ScrollingFinished.AddListener(evtAction);
            Clear();
            notificationBarRect = NotificationBar.GetComponent<RectTransform>().rect;
            notificationAction += SetNotification;
            newNotification.AddListener(notificationAction);
        }

        /// <summary>
        /// Instructs this gameObject to show the notification
        /// </summary>
        /// <param name="notification">Notification to be shown</param>
        public void SetNotification(object notification)
        {
            notifications.Enqueue(notification as Notification);
            if (!notificationBarUp)
            {
                StartCoroutine(NotificationBarUp());
            }
        }

        /// <summary>
        /// Animates extension of NotificationBar and shows the notificationQueue.
        /// </summary>
        /// <returns></returns>
        private IEnumerator NotificationBarUp()
        {
            notificationBarUp = true;
            Clear();
            while (notificationBarRect.height < maxHeight)
            {
                var stepChange = (Time.deltaTime / animationTime) * maxHeight;
                if (notificationBarRect.height + stepChange >= maxHeight)
                {
                    notificationBarRect.height = maxHeight;
                }
                else
                    notificationBarRect.height += stepChange;

                var sizeDelta = NotificationBar.GetComponent<RectTransform>().sizeDelta;
                NotificationBar.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(sizeDelta.x, notificationBarRect.height);
                yield return null;
            }

            displayRoutine = StartCoroutine(ShowNotificationQueue());
        }

        /// <summary>
        /// This function only exists, so a test class can call the coroutine, because it does not work otherwise.
        /// </summary>
        public void up() => StartCoroutine(NotificationBarUp());

        /// <summary>
        /// This function only exists, so a test class can call the coroutine, because it does not work otherwise.
        /// </summary>
        public void down() => StartCoroutine(NotificationBarDown());

        /// <summary>
        /// Stop the notification bar display process.
        /// </summary>
        public void Stop()
        {
            banner.Set("");
            StopCoroutine(displayRoutine);
            notificationBeingDisplayed = false;
            scrollingFinished = true;
            StartCoroutine(NotificationBarDown());

            while (notifications.Count > 0)
            {
                var n = notifications.Dequeue();
                n.Displayed = true;
            }
        }

        /// <summary>
        /// Animates the retraction of NotificationBar
        /// </summary>
        /// <returns></returns>
        private IEnumerator NotificationBarDown()
        {
            notificationBarUp = false;
            Clear();
            
            while (notificationBarRect.height > 0)
            {
                var stepChange = (Time.deltaTime / animationTime) * maxHeight;
                if (notificationBarRect.height - stepChange <= 0)
                {
                    notificationBarRect.height = 0;
                }
                else
                    notificationBarRect.height -= stepChange;

                var sizeDelta = NotificationBar.GetComponent<RectTransform>().sizeDelta;
                NotificationBar.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(sizeDelta.x, notificationBarRect.height);
                yield return null;
            }
        }

        /// <summary>
        /// Clears text and icon from this gameObject.
        /// </summary>
        private void Clear()
        {
            text.text = "";
            icon.sprite = null;
            icon.color = new Color(0, 0, 0, 0); // icon needs to be invisible, otherwise the color is shown. 
        }

        /// <summary>
        /// Displays all notifications in the queue
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowNotificationQueue()
        {
            icon.color = new Color(255, 255, 255, 1);
            while (notificationBeingDisplayed)
            {
                yield return new WaitForSeconds(1);
            }
            notificationBeingDisplayed = true;
            while (notifications.Count > 0)
            {
                var current = notifications.Dequeue();
                icon.sprite = current.GetCorrespondingIcon();
                scrollingFinished = false;
                current.Displayed = true;
                banner.Set(current.Message);
                while (!scrollingFinished)
                {
                    yield return new WaitForSeconds(1);
                }

                yield return new WaitForSeconds(xSeconds);
            }

            notificationBeingDisplayed = false;
            StartCoroutine(NotificationBarDown());
            displayRoutine = null;
        }

        /// <summary>
        /// Scrolling of the notification message is finished.
        /// </summary>
        internal void ScrollingFinished() => this.scrollingFinished = true;
    }
}

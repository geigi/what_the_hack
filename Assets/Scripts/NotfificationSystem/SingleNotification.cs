using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts.NotificationSystem;
using UE.Events;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SingleNotification : MonoBehaviour
{
    public Text text;
    public Image icon;
    public NetObjectEvent newNotification;
    public GameObject NotificationBar;
    public float xSeconds = 1f;
    public float animationTime = 2f;
    public int maxHeight = 40;

    private Queue<Notification> notifications = new Queue<Notification>();
    private UnityAction<object> notificationAction;
    private Rect notificationBarRect;
    private bool notificationBarUp = false;
    private UnityAction evtAction;

    private bool notificationBeingDisplayed = false;
    private bool scrollingFinished = true;

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

    public void SetNotification(object notification)
    {
        notifications.Enqueue(notification as Notification);
        if (!notificationBarUp)
        {
            StartCoroutine(NotificationBarUp());
        }
    }

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

            NotificationBar.GetComponent<RectTransform>().sizeDelta = new Vector2(notificationBarRect.width, notificationBarRect.height);
            yield return null;
        }
            StartCoroutine(ShowNotificationQueue());
    }

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

            NotificationBar.GetComponent<RectTransform>().sizeDelta = new Vector2(notificationBarRect.width, notificationBarRect.height);
            yield return null;
        }
    }

    private void Clear()
    {
        text.text = "";
        icon.sprite = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    private IEnumerator ShowNotificationQueue()
    {
        icon.color = new Color(255, 255, 255, 1); 
        while (notificationBeingDisplayed)
        {
            yield return new WaitForSeconds(1);
        }
        notificationBeingDisplayed = true;
        var banner = text.GetComponent<TextBanner>();
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
    }

    private void ScrollingFinished() => this.scrollingFinished = true;
    
}

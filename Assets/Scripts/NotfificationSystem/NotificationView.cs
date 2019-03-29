using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.NotfificationSystem;
using Assets.Scripts.NotificationSystem;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class NotificationView : MonoBehaviour
{
    public GameObject NotificationUIPrefab;
    public GameObject Content;
    private List<Notification> displayedNotifications = new List<Notification>();

    public void Show()
    {
        foreach (var current in displayedNotifications)
        {
            var element = Instantiate(NotificationUIPrefab);
            element.GetComponent<NotificationUIElement>().SetNotification(current.Message, current.Date, current.GetCorrespondingIcon());
            element.transform.parent = Content.transform;
            element.transform.localScale = Vector3.one;
        }
    }

    public void ShowDisplayedNotifications() =>
        displayedNotifications = NotificationManager.Instance.GetData().DisplayedNotifications;

    public void ShowSpecificNotifications(List<Notification> notifications) => displayedNotifications = notifications;

    public void OnDisable()
    {
        for (var i = 0; i < Content.transform.childCount; i++)
        {
            GameObject.Destroy(Content.transform.GetChild(i).gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.NotfificationSystem;
using Assets.Scripts.NotificationSystem;
using UnityEngine;

public class NotificationView : MonoBehaviour
{
    public GameObject NotificationUIPrefab;
    public GameObject Content;
    private List<Notification> displayedNotifications => NotificationCenter.Instance.GetData().DisplayedNotifications;

    private void Start()
    {
        foreach (var current in displayedNotifications)
        {
            var element = Instantiate(NotificationUIPrefab);
            element.transform.parent = Content.transform;
            element.GetComponent<NotificationUIElement>().SetNotification(current.Message, current.Date, current.GetCorrespondingIcon());
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.y > gameObject.GetComponent<RectTransform>().rect.height)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

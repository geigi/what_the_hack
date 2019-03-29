using UnityEngine;

public class NotificationCenter : MonoBehaviour
{
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

    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<NotificationView>().ShowDisplayedNotifications();
        transform.GetChild(0).GetComponent<NotificationView>().Show();
    }
}

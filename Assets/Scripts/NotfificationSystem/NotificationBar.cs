using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationBar : MonoBehaviour
{
    public GameObject NotificationCenter;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.y < gameObject.GetComponent<RectTransform>().rect.height)
            {
                NotificationCenter.SetActive(true);
            }
        }
    }
}

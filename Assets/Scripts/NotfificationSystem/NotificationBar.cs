using UnityEngine;

namespace Assets.Scripts.NotificationSystem
{
    /// <summary>
    /// Class that holds the logic for NotificationBar GameObject.
    /// </summary>
    public class NotificationBar : MonoBehaviour
    {
        /// <summary>
        /// NotificationCenter GameObject
        /// </summary>
        public GameObject NotificationCenter;

        private RectTransform rectTrans;

        void Awake()
        {
            rectTrans = gameObject.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Checks if this GameObject was clicked and opens the NotificationCenter
        /// </summary>
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (rectTrans.rect.Contains(rectTrans.InverseTransformPoint(Input.mousePosition)))
                {
                    NotificationCenter.SetActive(true);
                }
            }
        }
    }
}

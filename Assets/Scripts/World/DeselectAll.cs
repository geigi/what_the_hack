using UnityEngine;
using UnityEngine.EventSystems;

namespace World
{
    public class DeselectAll : MonoBehaviour, IPointerDownHandler
    {
        public GameObject NotificationCenter;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            GameSelectionManager.Instance.ClearEmployee();
            GameSelectionManager.Instance.ClearWorkplace();
            AudioPlayer.Instance.PlayDeselect();
            NotificationCenter.SetActive(false);
        }
    }
}
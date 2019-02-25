using UnityEngine;
using UnityEngine.EventSystems;

namespace World
{
    public class DeselectAll : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            GameSelectionManager.Instance.ClearEmployee();
            GameSelectionManager.Instance.ClearWorkplace();
            AudioPlayer.Instance.PlayDeselect();
        }
    }
}
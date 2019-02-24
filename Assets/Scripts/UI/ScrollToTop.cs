using UnityEngine;

namespace UI
{
    /// <summary>
    /// Scrolls a rect view to the top on start.
    /// </summary>
    public class ScrollToTop : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<RectTransform>().position = Vector3.zero;
        }
    }
}
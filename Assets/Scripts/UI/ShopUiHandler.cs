using Extensions;
using UnityEngine;

namespace UI
{
    public class ShopUiHandler : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<RectTransform>().ResetPosition();
        }
    }
}
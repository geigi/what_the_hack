using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class DisplayVersionNumber : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Text>().text = Application.version;
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// This component can be attached to a UI game component, that should float to a given anchor GameObject in 3D space.
    /// This component makes sure, that it can not leave the screen.
    /// You can specify an offset.
    /// NOTE: It's important that the pivot point is exactly in the middle of the UI component.
    /// </summary>
    public class FloatWindow : MonoBehaviour, IDragHandler
    {
        /// <summary>
        /// UI Object that will float
        /// </summary>
        public GameObject Window;
        /// <summary>
        /// Canvas the UI element is displayed in
        /// </summary>
        public Canvas Canvas;
        /// <summary>
        /// CanvasScaler the UI is displayed in
        /// </summary>
        public CanvasScaler Scaler;
        /// <summary>
        /// This offset will be added to the calculated screen position.
        /// </summary>
        public Vector2 Offset;

        private bool selected = false;
        private GameObject anchor;
        private RectTransform rect;
        private Vector2 defaultOffset;

        private void Awake()
        {
            defaultOffset = new Vector2(Offset.x, Offset.y);
            rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (selected)
            {
                // First calculate the screen position of the given anchor position
                var position = Camera.main.WorldToScreenPoint(anchor.transform.position);
                Vector3 positionFinal;
                positionFinal = position + new Vector3(Offset.x, Offset.y, 0);
                
                // Now test if we are leaving the screen
                var sizeDelta = rect.rect;
                var left = positionFinal.x - sizeDelta.width * Canvas.scaleFactor / 2;
                var right = positionFinal.x + sizeDelta.width * Canvas.scaleFactor / 2;
                var top = positionFinal.y - sizeDelta.height * Canvas.scaleFactor / 2;
                var bottom = positionFinal.y + sizeDelta.height * Canvas.scaleFactor / 2;

                // If we leave the screen, correct our position accordingly
                if (left < 0)
                {
                    positionFinal += new Vector3(-left, 0, 0);
                }

                if (right > Screen.width)
                {
                    positionFinal -= new Vector3(right - Screen.width, 0, 0);
                }

                if (top < 0)
                {
                    positionFinal += new Vector3(0, -top, 0);
                }

                if (bottom > Screen.height)
                {
                    positionFinal -= new Vector3(0, bottom - Screen.height, 0);
                }
                
                // Apply final postion
                Window.transform.position = positionFinal;
            }
        }

        /// <summary>
        /// Select a anchor where the UI element should float.
        /// Displays the UI object.
        /// </summary>
        /// <param name="gameobject">Anchor for the UI object</param>
        public void Select(GameObject gameobject)
        {
            anchor = gameobject;
            Canvas.enabled = true;
            Scaler.enabled = true;

            Offset.x = defaultOffset.x;
            Offset.y = defaultOffset.y;
            selected = true;
        }

        /// <summary>
        /// Remove anchor and hide UI object.
        /// </summary>
        public void Deselect()
        {
            selected = false;
            
            Canvas.enabled = false;
            Scaler.enabled = false;
            
            anchor = null;
        }

        /// <summary>
        /// This method handles dragging.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            var anchorPosition = Camera.main.WorldToScreenPoint(anchor.transform.position);
            var position = Window.transform.position;
            Offset.x = position.x - anchorPosition.x;
            Offset.y = position.y - anchorPosition.y;
            Offset += eventData.delta;
        }
    }
}
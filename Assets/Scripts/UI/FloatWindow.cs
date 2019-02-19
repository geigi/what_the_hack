using UE.StateMachine;
using UnityEngine;
using UnityEngine.Events;
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
        public Vector2 DefaultOffset;
        /// <summary>
        /// Top margin for the window.
        /// </summary>
        public int TopMargin;
        /// <summary>
        /// The employee window is drawed as a screen space overlay.
        /// Therefore the popup needs to be hidden when such a ui object is currently drawn.
        /// </summary>
        /// <returns></returns>
        public State ScreenSpaceOverlayGameObject;

        private bool selected = false;
        private GameObject anchor;
        private RectTransform rect;
        private Vector2 offset;
        private UnityAction<State> overlayEnterAction;
        private UnityAction<State, State> overlayLeaveAction;
        private bool tempHidden = false;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            overlayEnterAction = OnStateEntered;
            overlayLeaveAction = OnStateLeaved;
            ScreenSpaceOverlayGameObject.stateManager.AddStateEnterListener(overlayEnterAction);
            ScreenSpaceOverlayGameObject.stateManager.AddStateLeaveListener(overlayLeaveAction);
        }

        private void Start()
        {
            Canvas.enabled = false;
            Scaler.enabled = false;
        }

        private void Update()
        {
            if (selected)
                setPosition();
        }

        private void setPosition()
        {
            // First calculate the screen position of the given anchor position
            var position = Camera.main.WorldToScreenPoint(anchor.transform.position);
            Vector3 positionFinal;
            positionFinal = position + new Vector3(offset.x, offset.y, 0);

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

            if (bottom > Screen.height - TopMargin)
            {
                positionFinal -= new Vector3(0, bottom - Screen.height + TopMargin, 0);
            }

            // Apply final postion
            Window.transform.position = positionFinal;
        }

        /// <summary>
        /// Select a anchor where the UI element should float.
        /// Displays the UI object.
        /// </summary>
        /// <param name="gameobject">Anchor for the UI object</param>
        public void Select(GameObject gameobject)
        {
            anchor = gameobject;
            
            Scaler.enabled = true;
            
            var scaleFactor = Canvas.scaleFactor;
            offset.x = DefaultOffset.x * scaleFactor;
            offset.y = DefaultOffset.y * scaleFactor;
            
            setPosition();
            
            Canvas.enabled = true;
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
        /// Reset the temporarily hidden state.
        /// </summary>
        public void ResetTempHidden()
        {
            tempHidden = false;
        }

        /// <summary>
        /// This method handles dragging.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            var anchorPosition = Camera.main.WorldToScreenPoint(anchor.transform.position);
            var position = Window.transform.position;
            offset.x = position.x - anchorPosition.x;
            offset.y = position.y - anchorPosition.y;
            offset += eventData.delta;
        }

        private void OnStateEntered(State state)
        {
            if (state == ScreenSpaceOverlayGameObject && Canvas.enabled)
            {
                tempHidden = true;
                Canvas.enabled = false;
                Scaler.enabled = false;
            }
        }

        private void OnStateLeaved(State stateLeaved, State stateEntered)
        {
            if (stateLeaved == ScreenSpaceOverlayGameObject && tempHidden)
            {
                tempHidden = false;
                Canvas.enabled = true;
                Scaler.enabled = true;
            }
        }
    }
}
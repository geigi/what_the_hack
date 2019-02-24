using UE.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// This behaviour enters a state on click.
    /// </summary>
    public class OnClickSetState : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public State State;


        public void OnPointerUp(PointerEventData eventData)
        {
            State.Enter();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
    }
}
using Extensions;
using UE.Common;
using UE.StateMachine;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class InteractionWindow : MonoBehaviour
    {
        public State MainState;
        private MissionHook interaction;
        private UnityAction<bool> completed;

        private void Awake()
        {
            completed = onHookFinished;
        }

        public void SetInteraction(MissionHook interaction)
        {
            gameObject.transform.DestroyChildren();
            var go = Instantiate(interaction.GUIPrefab, gameObject.transform, true);
            var rect = go.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.ResetPosition();
            
            interaction.Completed.AddListener(completed);
            this.interaction = interaction;
        }

        private void onHookFinished(bool success)
        {
            interaction.Completed.RemoveListener(completed);
            gameObject.transform.DestroyChildren();
            interaction = null;
            MainState.Enter();
        }
    }
}
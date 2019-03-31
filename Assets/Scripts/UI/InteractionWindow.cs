using Extensions;
using UE.Common;
using UE.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.UIElements;

namespace UI
{
    public class InteractionWindow : MonoBehaviour
    {
        public State MainState;
        public ScrollView scrollView;
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
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            
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
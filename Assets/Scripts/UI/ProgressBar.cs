using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// This component brings progressbar functionality to a Unity Slider.
    /// </summary>
    public class ProgressBar : MonoBehaviour
    {
        public Slider Slider;
        public Image FillSprite;
        public Color InProgressColor;
        public Color CompletedColor;
        
        private UnityAction<float> progressAction;

        private void Awake()
        {
            progressAction = SetProgress;
        }

        /// <summary>
        /// Set the progress.
        /// </summary>
        /// <param name="progress">0f is no progress, 1f is 100%</param>
        public void SetProgress(float progress)
        {
            Slider.value = progress;

            if (progress < 1f)
                FillSprite.color = InProgressColor;
            else
                FillSprite.color = CompletedColor;
        }
    }
}
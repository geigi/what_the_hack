using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// This component creates a running banner text out of a text field
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class TextBanner : MonoBehaviour
    {
        [Range(0.1f, 3.0f)]
        public float Speed = 1.0f;
        /// <summary>
        /// Number of maximum showed characters.
        /// If the text is shorter, no scrolling will happen.
        /// </summary>
        public int MaxCharacters = 5;
        /// <summary>
        /// Loop the animation?
        /// </summary>
        public bool Loop = false;
        /// <summary>
        /// Gets fired when scrolling has finished.
        /// Only when <see cref="Loop"/> is false.
        /// </summary>
        public UnityEvent ScrollingFinished;

        private string completeText;
        private int position = 0;
        private Text textComponent;
        private Coroutine coroutine;
        private bool awaitStart = false;
        
        private void Awake()
        {
            textComponent = GetComponent<Text>();
        }

        private void OnEnable()
        {
            if (awaitStart)
            {
                awaitStart = false;
                coroutine = StartCoroutine(updateText());
            }
        }

        /// <summary>
        /// Set the text and start the animation.
        /// </summary>
        /// <param name="text"></param>
        public void Set(string text)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            completeText = text;
            position = 0;

            //Text now always scrolls, so this check is not needed.
            /* 
            if (text.Length <= MaxCharacters)
            {
                textComponent.text = text;
                ScrollingFinished.Invoke();
                return;
            }
            */

            textComponent.text = text.Substring(0, completeText.Length);

            if (gameObject.activeInHierarchy)
                coroutine = StartCoroutine(updateText());
            else
                awaitStart = true;
        }

        /// <summary>
        /// Get the complete text of this text banner.
        /// </summary>
        /// <returns>Complete text</returns>
        public string GetComplete()
        {
            return completeText;
        }

        /// <summary>
        /// The animation coroutine starts here.
        /// It loops the animation or fires the scrolling finished event.
        /// </summary>
        /// <returns></returns>
        private IEnumerator updateText()
        {
            yield return new WaitForSeconds(1); //wait xSeconds at the beginning.
            if (Loop)
            {
                completeText = completeText + new string(' ', completeText.Length);
                while (true)
                {
                    yield return MoveText();
                    position = 0;
                    textComponent.text = completeText.Substring(position);
                }
            }
            else
            {
                yield return MoveText();
                ScrollingFinished.Invoke();
            }
            
            yield return null;
        }

        /// <summary>
        /// Animate the text.
        /// Moves the complete text forward by one char at a time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveText()
        {
            while (position < completeText.Length)
            {
                yield return new WaitForSeconds(Speed);
                position++;
                textComponent.text = completeText.Substring(position); // The text that is shown, gets smaller and smaller, till the length is 0
            }
        }
    }
}
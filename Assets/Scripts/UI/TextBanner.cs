using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public enum TextBannerMode
    {
        Normal,
        CharacterWidth
    }

    /// <summary>
    /// This component creates a running banner text out of a text field
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class TextBanner : MonoBehaviour
    {
        [Range(0.1f, 3.0f)]
        public float Speed = 1.0f;
        [Range(0, 10)]
        public int BeginPause = 1;
        /// <summary>
        /// Defines, whether the max characters are set manually
        /// or calculated from character width and length.
        /// </summary>
        public TextBannerMode Mode;
        /// <summary>
        /// Number of maximum showed characters.
        /// If the text is shorter, no scrolling will happen.
        /// </summary>
        public int MaxCharacters = 5;
        /// <summary>
        /// Defines the width of a character. Only needed, when character mode is set.
        /// </summary>
        public int CharacterWidth = 15;
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
        private RectTransform rect;
        private Coroutine coroutine;
        private bool awaitStart = false;
        
        private void Awake()
        {
            textComponent = GetComponent<Text>();
            rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (Mode == TextBannerMode.CharacterWidth)
                StartCoroutine(calculateMaxCharacters());
        }

        private IEnumerator calculateMaxCharacters()
        {
            // We need to wait one frame because the layout scripts need to do their work in the first frame.
            yield return new WaitForEndOfFrame();
            MaxCharacters = (int) (rect.rect.width / CharacterWidth);
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

            if (text.Length <= MaxCharacters)
            {
                textComponent.text = text;
                ScrollingFinished.Invoke();
                return;
            }

            textComponent.text = text.Substring(0, MaxCharacters);

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
            if (Loop)
            {
                while (true)
                {
                    yield return new WaitForSeconds(BeginPause);
                    yield return MoveText();
                    yield return new WaitForSeconds(BeginPause);
                    position = 0;
                    textComponent.text = completeText.Substring(position, MaxCharacters);
                }
            }
            else
            {
                yield return new WaitForSeconds(BeginPause);
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
            while (position + MaxCharacters < completeText.Length)
            {
                yield return new WaitForSeconds(Speed);
                position++;
                textComponent.text = completeText.Substring(position, MaxCharacters);
            }
        }
    }
}
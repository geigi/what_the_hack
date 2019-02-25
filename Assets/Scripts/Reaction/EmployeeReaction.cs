using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Reaction
{
    /// <summary>
    /// Object for an reaction.
    /// </summary>
    public class EmployeeReaction : MonoBehaviour
    {
        /// <summary>
        /// Shows the emoji.
        /// </summary>
        public  SpriteRenderer reactionRenderer;

        /// <summary>
        /// Offset of the reaction
        /// </summary>
        public Vector3 offset = Vector3.zero; 

        /// <summary>
        /// Position of the reaction with offset
        /// </summary>
        public Vector3 Position
        {
            get => gameObject.transform.position;
            set => gameObject.transform.position = value + offset;
            
        }

        void Awake()
        {
            reactionRenderer = gameObject.AddComponent<SpriteRenderer>();
            reactionRenderer.sortingOrder = 100;
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("EmployeeReactions").transform;
        }

        /// <summary>
        /// Shows the specified reaction
        /// </summary>
        /// <param name="reaction">Sprite of the reaction</param>
        public void ShowReaction(Sprite reaction)
        {
            reactionRenderer.sprite = reaction;
            gameObject.transform.position = Position;
        }
    }
}
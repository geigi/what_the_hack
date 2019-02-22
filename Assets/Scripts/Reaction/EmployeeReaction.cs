using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Reaction
{
    public class EmployeeReaction : MonoBehaviour
    {
        public  SpriteRenderer reactionRenderer;

        public Vector3 offset = Vector3.zero; 

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

        public void ShowReaction(Sprite reaction, Vector3 position)
        {
            reactionRenderer.sprite = reaction;
            gameObject.transform.position = position;
        }
    }
}
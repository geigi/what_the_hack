using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Wth.ModApi
{
    /// <summary>
    /// A Class for a Skill Scriptable Object.
    /// </summary>
    [CreateAssetMenu(menuName = "What_The_Hack ModApi/Skills/Skill Definition")]
    public class SkillDefinition : ScriptableObject
    {
        /// <summary>
        /// The skills name.
        /// </summary>
        public string skillName;

        /// <summary>
        /// The skills image / sprite.
        /// </summary>
        public Sprite skillSprite;
    }
}
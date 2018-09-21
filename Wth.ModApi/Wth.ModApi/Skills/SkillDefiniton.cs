using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Wth.ModApi
{
    [CreateAssetMenu(menuName = "What_The_Hack ModApi/Skills/Skill Definition")]
    public class SkillDefinition : ScriptableObject
    {
        //Name des Skills
        public string skillName;

        //Sprite des Skills
        public Sprite skillSprite;
    }
}
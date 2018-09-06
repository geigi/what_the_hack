using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Wth.ModApi
{
    public class SkillDefinition : ScriptableObject
    {
        //Name des Skills
        public String skillName;

        //Sprite des Skills
        public Sprite skillSprite;

        //Auftrittswahrscheinlichkeit des Skills
        [Range(0f, 1f)] public float percentage;
    }
}
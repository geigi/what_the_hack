using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi
{
    [CreateAssetMenu(fileName = "SkillSet", menuName = "What_The_Hack ModApi/Skills/Skill Set", order = 1)]
    public class SkillSet : ScriptableObject
    {
        public List<SkillDefinition> keys;
        public List<float> values;
    }
}
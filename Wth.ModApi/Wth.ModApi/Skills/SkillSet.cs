using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi
{
    /// <summary>
    /// A Class which holds a Set of Skills, with theire respective occurence probability
    /// </summary>
    [CreateAssetMenu(fileName = "SkillSet", menuName = "What_The_Hack ModApi/Skills/Skill Set", order = 1)]
    public class SkillSet : ScriptableObject
    {
        /// <summary>
        /// A list of the skills itself.
        /// </summary>
        public List<SkillDefinition> keys;
        /// <summary>
        /// A list of the occurence probability for each skill.
        /// </summary>
        public List<float> values;
    }
}
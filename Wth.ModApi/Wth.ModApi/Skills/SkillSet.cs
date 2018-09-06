using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi
{
    public class SkillSet : ScriptableObject
    {
        // Dictionary mit SkillDefinition als Key und deren Auftrittswahrscheinlichkeit als Value
        public Dictionary<SkillDefinition, float> skills = new Dictionary<SkillDefinition, float>();

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi
{
    public class SkillSet : ScriptableObject
    {
        public List<SkillDefinition> keys;
        public List<float> values;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi {
    [CreateAssetMenu(fileName = "ModInfo", menuName = "Mod/ModInfo", order = 1)]
    public class ModInfo : ScriptableObject {
        public Sprite banner;
        public SkillSet skillSet;
    }
}
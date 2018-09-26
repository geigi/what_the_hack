using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi {
    /// <summary>
    /// This Scriptable Object is the gathering point for the mod data that will be loaded by the base game.
    /// Everything that should be loaded by the base game needs to be in here.
    /// </summary>
    [CreateAssetMenu(fileName = "ModInfo", menuName = "What_The_Hack ModApi/Mod Info", order = 1)]
    public class ModInfo : ScriptableObject {
        [Header("Unique ID (required)")]
        [Space(10)]
        [Header("This asset contains references to important mod data")]
        [Tooltip("Each mod requires a unique id to identify within the game.")]
        public string id = "com.abc.myWthMod";

        [Header("Selection menu banner (required)")]
        [Tooltip("The image that will be displayed in the mod selection menu. Resolution: 400x182")]
        public Sprite banner;
        
        [Header("Custom Skill Set")]
        [Tooltip("If you created a custom skill set drop it here.")]
        public SkillSet skillSet;
        
        [Header("Custom Employees")]
        [Tooltip("If you created custom employees drop them here.")]
        public EmployeeList EmployeeList;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi.Employees;
using Wth.ModApi.Skills;
using Wth.ModApi.Tools;

namespace Wth.ModApi {
    /// <summary>
    /// This Scriptable Object is the gathering point for the mod data that will be loaded by the base game.
    /// Everything that should be loaded by the base game needs to be in here.
    /// </summary>
    [CreateAssetMenu(fileName = "ModInfo", menuName = "What_The_Hack ModApi/Mod Info", order = 1)]
    public class ModInfo : ScriptableObject {
        /// <summary>
        /// Each mod requires a unique id to identify within the game.
        /// Required for each mod.
        /// </summary>
        [Header("Unique ID (required)")]
        [Space(10)]
        [Header("This asset contains references to important mod data")]
        [Tooltip("Each mod requires a unique id to identify within the game.")]
        public string id = "com.abc.myWthMod";

        /// <summary>
        /// Each Scriptable Object that needs to be referenced in a save game needs to be included here.
        /// Required for each mod.
        /// </summary>
        [Header("Scriptable Object Dictionary (required)")]
        [Tooltip("Each Scriptable Object that needs to be referenced in a save game needs to be included here.")]
        public ScriptableObjectDictionary ScriptableObjectDictionary;

        /// <summary>
        /// The image that will be displayed in the mod selection menu. Resolution: 400x182
        /// </summary>
        [Header("Selection menu banner (required)")]
        [Tooltip("The image that will be displayed in the mod selection menu. Resolution: 400x182")]
        public Sprite banner;
        
        /// <summary>
        /// The mods custom <see cref="SkillSet"/>.
        /// </summary>
        [Header("Custom Skill Set")]
        [Tooltip("If you created a custom skill set drop it here.")]
        public SkillSet skillSet;
        
        /// <summary>
        /// The mods custom <see cref="EmployeeList"/>.
        /// </summary>
        [Header("Custom Employees")]
        [Tooltip("If you created custom employees drop them here.")]
        public EmployeeList EmployeeList;
    }
}
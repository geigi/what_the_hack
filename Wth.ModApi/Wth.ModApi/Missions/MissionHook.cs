using System;
using UnityEngine;
using UnityEngine.Events;

namespace Wth.ModApi.Missions
{
    /// <summary>
    /// This class represents a hook which can occur at any time in the mission progress.
    /// It can contain a GUI for the player to take extra actions.
    /// The hook has to fire HookSuccessful or HookFailed to continue the mission progress.
    /// </summary>
    [CreateAssetMenu(fileName = "MissionHook", menuName = "What_The_Hack ModApi/Missions/Mission Hook", order = -399)]
    public sealed class MissionHook: ScriptableObject
    {
        /// <summary>
        /// This GameObject contains the complete GUI.
        /// Be sure that the first element of the GUI is visible.
        /// Also all code that is required by the hook should be included as scripts
        /// and attached somewhere in the Prefab.
        /// </summary>
        public GameObject GUIPrefab;
        
        /// <summary>
        /// Fire this event when the hook has completed successfully.
        /// The mission will not continue until one of the hooks is fired.
        /// </summary>
        public event EventHandler HookSuccessful;
        
        /// <summary>
        /// Fire this event when the hook has failed.
        /// The mission will not continue until one of the hooks is fired.
        /// </summary>
        public event EventHandler HookFailed;
    }
}
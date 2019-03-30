using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi.Missions
{
    /// <summary>
    /// Contains all mission hooks of a mission.
    /// </summary>
    public class MissionHookList: ScriptableObject
    {
        public List<MissionHook> MissionHooks;
    }
}
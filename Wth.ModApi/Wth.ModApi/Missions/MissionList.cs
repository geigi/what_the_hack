using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi.Missions{
    /// <summary>
    /// This class contains a list of all custom missions for this mod.
    /// </summary>
    [CreateAssetMenu(fileName = "MissionList", menuName = "What_The_Hack ModApi/Missions/Mission List", order = -402)]
    public class MissionList : ScriptableObject
    {
        /// <summary>
        /// List of all custom employees.
        /// </summary>
        public List<MissionDefinition> missionList;
    }
}
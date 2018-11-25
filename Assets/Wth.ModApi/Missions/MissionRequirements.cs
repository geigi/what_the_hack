using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class contains required missions for another mission to appear.
/// </summary>
[CreateAssetMenu(fileName = "MissionRequirement", menuName = "What_The_Hack ModApi/Missions/Mission Requirements",
    order = -400)]
public class MissionRequirements : ScriptableObject
{
    /// <summary>
    /// List of required missions.
    /// </summary>
    public List<MissionDefinition> RequiredMissions = new List<MissionDefinition>();
}
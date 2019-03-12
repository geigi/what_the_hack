using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains a list of all custom missions for this mod.
/// </summary>
[CreateAssetMenu(fileName = "MissionList", menuName = "What_The_Hack ModApi/Missions/Mission List", order = -402)]
public class MissionList : ScriptableObject
{

    public enum DifficultyOption
    {
        Easy,
        Normal,
        Hard,
        Guru
    }

    /// <summary>
    /// List of all custom employees.
    /// </summary>
    public List<MissionDefinition> missionList;

    public string easyDifficultyDescription = "In this difficulty you can expect easy and straightforward missions.";
    public string normalDifficultyDescription = "In this difficulty you can expect everything from the previous " +
                                                "difficulty, as well as special missions on the topic of SQL-Injection.";
    public string hardDifficultyDescription = "In this difficulty you can expect everything from the previous " +
                                              "difficulties, as well as special missions on the topic of CrossSiteScripting.";
    public string guruDifficultyDescription = "In this difficulty you can expect everything from the previous " +
                                              "difficulties, as well as special missions on the topic of Secure Passwords.";
}
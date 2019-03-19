using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a mission which will be later shown in the mission manager.
/// In this class there is no progress data or any data that relates to a running game.
/// </summary>
[CreateAssetMenu(fileName = "Mission", menuName = "What_The_Hack ModApi/Missions/Mission Definition", order = -401)]
public sealed class MissionDefinition : ScriptableObject
{
    /// <summary>
    /// The title of this mission.
    /// </summary>
    public string Title = "";

    /// <summary>
    /// A description of this mission. This can be a longer text.
    /// Ideally this also contains some information about what is going to be hacked
    /// aswell as provide something that the player can learn in terms of IT security.
    /// This text can contain placeholders that will be replaced with random strings for:
    /// %COMPANY_NAME%, %PERSON_NAME%, %PERSON_SURNAME%, %CITY_NAME%.
    /// </summary>
    public string Description = "";

    /// <summary>
    /// The maximum timeframe for the mission in hours.
    /// This is also used to calculate the workflow for the employees.
    /// </summary>
    public int Deadline = 1;

    /// <summary>
    /// The text that will be displayed on a completed mission.
    /// </summary>
    public string MissionSucceeded = "Success!";

    /// <summary>
    /// The text that will be displayed when the mission fails.
    /// </summary>
    public string MissionFailed = "Failed :(";

    /// <summary>
    /// A list of 
    /// </summary>
    public List<SkillDefinition> SkillsRequired = new List<SkillDefinition>();

    /// <summary>
    /// The DifficultyOption of this level.
    /// </summary>
    public MissionList.DifficultyOption Difficulty = MissionList.DifficultyOption.Easy;

    /// <summary>
    /// The hardness of this level. Must be between 0.0-10.0.
    /// </summary>
    public float Hardness = 1.0f;

    /// <summary>
    /// A List of hooks that will be called in mission progress.
    /// This is optional.
    /// </summary>
    public List<MissionHook> MissionHooks = new List<MissionHook>();

    /// <summary>
    /// Required level of one employee for this mission to appear.
    /// "0" for no limitation.
    /// </summary>
    public int RequiredLevel = 0;

    /// <summary>
    /// Required number of currently employed Employees for this mission to appear.
    /// "0" for no limitation.
    /// </summary>
    public int RequiredEmployees = 0;

    /// <summary>
    /// Number of days after which this mission can first appear.
    /// "0" for no limitation.
    /// </summary>
    public int AppearAfterDays = 0;

    /// <summary>
    /// List of required missions for this mission to appear.
    /// Empty or null for no limitation.
    /// </summary>
    public MissionRequirements RequiredMissions;

    /// <summary>
    /// Instead of showing this mission randomly show it as soon as the requirements are met.
    /// Default: False.
    /// </summary>
    public bool ForceAppear = false;

    /// <summary>
    /// If the mission could not complete successfully, should it reappear in the available mission section?
    /// </summary>
    public bool ReApper = false;
}
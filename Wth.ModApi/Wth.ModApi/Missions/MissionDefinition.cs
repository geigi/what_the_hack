using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi.Skills;

namespace Wth.ModApi.Missions
{
    /// <summary>
    /// This class defines a mission which will be later shown in the mission manager.
    /// In this class there is no progress data or any data that relates to a running game.
    /// </summary>
    public sealed class MissionDefinition: ScriptableObject
    {
        /// <summary>
        /// The title of this mission.
        /// </summary>
        public string Title;
        
        /// <summary>
        /// A description of this mission. This can be a longer text.
        /// Ideally this also contains some information about what is going to be hacked
        /// aswell as provide something that the player can learn in terms of IT security.
        /// This text can contain placeholders that will be replaced with random strings for:
        /// %COMPANY_NAME%, %PERSON_NAME%, %PERSON_SURNAME%, %CITY_NAME%.
        /// </summary>
        public string Description;
        
        /// <summary>
        /// The maximum timeframe for the mission.
        /// This is also used to calculate the workflow for the employees.
        /// </summary>
        public int Deadline;
        
        /// <summary>
        /// The text that will be displayed on a completed mission.
        /// </summary>
        public string MissionSucceeded;
        
        /// <summary>
        /// The text that will be displayed when the mission fails.
        /// </summary>
        public string MissionFailed;
        
        /// <summary>
        /// A list of 
        /// </summary>
        public List<SkillDefinition> SkillsRequired;

        /// <summary>
        /// The Difficulty of this level. Must be 0-5.
        /// </summary>
        public int Difficulty;
        
        /// <summary>
        /// The hardness of this level. Must be between 0.0-10.0.
        /// </summary>
        public float Hardness;
        
        /// <summary>
        /// A List of hooks that will be called in mission progress.
        /// This is optional.
        /// </summary>
        public List<MissionHook> MissionHooks;
        
        /// <summary>
        /// Required level of one employee for this mission to appear.
        /// "0" for no limitation.
        /// </summary>
        public int RequiredLevel;
        
        /// <summary>
        /// Required number of currently employed Employees for this mission to appear.
        /// "0" for no limitation.
        /// </summary>
        public int RequiredEmployees;
        
        /// <summary>
        /// Number of days after which this mission can first appear.
        /// "0" for no limitation.
        /// </summary>
        public int AppearAfterDays;
        
        /// <summary>
        /// List of required missions for this mission to appear.
        /// Empty or null for no limitation.
        /// </summary>
        public List<MissionDefinition> RequiredMission;
        
        /// <summary>
        /// Instead of showing this mission randomly show it as soon as the requirements are met.
        /// Default: False.
        /// </summary>
        public bool ForceAppear;
    }
}
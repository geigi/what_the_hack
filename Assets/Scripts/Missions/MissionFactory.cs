using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Employees;
using Team;
using UnityEngine;
using Wth.ModApi.Names;
using Wth.ModApi.Tools;
using Random = UnityEngine.Random;

namespace Missions
{
    /// <summary>
    /// This class contains methods to generate various mission objects.
    /// </summary>
    public class MissionFactory: Singleton<MissionFactory>
    {
        public float MissionDifficultyVariance = 0.15f;
        public int MissionDurationMinimum = 2;
        public int MissionDurationVariance = 1;
        public float MissionRewardmoneyVariance = 0.1f;
        public int MissionRewardmoneyFactor = 20;
        public int MissionBasePower = 1;
        public float SkillPowerPerDifficulty = 3.5f;
        public float SkillDifficultyVariance = 0.3f;

        public float SKILL_GAME_PROGRESS_POWER = 0.1f;
        public float SKILL_DIFFICULTY_FACTOR = 1.5f;

        private MissionList missionList;
        private List<MissionDefinition> forceAppearMissions;

        private void Awake()
        {
            missionList = ModHolder.Instance.GetMissionList();
            
            if (missionList == null)
            {
                missionList = ContentHub.Instance.DefaultMissionList;
            }
            
            findForceAppearMissions();
        }

        /// <summary>
        /// Create a new random mission element with a given difficulty level.
        /// </summary>
        /// <param name="difficulty">Game progress</param>
        /// <returns>Random mission object</returns>
        public Mission CreateMission(float difficulty)
        {
            MissionDefinition definition;
            do
            {
                definition = missionList.missionList[Random.Range(0, missionList.missionList.Count)];
            } while (definition.ForceAppear || !RequirementsFullfilled(definition));

            var mission = new Mission(definition);
            
            SetPlaceholders(mission);
            SetGameProgress(difficulty, mission);

            return mission;
        }

        /// <summary>
        /// Calculates and sets all values that are dependent from the game progress.
        /// </summary>
        /// <param name="progress">Current game progress</param>
        /// <param name="mission"></param>
        public void SetGameProgress(float progress, Mission mission)
        {
            calcDurationVariance(mission);
            RandomSkillValues(mission, progress);

            generateOutcome(mission, progress);
        }

        /// <summary>
        /// Get a list of all missions that should force appear.
        /// Includes only missions which requirements are fulfilled.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public List<Mission> GetForceAppearMissions()
        {
            List<Mission> missions = new List<Mission>();
            
            foreach (var definition in forceAppearMissions)
            {
                var mission = new Mission(definition);
                SetPlaceholders(mission);
                missions.Add(mission);
            }

            return missions;
        }

        /// <summary>
        /// Searches for missions, that should always appear when the requirements are met
        /// and saves them in a separate list for easy access.
        /// </summary>
        private void findForceAppearMissions()
        {
            forceAppearMissions = missionList.missionList.Where(m => m.ForceAppear).ToList();
        }
        
        /// <summary>
        /// Calculate the duration for this mission.
        /// </summary>
        /// <param name="mission"></param>
        private void calcDurationVariance(Mission mission) {
            mission.Duration = Math.Max(MissionDurationMinimum, mission.Duration + RandomUtils.var(MissionDurationVariance));
            mission.RemainingTicks = mission.Duration * GameTime.GameTime.Instance.ClockSteps;
        }
        
        /// <summary>
        /// Generate outcome for a mission.
        /// </summary>
        /// <param name="mission"></param>
        private void generateOutcome(Mission mission, float progress) {
            mission.RewardMoney = (int) (((Math.Sqrt(mission.AverageSkillDifficulty * mission.Difficulty) * 1f + Math.Max(progress, 1f) * 2f ) * RandomUtils.mult_var(MissionRewardmoneyVariance) * MissionRewardmoneyFactor) * 10);
        }
        
        /// <summary>
        /// Generate skill difficulties for a given mission.
        /// </summary>
        /// <param name="mission"></param>
        /// <param name="baseDifficulty">Base difficulty that will be added to the calculated difficulty.</param>
        private void RandomSkillValues(Mission mission, float baseDifficulty)
        {
            mission.SkillDifficulty = new Dictionary<SkillDefinition, int>();
            List<SkillDefinition> skills = mission.Definition.SkillsRequired;

            float difficulty = RandomUtils.mult_var(0.2f);
            int numSkills = skills.Count;
            mission.Difficulty = difficulty;

            int difficultyPerSkill = TeamManager.Instance.GetRandomSkillValue(numSkills);
            foreach (var s in skills) {
                mission.SkillDifficulty.Add(s, difficultyPerSkill);
            }
        }
        
        /// <summary>
        /// Calculate the mission level.
        /// </summary>
        /// <param name="baseDifficulty">Base difficulty gets multiplied with calculated difficulty.</param>
        /// <param name="hardness"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private int calcMissionLevel(float baseDifficulty, float hardness, int duration) {
            return Math.Max(1, (int) (
                baseDifficulty
                * hardness
//                        * durationDifficultyFactor(duration)
                * RandomUtils.mult_var(MissionDifficultyVariance)));
        }
        
        private float durationDifficultyFactor(int duration) {

            return Math.Min(2, 1f + 2f / duration);
        }
        
        /// <summary>
        /// Generate random placeholder replacements for a given mission.
        /// </summary>
        /// <param name="mission"></param>
        private void SetPlaceholders(Mission mission)
        {
            var nameList = ContentHub.Instance.GetNameLists();
            
            mission.Replacements.Add("%CONTACT%", nameList.PersonName(PersonNames.UndecidedFullName));
            mission.Replacements.Add("%CONTACT_L%", nameList.PersonName(PersonNames.LastName));
            mission.Replacements.Add("%CONTACT_M%", nameList.PersonName(PersonNames.MaleFullName));
            mission.Replacements.Add("%CONTACT_F%", nameList.PersonName(PersonNames.FemaleFullName));
            mission.Replacements.Add("%COMPANY%", nameList.Company());
            mission.Replacements.Add("%PW_APPLICATION%", nameList.PasswordApplication());
            mission.Replacements.Add("%UNIVERSITY%", nameList.University());
            mission.Replacements.Add("%WEBSERVICE%", nameList.WebService());
            mission.Replacements.Add("%SOFTWARE%", nameList.Software());
            mission.Replacements.Add("%TOWN%", nameList.Town());
            mission.Replacements.Add("%COUNTRY%", nameList.Country());
            mission.Replacements.Add("%INSTITUTION%", nameList.Institution());
        }

        /// <summary>
        /// Tests whether a missions requirements are fulfilled or not.
        /// </summary>
        /// <returns></returns>
        public bool RequirementsFullfilled(MissionDefinition mission)
        {
            if (mission == null) return false;
            
            if (mission.RequiredLevel != 0 && 
                EmployeeManager.Instance.GetCurrentMaxEmployeeLevel() < mission.RequiredLevel)
            {
                return false;
            }

            if (mission.RequiredEmployees != 0 &&
                EmployeeManager.Instance.HiredEmployees < mission.RequiredEmployees)
            {
                return false;
            }

            foreach (var requirement in mission.RequiredMissions.RequiredMissions)
            {
                if (MissionManager.Instance.GetData().Completed.All(m => m.Definition != requirement))
                    return false;
            }
            
            return true;
        }
    }
}
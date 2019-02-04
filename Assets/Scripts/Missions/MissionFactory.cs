using System;
using System.Collections.Generic;
using Base;
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
        public int MissionBasePower = 18;
        public float SkillPowerPerDifficulty = 3.5f;
        public float SkillDifficultyVariance = 0.3f;

        private MissionList missionList;

        private void Awake()
        {
            missionList = ModHolder.Instance.GetMissionList();
            if (missionList == null)
            {
                missionList = ContentHub.Instance.DefaultMissionList;
            }
        }

        /// <summary>
        /// Create a new random mission element with a given difficulty level.
        /// </summary>
        /// <param name="difficulty">Game progress</param>
        /// <returns>Random mission object</returns>
        public Mission CreateMission(int difficulty)
        {
            MissionDefinition definition;
            do
            {
                definition = missionList.missionList[Random.Range(0, missionList.missionList.Count)];
            } while (definition.RequiredLevel > difficulty);

            var mission = new Mission(definition);
            
            SetPlaceholders(mission);
            calcDurationVariance(mission);
            RandomSkillValues(mission, difficulty);

            generateOutcome(mission);

            return mission;
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
        private void generateOutcome(Mission mission) {
            mission.RewardMoney = calcRewardMoney(mission);
        }
        
        /// <summary>
        /// Generate skill difficulties for a given mission.
        /// </summary>
        /// <param name="mission"></param>
        /// <param name="baseDifficulty">Base difficulty that will be added to the calculated difficulty.</param>
        private void RandomSkillValues(Mission mission, int baseDifficulty)
        {

            List<SkillDefinition> skills = mission.Definition.SkillsRequired;

            int difficulty = calcMissionLevel(baseDifficulty + MissionBasePower, mission.Definition.Hardness, mission.Duration);
            int numSkills = skills.Count;
            mission.Difficulty = difficulty;

            float difficultyPerSkill = calcSkillDifficulty(difficulty, numSkills);
            foreach (var s in skills) {
                mission.SkillDifficulty.Add(s, (int) (difficultyPerSkill * RandomUtils.mult_var(SkillDifficultyVariance)));
            }
        }
        
        /// <summary>
        /// Calculate the difficulty for a single skill.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="numSkills">Number of total skills required for the mission.</param>
        /// <returns></returns>
        private float calcSkillDifficulty(int difficulty, int numSkills) {
            return Math.Min(1, (difficulty * 2) / (float) (numSkills + 0.9)) * SkillPowerPerDifficulty;
        }
        
        /// <summary>
        /// Calculate the mission level.
        /// </summary>
        /// <param name="baseDifficulty">Base difficulty gets multiplied with calculated difficulty.</param>
        /// <param name="hardness"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private int calcMissionLevel(int baseDifficulty, float hardness, int duration) {
            return Math.Max(1, (int) (
                baseDifficulty
                * hardness
//                        * durationDifficultyFactor(duration)
                * RandomUtils.mult_var(MissionDifficultyVariance)));
        }
        
        /// <summary>
        /// Calculate the reward money for a given mission.
        /// </summary>
        /// <param name="mission"></param>
        /// <returns></returns>
        public int calcRewardMoney(Mission mission) {
            return (int) (Math.Sqrt(mission.Difficulty) * /*(1 + mission.getRisk()) * */ RandomUtils.mult_var(MissionRewardmoneyVariance) * MissionRewardmoneyFactor) * 10;
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
    }
}
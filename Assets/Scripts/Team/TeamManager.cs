using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using GameSystem;
using Interfaces;
using Items;
using Missions;
using SaveGame;
using UE.Events;
using UnityEngine;
using Wth.ModApi.Employees;
using Wth.ModApi.Tools;

namespace Team
{
    /// <summary>
    /// This class manages team specific stuff like calculating the game progress.
    /// </summary>
    public class TeamManager: Singleton<TeamManager>, ISaveable<TeamManagerData>
    {
        private const float GAME_PROGRESS_MONEY_FACTOR = 0.00001f;
        private const float MISSON_PROGRESS_MONEY_FACTOR = 0.8f;
        private float SKILL_GAME_PROGRESS_POWER = 0.3f;
        private float SKILL_DIFFICULTY_FACTOR = 1.25f;
        private float SkillPowerPerDifficulty = 3f;
        private float SkillDifficultyVariance = 0.3f;
    
        public int MaxFloors = 3;
        public IntEvent FloorsChangedEvent;
        public NetObjectEvent FirstLevelUpOccurred;
        public List<Workplace> Workplaces;
        
        private TeamManagerData data;

        private void Awake()
        {
            if  (GameSettings.NewGame)
                InitDefaultState();
            else
                LoadState();
        }

        /// <summary>
        /// Return the number of bought floors.
        /// </summary>
        /// <returns></returns>
        public int GetFloors()
        {
            return data.Floors;
        }

        /// <summary>
        /// Add a floor.
        /// </summary>
        /// <returns></returns>
        public bool AddFloor()
        {
            if (data.Floors < MaxFloors)
            {
                data.Floors += 1;
                FloorsChangedEvent.Raise(data.Floors);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Get the number of bought workplaces.
        /// </summary>
        /// <returns></returns>
        public int GetWorkplaces()
        {
            return data.Workplaces;
        }

        /// <summary>
        /// Add a workplace.
        /// </summary>
        /// <returns></returns>
        public bool AddWorkplace()
        {
            if (data.Workplaces < data.Floors * 4)
            {
                data.Workplaces += 1;
                Workplaces[data.Workplaces - 1].Enable(true);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Get the number of available workplaces (4 workplaces for each floor).
        /// </summary>
        /// <returns></returns>
        public int GetAvailableWorkplaces()
        {
            return data.Floors * 4;
        }

        public List<Workplace> GetWorkplacesWorkingOnMission(Mission mission)
        {
            return Workplaces.Where(w => w.Mission == mission).ToList();
        }

        private void LoadState()
        {
            var mainSaveGame = gameObject.GetComponent<SaveGameSystem>().GetCurrentSaveGame();
            data = mainSaveGame.teamManagerData;
        }

        private void InitDefaultState()
        {
            data = new TeamManagerData();
        }

        /// <summary>
        /// Calculate the current game progress.
        /// </summary>
        /// <returns></returns>
        public virtual float calcGameProgress()
        {
            float result = 0;

            result += ContentHub.Instance.bank.Balance * GAME_PROGRESS_MONEY_FACTOR;
            result += MissionManager.Instance.GetData().Completed.Count * MISSON_PROGRESS_MONEY_FACTOR;
            
            return result;
        }

        /// <summary>
        /// Calculates a random skill value to be used for freshmens or missions.
        /// </summary>
        /// <returns></returns>
        public virtual int GetRandomSkillValue(int skillCount)
        {
            var difficultyPerSkill = Math.Max(calcGameProgress() * SKILL_GAME_PROGRESS_POWER, 1f) * SKILL_DIFFICULTY_FACTOR / (skillCount + 2f) * SkillPowerPerDifficulty;
            return Math.Max(1, (int) (difficultyPerSkill * RandomUtils.mult_var(SkillDifficultyVariance)));
        }

        /// <summary>
        /// Employees must report a level up here.
        /// It notifies the user when the tutorial is enabled on the first level up.
        /// </summary>
        public void ReportLevelUp(EmployeeData emp)
        {
            if (data.FirstLevelUpOccurred) return;

            data.FirstLevelUpOccurred = true;
            FirstLevelUpOccurred.Raise(emp);
        }

        public TeamManagerData GetData()
        {
            data.WorkplaceDatas = new List<WorkplaceData>();
            data.WorkplaceDatas.AddRange(Workplaces.Select(w => w.GetData()));
            return data;
        }
    }
}
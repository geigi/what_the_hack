using System;
using System.Collections.Generic;
using GameSystem;
using Interfaces;
using Items;
using SaveGame;
using UE.Events;
using UnityEngine;

namespace Team
{
    /// <summary>
    /// This class manages team specific stuff like calculating the game progress.
    /// </summary>
    public class TeamManager: MonoBehaviour, ISaveable<TeamManagerData>
    {
        #region Singleton
        private static readonly Lazy<TeamManager> lazy = 
            new Lazy<TeamManager>(() => GameObject.FindWithTag("Managers").GetComponent<TeamManager>());

        /// <summary>
        /// The single Instance of this class
        /// </summary>
        public static TeamManager Instance => lazy.Value;

        private TeamManager() { }
        #endregion

        public int MaxFloors = 3;
        public IntEvent FloorsChangedEvent;
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
        public int calcGameProgress()
        {
            return 0;
        }

        public TeamManagerData GetData()
        {
            return data;
        }
    }
}
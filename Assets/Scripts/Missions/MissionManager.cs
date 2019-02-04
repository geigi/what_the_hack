using System;
using Base;
using Employees;
using GameSystem;
using GameTime;
using Interfaces;
using SaveGame;
using Team;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Tools;
using Object = UnityEngine.Object;

namespace Missions
{
    /// <summary>
    /// This class is responsible for the management of missions.
    /// This includes shuffling the currently available missions.
    /// </summary>
    public class MissionManager : Singleton<MissionManager>, ISaveable<MissionManagerData>
    {
        public int MaxActiveMissions = 4;
        public int MaxOpenMission = 6;
        public float RefreshRate = 0.3f;
        
        /// <summary>
        /// Event that will be fired when a day changes.
        /// </summary>
        public NetObjectEvent GameTimeDayTickEvent;

        public IntEvent GameTimeTickEvent;

        public GameEvent AvailableMissionsChanged;
        public GameEvent CompletedMissionsChanged;
        public GameEvent InProgressMissionsChanged;
        
        private MissionManagerData data;
        private ContentHub contentHub;
        private UnityAction<object> dayChangedAction;
        private UnityAction<int> onTickAction;
        private MissionList missionList;
        
        private void Awake()
        {
            if  (GameSettings.NewGame)
                InitDefaultState();
            else
                LoadState();

            dayChangedAction += DayChanged;
            GameTimeDayTickEvent.AddListener(dayChangedAction);

            onTickAction += onTick;
            GameTimeTickEvent.AddListener(onTickAction);
            
            missionList = ModHolder.Instance.GetMissionList();
            if (missionList == null)
                missionList = ContentHub.Instance.DefaultMissionList;
        }
        
        /// <summary>
        /// Initializes the MissionManager. This Method should be called before using the Manager.
        /// </summary>
        private void InitDefaultState()
        {
            contentHub = ContentHub.Instance;

            data = new MissionManagerData();
            
            fillOpenMissions();
            AvailableMissionsChanged.Raise();
        }
        
        /// <summary>
        /// Load state from a given savegame.
        /// </summary>
        private void LoadState()
        {
            var mainSaveGame = SaveGameSystem.Instance.GetCurrentSaveGame();
            data = mainSaveGame.missionManagerData;
            AvailableMissionsChanged.Raise();
            CompletedMissionsChanged.Raise();
            InProgressMissionsChanged.Raise();
        }
        
        void Start()
        {
            
        }
        
        /// <summary>
        /// Adds new missions to the available list.
        /// </summary>
        private void DayChanged(object date)
        {
            refreshOpenMissions();
        }

        private void onTick(int tick)
        {
            foreach (var mission in data.InProgress)
            {
                mission.RemainingTicks -= 1;
                if (mission.RemainingTicks < 1)
                {
                    // Close mission
                }
            }
        }

        /// <summary>
        /// Get the data object that needs to be serialized.
        /// </summary>
        /// <returns></returns>
        public MissionManagerData GetData()
        {
            return data;
        }

        public void AcceptMission(Mission mission)
        {
            data.Available.Remove(mission);
            AvailableMissionsChanged.Raise();
            data.InProgress.Add(mission);
            mission.RemainingTicks = mission.Duration * GameTime.GameTime.Instance.ClockSteps;
            InProgressMissionsChanged.Raise();
            //TODO: Start mission and countdown time
        }

        /// <summary>
        /// Refresh the missions currently available.
        /// </summary>
        private void refreshOpenMissions()
        {
            removeMissions(RefreshRate);
            fillOpenMissions();
            AvailableMissionsChanged.Raise();
        }

        /// <summary>
        /// Remove missions from the open list.
        /// </summary>
        /// <param name="fraction">Fraction to be removed.</param>
        private void removeMissions(float fraction)
        {
            int removeAmount = MathUtils.Clamp((int) (fraction * MaxOpenMission), 0, data.Available.Count);

            for (int i = 0; i < removeAmount; i++) {
                data.Available.RemoveAt(0);
            }
        }

        /// <summary>
        /// Fill the open missions list until the maximum is reached.
        /// </summary>
        private void fillOpenMissions()
        {
            int gameProgress = TeamManager.Instance.calcGameProgress();

            for (int i = data.Available.Count; i < MaxOpenMission; i++) {
                data.Available.Add(MissionFactory.Instance.CreateMission(gameProgress));
            }
        }
    }
}

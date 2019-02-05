using System;
using System.Collections.Generic;
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
using Wth.ModApi.Employees;
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
        private UnityAction<object> dayChangedAction;
        private UnityAction<int> onTickAction;
        private MissionList missionList;

        private Dictionary<Mission, MissionWorker> missionWorkers;

        private void Awake()
        {
            missionWorkers = new Dictionary<Mission, MissionWorker>();
            
            if (GameSettings.NewGame)
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

            foreach (var m in data.InProgress)
            {
                m.Finished.AddListener(missionFinished);
                createMissionWorker(m);
            }
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
            for (int i = 0; i < data.InProgress.Count; i++)
            {
                data.InProgress[i].RemainingTicks -= 1;
                if (data.InProgress[i].RemainingTicks < 1)
                {
                    data.InProgress[i].Finish();
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
            missionWorkers.Add(mission, new MissionWorker(mission));
            mission.Finished.AddListener(missionFinished);
        }

        /// <summary>
        /// Abort a mission.
        /// TODO: Removes it from in progress and re-adds it to available if defined.
        /// </summary>
        /// <param name="mission"></param>
        public void AbortMission(Mission mission)
        {
            foreach (var workplace in TeamManager.Instance.GetWorkplacesWorkingOnMission(mission))
            {
                workplace.StopWorking();
            }
            
            mission.Finished.Invoke(mission);
            missionFinished(mission);
        }

        /// <summary>
        /// Adds an employee to the mission worker of a mission.
        /// </summary>
        /// <param name="mission"></param>
        /// <param name="employee"></param>
        public void StartWorking(Mission mission, EmployeeData employee)
        {
            missionWorkers[mission].AddEmployee(employee);
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

            for (int i = 0; i < removeAmount; i++)
            {
                data.Available.RemoveAt(0);
            }
        }

        /// <summary>
        /// Fill the open missions list until the maximum is reached.
        /// </summary>
        private void fillOpenMissions()
        {
            int gameProgress = TeamManager.Instance.calcGameProgress();

            for (int i = data.Available.Count; i < MaxOpenMission; i++)
            {
                data.Available.Add(MissionFactory.Instance.CreateMission(gameProgress));
            }
        }

        /// <summary>
        /// Get the mission worker which belongs to this mission.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public MissionWorker GetMissionWorker(Mission m)
        {
            return missionWorkers[m];
        }

        private void createMissionWorker(Mission mission)
        {
            var worker = new MissionWorker(mission);
            missionWorkers.Add(mission, worker);
        }

        /// <summary>
        /// This method is called when a mission finishes.
        /// It determines whether the mission is completed successfully or has failed.
        /// It also handles the payout aswell as a possible reappearing of the mission.
        /// </summary>
        /// <param name="mission"></param>
        private void missionFinished(Mission mission)
        {
            data.InProgress.Remove(mission);
            InProgressMissionsChanged.Raise();
            missionWorkers[mission].Cleanup();
            missionWorkers.Remove(mission);

            if (mission.Completed())
            {
                // Mission has completed successfully
                data.Completed.Add(mission);
                CompletedMissionsChanged.Raise();

                // Payout
                ContentHub.Instance.bank.Income(mission.RewardMoney);
            }
            else
            {
                // Mission has failed

                // TODO: Re-add to available if requested by definition
            }
        }
    }
}
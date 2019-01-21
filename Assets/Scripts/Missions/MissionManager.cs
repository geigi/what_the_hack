using System;
using Employees;
using GameSystem;
using Interfaces;
using SaveGame;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Tools;
using Object = UnityEngine.Object;

namespace Missions
{
    public class MissionManager : MonoBehaviour, Saveable<MissionManagerData>
    {
        #region Singleton
        private static readonly Lazy<MissionManager> lazy = 
            new Lazy<MissionManager>(() => GameObject.FindWithTag("MissionManager").GetComponent<MissionManager>());

        /// <summary>
        /// The single Instance of this class
        /// </summary>
        public static MissionManager Instance => lazy.Value;

        private MissionManager() { }
        #endregion
        
        public int MaxActiveMissions = 4;
        public int MaxOpenMission = 6;
        public float RefreshRate = 0.3f;
        
        /// <summary>
        /// Event that will be fired when a day changes.
        /// </summary>
        public ObjectEvent GameTimeDayTickEvent;

        public GameEvent AvailableMissionsChanged;
        public GameEvent CompletedMissionsChanged;
        public GameEvent InProgressMissionsChanged;
        
        private MissionManagerData data;
        private ContentHub contentHub;
        private UnityAction<Object> dayChangedAction;
        private MissionList missionList;
        
        private void Awake()
        {
            if  (GameSettings.NewGame)
                InitDefaultState();
            else
                LoadState();

            dayChangedAction += DayChanged;
            GameTimeDayTickEvent.AddListener(dayChangedAction);
            
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
            var mainSaveGame = gameObject.GetComponent<SaveGameSystem>().GetCurrentSaveGame();
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
        public void DayChanged(Object date)
        {
            refreshOpenMissions();
        }

        public MissionManagerData GetData()
        {
            return data;
        }

        private void refreshOpenMissions()
        {
            removeMissions(RefreshRate);
            fillOpenMissions();
            AvailableMissionsChanged.Raise();
        }

        private void removeMissions(float fraction)
        {
            int removeAmount = MathUtils.Clamp((int) (fraction * MaxOpenMission), 0, data.Available.Count);

            for (int i = 0; i < removeAmount; i++) {
                data.Available.RemoveAt(0);
            }
        }

        private void fillOpenMissions()
        {
            int gameProgress = TeamManager.Instance.calcGameProgress();

            for (int i = data.Available.Count; i < MaxOpenMission; i++) {
                data.Available.Add(MissionFactory.Instance.CreateMission(gameProgress));
            }
        }
    }
}

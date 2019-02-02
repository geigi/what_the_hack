using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameTime;
using UE.Common;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Missions
{
    /// <summary>
    /// This selector is only used to select between the three different mission lists of the mission manager.
    /// </summary>
    public enum MissionHandlerSelector
    {
        Available,
        InProgress,
        Completed
    }
    
    /// <summary>
    /// This component attaches to a mission event by the <see cref="MissionManager"/>.
    /// It updates the UI with new missions and removes old ones.
    /// </summary>
    public class MissionUIHandler: MonoBehaviour
    {
        public GameEvent Event;
        public MissionHandlerSelector Selector;
        public GameObject MissionPrefab;

        private UnityAction listener;
        
        private void Awake()
        {
            listener = handle;
            Event.AddListener(listener);
        }

        private void Start()
        {
            var rect = GetComponent<RectTransform>();
            rect.ResetPosition();
        }

        /// <summary>
        /// Handle the event.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void handle()
        {
            List<Mission> missions;

            switch (Selector)
            {
                case MissionHandlerSelector.Available:
                    missions = MissionManager.Instance.GetData().Available;
                    break;
                case MissionHandlerSelector.InProgress:
                    missions = MissionManager.Instance.GetData().InProgress;
                    break;
                case MissionHandlerSelector.Completed:
                    missions = MissionManager.Instance.GetData().Completed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            removeOld(missions);
            addNew(missions);
        }

        /// <summary>
        /// Remove old missions from the UI.
        /// </summary>
        /// <param name="missions">Missions that are active.</param>
        private void removeOld(List<Mission> missions)
        {
            foreach (Transform child in transform)
            {
                var uiElement = child.GetComponent<MissionUIElement>();
                if (!missions.Contains(uiElement.GetMission()))
                {
                    child.parent = null;
                    Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Add new missions to the UI.
        /// </summary>
        /// <param name="missions">Missions that are active.</param>
        private void addNew(List<Mission> missions)
        {
            foreach (var mission in missions)
            {
                IEnumerable<Transform> childs = transform.Cast<Transform>();
                if (childs.All(p => p.GetComponent<MissionUIElement>().GetMission() != mission))
                {
                    var element = Instantiate(MissionPrefab, gameObject.transform, false);
                    element.GetComponent<MissionUIElement>().SetMission(mission);
                }
            }
        }
    }
}
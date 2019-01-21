using System;
using System.Collections.Generic;
using System.Linq;
using GameTime;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Missions
{
    public enum MissionHandlerSelector
    {
        Available,
        InProgress,
        Completed
    }
    
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

        private void removeOld(List<Mission> missions)
        {
            foreach (Transform child in transform)
            {
                var uiElement = child.GetComponent<MissionUIElement>();
                if (!missions.Contains(uiElement.GetMission()))
                {
                    child.parent = null;
                    Destroy(child);
                }
            }
        }

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
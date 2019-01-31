using System;
using System.Collections.Generic;
using System.Linq;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Missions
{
    public class MissionUiSelector : MonoBehaviour
    {
        public GameEvent Event;
        public GameObject MissionPrefab;

        private UnityAction listener;

        private void Awake()
        {
            listener = handle;
            Event.AddListener(listener);
        }

        private void Start()
        {
        }

        /// <summary>
        /// Handle the event.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void handle()
        {
            List<Mission> missions;
            missions = MissionManager.Instance.GetData().InProgress;
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
using UnityEngine;
using UnityEngine.UI;

namespace Missions
{
    public class MissionUIElement: MonoBehaviour
    {
        public Text Name;
        public Text Description;
        public Text MissionRequirement;
        public GameObject SkillRequirementContent;
        public Text Duration;
        public Text Reward;

        public GameObject SkillRequirementPrefab;
        
        private Mission mission;
        
        public void SetMission(Mission mission)
        {
            this.mission = mission;
        }

        public Mission GetMission()
        {
            return mission;
        }
    }
}
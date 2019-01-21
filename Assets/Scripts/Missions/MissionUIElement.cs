using UnityEngine;
using UnityEngine.UI;

namespace Missions
{
    public class MissionUIElement: MonoBehaviour
    {
        public Text Name;
        public Text Description;
        public GameObject MissionRequirementContainer;
        public GameObject SkillRequirementContent;
        public Text Duration;
        public Text Reward;

        public GameObject SkillRequirementPrefab;
        public GameObject MissionRequirementPrefab;
        
        private Mission mission;
        
        public void SetMission(Mission mission)
        {
            this.mission = mission;

            // General Info
            Name.text = mission.GetName();
            Description.text = mission.GetDescription();
            Duration.text = mission.Duration.ToString();
            Reward.text = mission.RewardMoney.ToString();
            
            // Skills
            foreach (var skillDefinition in mission.SkillDifficulty)
            {
                var skill = Instantiate(SkillRequirementPrefab, SkillRequirementContent.transform, false);
                skill.transform.GetChild(0).GetComponent<Image>().sprite = skillDefinition.Key.skillSprite;
                skill.transform.GetChild(1).GetComponent<Text>().text = skillDefinition.Value.ToString();
            }
            
            // Mission Requirements
            foreach (var missionRequirement in mission.Definition.RequiredMissions.RequiredMissions)
            {
                var missionR = Instantiate(MissionRequirementPrefab, MissionRequirementContainer.transform, false);
                missionR.transform.GetChild(1).GetComponent<Text>().text = missionRequirement.Title;
            }
        }

        public Mission GetMission()
        {
            return mission;
        }
    }
}
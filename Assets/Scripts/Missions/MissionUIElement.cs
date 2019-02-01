using UnityEngine;
using UnityEngine.UI;
using World;

namespace Missions
{
    /// <summary>
    /// This component should be attached to the MissionUIElement prefab.
    /// It contains methods to set the mission data to the ui components.
    /// </summary>
    public class MissionUIElement : MonoBehaviour
    {
        public Text Name;
        public Text Description;
        public GameObject MissionRequirementContainer;
        public GameObject SkillRequirementContent;
        public Text Duration;
        public Text RemainingDays;
        public Text Reward;
        public Button AcceptMissionButton;
        public Button SelectMissionButton;

        public GameObject SkillRequirementPrefab;
        public GameObject MissionRequirementPrefab;

        public TouchClickController TouchClickController;
        
        private Mission mission;

        /// <summary>
        /// Set the data of the given mission to the UI elements.
        /// </summary>
        /// <param name="mission"></param>
        public void SetMission(Mission mission)
        {
            this.mission = mission;

            // General Info
            if (Name != null)
                Name.text = mission.GetName();
            if (Description != null)
                Description.text = mission.GetDescription();
            if (Duration != null)
                Duration.text = mission.Duration.ToString();
            if (RemainingDays != null)
                RemainingDays.text = mission.RemainingDays.ToString();
            if (Reward != null)
                Reward.text = mission.RewardMoney.ToString();

            if (SkillRequirementContent != null && SkillRequirementPrefab != null)
            {
                // Skills
                foreach (var skillDefinition in mission.SkillDifficulty)
                {
                    var skill = Instantiate(SkillRequirementPrefab, SkillRequirementContent.transform, false);
                    skill.transform.GetChild(0).GetComponent<Image>().sprite = skillDefinition.Key.skillSprite;
                    skill.transform.GetChild(1).GetComponent<Text>().text = skillDefinition.Value.ToString();
                }
            }

            if (MissionRequirementContainer != null && SkillRequirementPrefab != null)
            {
                // Mission Requirements
                foreach (var missionRequirement in mission.Definition.RequiredMissions.RequiredMissions)
                {
                    var missionR = Instantiate(MissionRequirementPrefab, MissionRequirementContainer.transform, false);
                    missionR.transform.GetChild(1).GetComponent<Text>().text = missionRequirement.Title;
                }
            }

            if (AcceptMissionButton != null)
                AcceptMissionButton.onClick.AddListener(OnAcceptMission);
            
            if (SelectMissionButton != null)
                SelectMissionButton.onClick.AddListener(OnSelectMission);
        }

        /// <summary>
        /// Return the mission displayed in this UI element.
        /// </summary>
        /// <returns></returns>
        public Mission GetMission()
        {
            return mission;
        }

        private void OnAcceptMission()
        {
            MissionManager.Instance.AcceptMission(mission);
        }

        private void OnSelectMission()
        {
            TouchClickController.MissionSelected(mission);
        }
    }
}
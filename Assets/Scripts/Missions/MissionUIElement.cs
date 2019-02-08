using Team;
using UE.Events;
using UI;
using UnityEngine;
using UnityEngine.Events;
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
        [Header("Display style")] public MissionUiModeSelector DisplayMode;
        [Header("General UI Elements")]
        public Text Name;
        public Text Description;
        public GameObject SkillRequirementPrefab;
        public GameObject MissionRequirementPrefab;
        public GameObject MissionRequirementContainer;
        public GameObject SkillRequirementContent;
        public Text Duration;
        public Text RemainingDays;
        public Text Reward;

        [Header("Available Elements")] 
        public Button AcceptMissionButton;
        
        [Header("In Progress Elements")]
        public Button AbortMissionButton;
        public ProgressBar RemainingTimeBar;
        public SkillProgressUi SkillProgressUi;
        public IntEvent TimeStepEvent;
        
        [Header("Selector Elements")]
        public Button SelectMissionButton;
        
        private Mission mission;
        private UnityAction<int> timeStepAction;
        
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

            SetSkillRequirements();

            if (MissionRequirementContainer != null && SkillRequirementPrefab != null)
            {
                // Mission Requirements
                foreach (var missionRequirement in mission.Definition.RequiredMissions.RequiredMissions)
                {
                    var missionR = Instantiate(MissionRequirementPrefab, MissionRequirementContainer.transform, false);
                    missionR.transform.GetChild(1).GetComponent<Text>().text = missionRequirement.Title;
                }
            }

            if (DisplayMode == MissionUiModeSelector.InProgress)
            {
                timeStepAction = OnTimeStep;
                TimeStepEvent.AddListener(timeStepAction);
                OnTimeStep(0);
                SkillProgressUi.SetMission(mission);
            }

            if (AcceptMissionButton != null)
                AcceptMissionButton.onClick.AddListener(OnAcceptMission);
            
            if (SelectMissionButton != null)
                SelectMissionButton.onClick.AddListener(OnSelectMission);
            
            if (AbortMissionButton != null)
                AbortMissionButton.onClick.AddListener(OnAbortMission);
        }

        private void SetSkillRequirements()
        {
            if (SkillRequirementContent != null && SkillRequirementPrefab != null)
            {
                foreach (Transform t in SkillRequirementContent.transform)
                {
                    Destroy(t.gameObject);
                }
                
                // Skills
                foreach (var skillDefinition in mission.SkillDifficulty)
                {
                    var skill = Instantiate(SkillRequirementPrefab, SkillRequirementContent.transform, false);
                    skill.transform.GetChild(0).GetComponent<Image>().sprite = skillDefinition.Key.skillSprite;
                    skill.transform.GetChild(1).GetComponent<Text>().text = skillDefinition.Value.ToString();
                }
            }
        }

        /// <summary>
        /// Return the mission displayed in this UI element.
        /// </summary>
        /// <returns></returns>
        public Mission GetMission()
        {
            return mission;
        }

        /// <summary>
        /// Update values that are subject to changes.
        /// This is used for story missions.
        /// </summary>
        public void UpdateValues()
        {
            SetSkillRequirements();
            
            if (Duration != null)
                Duration.text = mission.Duration.ToString();
            if (Reward != null)
                Reward.text = mission.RewardMoney.ToString();
        }

        private void OnAcceptMission()
        {
            MissionManager.Instance.AcceptMission(mission);
        }

        private void OnSelectMission()
        {
            GameSelectionManager.Instance.MissionSelected(mission);
        }

        private void OnAbortMission()
        {
            MissionManager.Instance.AbortMission(mission);
        }
        
        private void OnTimeStep(int step)
        {
            RemainingDays.text = mission.RemainingDays.ToString();
            if (mission.RemainingTicks > 0)
                RemainingTimeBar.SetProgress(
                    1f - mission.RemainingTicks / (float) mission.TotalTicks);
            else
                RemainingTimeBar.SetProgress(1f);
        }

        private void OnDestroy()
        {
            if (TimeStepEvent != null)
                TimeStepEvent.RemoveListener(timeStepAction);

            if (SkillProgressUi != null)
                SkillProgressUi.Clear();
        }
    }
}
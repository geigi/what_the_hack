using System.Collections.Generic;
using System.Linq;
using Missions;
using Team;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class SkillProgressUi : MonoBehaviour
    {
        /// <summary>
        /// Prefab that will be used to display single skill information
        /// </summary>
        [Header("General")]
        [Tooltip("Prefab that will be used to display single skill information")]
        public GameObject SkillPrefab;
        /// <summary>
        /// GameObject which will contain skill information
        /// </summary>
        [Tooltip("GameObject which will contain skill information")]
        public GameObject SkillContainer;
        
        [Header("Customization")]
        public Sprite Checkmark;
        public Color CheckmarkColor;
        public Sprite AllPurpose;

        private Mission mission;
        private List<KeyValuePair<SkillDefinition, ProgressBar>> progressBars;
        private UnityAction<KeyValuePair<SkillDefinition, float>> updateProgressAction;

        private void Awake()
        {
            // We need the null check here because the object might be instantiated after SetMission was called.
            // The Awake method will be executed only if the parent is also active.
            if (progressBars == null)
                progressBars = new List<KeyValuePair<SkillDefinition, ProgressBar>>();
            if (updateProgressAction == null)
                updateProgressAction = UpdateProgress;
        }

        public void SetMission(Mission mission)
        {
            this.mission = mission;
            var workplaces = TeamManager.Instance.GetWorkplacesWorkingOnMission(mission);

            foreach (var skill in mission.Progress)
            {
                // We have to do some initialization in this method, because the Awake method might not be called yet.
                // This happens when the object is instantiated but not shown. The Awake method will then call only
                // if the parent is also active.
                if (progressBars == null)
                    progressBars = new List<KeyValuePair<SkillDefinition, ProgressBar>>();
                if (updateProgressAction == null)
                    updateProgressAction = UpdateProgress;
                    
                var go = Instantiate(SkillPrefab, SkillContainer.transform, false);
                go.GetComponentsInChildren<Image>().First(i => i.name == "SkillImage").sprite =
                    skill.Key.skillSprite;
                var satisfiedImage = go.GetComponentsInChildren<Image>().First(i => i.name == "SatisfiedImage");

                bool skillSatisfied = false;
                foreach (var otherWorkplace in workplaces)
                {
                    if (otherWorkplace.IsOccupied())
                    {
                        foreach (var s in otherWorkplace.GetOccupyingEmployee().EmployeeData.Skills)
                        {
                            if (s.skillData == skill.Key) skillSatisfied = true;
                        }
                    }
                }

                satisfiedImage.sprite = skillSatisfied ? Checkmark : AllPurpose;
                satisfiedImage.color = skillSatisfied ? CheckmarkColor : Color.white;
                    
                var progress = go.GetComponentsInChildren<ProgressBar>().First();
                progressBars.Add(new KeyValuePair<SkillDefinition, ProgressBar>(skill.Key, progress));
                progress.SetProgress(skill.Value);
            }
                
            mission.ProgressChanged.AddListener(updateProgressAction);
        }

        public void Clear()
        {
            mission?.ProgressChanged?.RemoveListener(updateProgressAction);
            
            foreach (Transform go in SkillContainer.transform)
            {
                Destroy(go.gameObject);
            }
            
            progressBars.Clear();
        }

        public void EnableUpdate(bool enable)
        {
            
        }
        
        private void UpdateProgress(KeyValuePair<SkillDefinition, float> skill)
        {
            foreach (var pair in progressBars)
            {
                if (pair.Key == skill.Key)
                {
                    pair.Value.SetProgress(skill.Value);
                    break;
                }
            }
        }
    }
}
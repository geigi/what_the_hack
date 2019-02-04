using System.Collections.Generic;
using System.Linq;
using Extensions;
using Missions;
using UE.Events;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Team
{
    /// <summary>
    /// This component displays basic information about a workplace.
    /// </summary>
    public class WorkplaceInfoUi : MonoBehaviour
    {
        /// <summary>
        /// Text field for workplace name
        /// </summary>
        public Text Name;

        /// <summary>
        /// GameObject which will contain skill information
        /// </summary>
        public GameObject SkillContainer;

        /// <summary>
        /// Prefab that will be used to display single skill information
        /// </summary>
        public GameObject SkillPrefab;

        public ProgressBar TimeProgressbar;
        public Text RemainingDays;

        public Sprite Checkmark;
        public Color CheckmarkColor;
        public Sprite AllPurpose;

        /// <summary>
        /// The FloatWindow that contains the information.
        /// </summary>
        public FloatWindow FloatWindow;

        public IntEvent GameTickEvent;

        private Workplace workplace;
        private List<KeyValuePair<SkillDefinition, ProgressBar>> progressBars;
        private UnityAction<KeyValuePair<SkillDefinition, float>> updateProgressAction;
        private UnityAction<int> onGameTickAction;

        private void Awake()
        {
            progressBars = new List<KeyValuePair<SkillDefinition, ProgressBar>>();
            updateProgressAction = UpdateProgress;
            onGameTickAction = onGameTick;
            GameTickEvent.AddListener(onGameTickAction);
        }

        /// <summary>
        /// Select an employee, fill in the information and display the window.
        /// This attaches to the skill changed events.
        /// </summary>
        /// <param name="employee">Employee whos basic information should be displayed</param>
        public void Select(Workplace workplace)
        {
            if (workplace.Mission != null)
            {
                this.workplace = workplace;
                progressBars.Clear();
                var otherWorkplaces = TeamManager.Instance.GetWorkplacesWorkingOnMission(workplace.Mission);

                Name.text = workplace.Mission.GetName();
                UpdateRemainingTime();

                foreach (var skill in workplace.Mission.Progress)
                {
                    var go = Instantiate(SkillPrefab, SkillContainer.transform, false);
                    go.GetComponentsInChildren<Image>().First(i => i.name == "SkillImage").sprite =
                        skill.Key.skillSprite;
                    var satisfiedImage = go.GetComponentsInChildren<Image>().First(i => i.name == "SatisfiedImage");

                    bool skillSatisfied = false;
                    foreach (var otherWorkplace in otherWorkplaces)
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
                
                workplace.Mission.ProgressChanged.AddListener(updateProgressAction);
                
                FloatWindow.Select(workplace.gameObject);
            }
        }

        private void UpdateRemainingTime()
        {
            RemainingDays.text = workplace.Mission.RemainingDays.ToString();
            if (workplace.Mission.RemainingTicks > 0)
                TimeProgressbar.SetProgress(
                    1f - workplace.Mission.RemainingTicks / (float) workplace.Mission.TotalTicks);
            else
                TimeProgressbar.SetProgress(1f);
        }

        /// <summary>
        /// Remove information and hide float window.
        /// </summary>
        public void Deselect()
        {
            FloatWindow.Deselect();

            if (!workplace.IsTrueNull())
                workplace.Mission?.ProgressChanged.RemoveListener(updateProgressAction);

            workplace = null;
            
            foreach (Transform go in SkillContainer.transform)
            {
                Destroy(go.gameObject);
            }
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

        private void onGameTick(int tick)
        {
            if (!workplace.IsTrueNull()) UpdateRemainingTime();
        }
    }
}
using System.Linq;
using Extensions;
using Missions;
using UE.Events;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using World;

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
        [Header("Content")]
        public Text Name;
        public SkillProgressUi SkillProgressUi;
        public ProgressBar TimeProgressbar;
        public Text RemainingDays;

        [Header("Buttons")] 
        public Button StopWorkingButton;

        /// <summary>
        /// The FloatWindow that contains the information.
        /// </summary>
        [Header("General")]
        public FloatWindow FloatWindow;

        [Header("Events")]
        public IntEvent GameTickEvent;
        public GameEvent CompletedMissionChanged, WorkplaceInfoOpened;

        private Workplace workplace;
        private UnityAction<int> onGameTickAction;
        private UnityAction onCompletedMissionAction;
        
        private GameSelectionManager gameSelectionManager;

        private void Awake()
        {
            gameSelectionManager = GameSelectionManager.Instance;
            onGameTickAction = onGameTick;
            GameTickEvent.AddListener(onGameTickAction);
            StopWorkingButton.onClick.AddListener(onStopWorking);
            onCompletedMissionAction = onCompletedMission;
            CompletedMissionChanged.AddListener(onCompletedMissionAction);
        }

        /// <summary>
        /// Select an employee, fill in the information and display the window.
        /// This attaches to the skill changed events.
        /// </summary>
        /// <param name="workplace">Workplace which basic information should be displayed</param>
        public void Select(Workplace workplace)
        {
            if (workplace.Mission != null)
            {
                this.workplace = workplace;
                
                Name.text = workplace.Mission.GetName();
                SkillProgressUi.SetMission(workplace.Mission);
                UpdateRemainingTime();
                
                FloatWindow.Select(workplace.gameObject);
                
                WorkplaceInfoOpened.Raise();
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
            SkillProgressUi.Clear();

            workplace = null;
        }

        private void onStopWorking()
        {
            workplace.StopWorking(false, false);
            gameSelectionManager.ClearEmployee();
            gameSelectionManager.ClearWorkplace();
        }

        private void onCompletedMission()
        {
            if (workplace != null && MissionManager.Instance.GetData().Completed.Contains(workplace.Mission))
            {
                Deselect();
            }
        }

        private void onGameTick(int tick)
        {
            if (!workplace.IsTrueNull()) UpdateRemainingTime();
        }
    }
}
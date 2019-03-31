using System.Runtime.Serialization;
using UE.Common;
using UI;
using UI.EmployeeWindow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace Assets.Scripts.UI.EmployeeWindow
{
    /// <summary>
    /// UiBuilder for hired employees.
    /// </summary>
    public class HiredEmployeeUiBuilder : EmployeeUiBuilder
    {
        /// <summary>
        /// Employee for which the UI is build
        /// </summary>
        internal Employee emp;
        
        /// <summary>
        /// Text to display the current state of the employee.
        /// </summary>
        [Header("Hired UI Elements")]
        public TextBanner employeeState;
        public Text EmployeeLevel;
        public Button LevelUpButton;
        public Image LevelUpArrowImage;
        public Color SkillPointsAvailableColor;
        
        [Header("Events")]
        public UnityEvent stateEvent;
        
        private SkillEmployeeUi SkillEmployeeUi;
        private Color defaultButtonColor;
        private UnityAction<int> availablePointsChangedAction;
        private UnityAction<int> levelChangedAction;
        private Image levelUpButtonImage;
        private Text levelUpButtonText;
        private RectTransform levelUpButtonRect;
        
        private void Awake()
        {
            levelUpButtonImage = LevelUpButton.GetComponent<Image>();
            defaultButtonColor = levelUpButtonImage.color;
            levelUpButtonText = LevelUpButton.GetComponentInChildren<Text>();
            levelUpButtonRect = levelUpButtonText.gameObject.GetComponent<RectTransform>();
        }

        public void Update()
        {
            empImage.sprite = emp.GetComponent<SpriteRenderer>().sprite;
        }
    
        /// <summary>
        /// Sets the employee which is hired.
        /// </summary>
        /// <param name="_emp">Object of the employee</param>
        /// <param name="buttonAction">Action the button should perform, when pressed</param>
        public void SetEmp(Employee _emp, UnityEvent _stateEvent, UnityAction buttonAction, SkillEmployeeUi skillEmployeeUi)
        {
            this.emp = _emp;
            this.SkillEmployeeUi = skillEmployeeUi;
            base.SetEmp(emp.EmployeeData, buttonAction);
            this.stateEvent = _stateEvent;
            this.stateEvent.AddListener(UpdateEmployeeState);
            LevelUpButton.onClick.AddListener(() => SkillEmployeeUi.Show(emp));
            
            availablePointsChangedAction = onAvailablePointsChanged;
            emp.EmployeeData.SkillPointsChanged.AddListener(availablePointsChangedAction);
            onAvailablePointsChanged(emp.EmployeeData.SkillPoints);

            levelChangedAction = onLevelChanged;
            emp.EmployeeData.LevelChanged.AddListener(levelChangedAction);
            onLevelChanged(emp.EmployeeData.Level);
        }

        private void OnDestroy()
        {
            LevelUpButton.onClick.RemoveListener(() => SkillEmployeeUi.Show(emp));
            emp.EmployeeData.SkillPointsChanged.RemoveListener(availablePointsChangedAction);
            emp.EmployeeData.LevelChanged.RemoveListener(levelChangedAction);
        }

        private void onAvailablePointsChanged(int points)
        {
            if (points > 0)
            {
                // Skill level up points available
                levelUpButtonImage.color = SkillPointsAvailableColor;
                LevelUpArrowImage.enabled = true;
                var offsetMin = levelUpButtonRect.offsetMin;
                offsetMin = new Vector2(5, offsetMin.y);
                levelUpButtonRect.offsetMin = offsetMin;
                levelUpButtonText.alignment = TextAnchor.MiddleLeft;
            }
            else
            {
                // Skill level up points NOT available
                levelUpButtonImage.color = defaultButtonColor;
                LevelUpArrowImage.enabled = false;
                var offsetMin = levelUpButtonRect.offsetMin;
                offsetMin = new Vector2(0, offsetMin.y);
                levelUpButtonRect.offsetMin = offsetMin;
                levelUpButtonText.alignment = TextAnchor.MiddleCenter;
            }
        }

        private void onLevelChanged(int level)
        {
            EmployeeLevel.text = emp.EmployeeData.Level.ToString();
        }
        
        private void UpdateEmployeeState()
        {
            string text;
            
            if (emp.State == Enums.EmployeeState.WORKING)
                text = "Working";
            else
                text = "Idle";
            
            if (!employeeState.GetText().Equals(text))
                employeeState.Set(text);
        }
    }
}

using UI.EmployeeWindow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        public Text employeeState;

        public UnityEvent stateEvent;

        public Button LevelUpButton;
        private void UpdateEmployeeState() => employeeState.text = emp.State.ToString();
        
        private SkillEmployeeUi SkillEmployeeUi;

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
        }

        private void OnDestroy()
        {
            LevelUpButton.onClick.RemoveListener(() => SkillEmployeeUi.Show(emp));
        }
    }
}

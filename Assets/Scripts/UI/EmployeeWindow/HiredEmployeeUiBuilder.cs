using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.EmployeeWindow
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

        private void UpdateEmployeeState() => employeeState.text = emp.Walking ? "idle" : "Working";

        public void Update()
        {
            empImage.sprite = emp.GetComponent<SpriteRenderer>().sprite;
        }
    
        /// <summary>
        /// Sets the employee which is hired.
        /// </summary>
        /// <param name="_emp">Object of the employee</param>
        /// <param name="buttonAction">Action the button should perform, when pressed</param>
        public void SetEmp(Employee _emp, UnityEvent _stateEvent, UnityAction buttonAction)
        {
            this.emp = _emp;
            base.SetEmp(emp.EmployeeData, buttonAction);
            this.stateEvent = _stateEvent;
            this.stateEvent.AddListener(UpdateEmployeeState);
        }
    }
}

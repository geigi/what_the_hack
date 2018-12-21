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

        public override void FillSpecificGUIElements()
        {
            empImage.sprite = emp.GetComponent<SpriteRenderer>().sprite;
            employeeState.text = emp.walking ? "idle" : "Working";
        }
    
        /// <summary>
        /// Sets the employee which is hired.
        /// </summary>
        /// <param name="_emp">Object of the employee</param>
        /// <param name="buttonAction">Action the button should perform, when pressed</param>
        public void SetEmp(Employee _emp, UnityAction buttonAction)
        {
            this.emp = _emp;
            base.SetEmp(emp.EmployeeData, buttonAction);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wth.ModApi.Employees;

namespace Assets.Scripts.UI.EmployeeWindow
{
    /// <summary>
    /// UiBuilder for hireable employees. 
    /// </summary>
    public class HireableEmployeeUiBuilder : EmployeeUiBuilder
    {
        /// <summary>
        /// Contains every sprite, the employee could have. 
        /// </summary>
        public Sprite[] sprites;
    
        /// <summary>
        /// Text to display the prize of the employee.
        /// </summary>
        public Text prize;

        /// <summary>
        /// Set the employee which is hireable.
        /// </summary>
        /// <param name="_empData">Data of the hired employee</param>
        /// <param name="buttonAction">Action the button should perform, when pressed</param>
        public override void SetEmp(EmployeeData _empData, UnityAction buttonAction)
        {
            base.SetEmp(_empData, buttonAction);
            empImage.sprite = (employeeData.generatedData.gender == "male")
                ? sprites[employeeData.generatedData.idleClipIndex]
                : sprites[4 + employeeData.generatedData.idleClipIndex];
            //Prize does not change, can be set once.
            prize.text = $"{employeeData.Prize} $";
        }
    }
}

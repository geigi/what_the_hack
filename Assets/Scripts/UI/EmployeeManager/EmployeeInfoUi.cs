using System.Collections.Generic;
using System.Linq;
using Employees;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wth.ModApi.Employees;

namespace UI.EmployeeWindow
{
    /// <summary>
    /// This component displays basic information about an employee.
    /// </summary>
    public class EmployeeInfoUi : MonoBehaviour
    {
        /// <summary>
        /// Text field for employee name
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
        /// <summary>
        /// The FloatWindow that contains the information.
        /// </summary>
        public FloatWindow FloatWindow;
        /// <summary>
        /// The window must be hidden if the displayed employee is fired.
        /// </summary>
        public ObjectEvent EmployeeFiredEvent;
        
        private Employee employee;
        private Dictionary<Skill, UnityAction> skillEvents;
        private UnityAction<Object> employeeFiredAction;

        private void Awake()
        {
            skillEvents = new Dictionary<Skill, UnityAction>();
            employeeFiredAction = new UnityAction<Object>(onEmployeeFired);
            EmployeeFiredEvent.AddListener(employeeFiredAction);
        }

        /// <summary>
        /// Select an employee, fill in the information and display the window.
        /// This attaches to the skill changed events.
        /// </summary>
        /// <param name="employee">Employee whos basic information should be displayed</param>
        public void Select(Employee employee)
        {
            this.employee = employee;

            Name.text = employee.Name;

            foreach (var skill in employee.EmployeeData.Skills)
            {
                var go = Instantiate(SkillPrefab, SkillContainer.transform, false);
                go.GetComponentsInChildren<Text>().First().text = skill.Level.ToString();
                go.GetComponentsInChildren<Image>().First().sprite = skill.GetSprite();

                UnityAction skillChanged = () => go.GetComponent<Text>().text = skill.Level.ToString();
                skill.SkillEvent.AddListener(skillChanged);
                skillEvents.Add(skill, skillChanged);
            }
            
            FloatWindow.Select(employee.gameObject);
        }
        
        /// <summary>
        /// Remove information and hide float window.
        /// </summary>
        public void Deselect()
        {
            FloatWindow.Deselect();
            
            for (int i = 0; i < employee.EmployeeData.Skills.Count; i++)
            {
                var skill = employee.EmployeeData.Skills[i];
                skill.SkillEvent.RemoveListener(skillEvents[skill]);
            }
            
            foreach (Transform go in SkillContainer.transform)
            {
                Destroy(go.gameObject);
            }
            
            skillEvents.Clear();
        }

        private void onEmployeeFired(Object emp)
        {
            FloatWindow.ResetTempHidden();
        }
    }
}
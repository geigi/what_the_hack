using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        
        private Employee employee;
        private List<UnityAction> skillEvents;

        private void Awake()
        {
            skillEvents = new List<UnityAction>();
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
                go.GetComponentsInChildren<Text>().First().text = skill.level.ToString();
                go.GetComponentsInChildren<Image>().First().sprite = skill.GetSprite();

                UnityAction skillChanged = () => go.GetComponent<Text>().text = skill.level.ToString();
                skill.skillEvent.AddListener(skillChanged);
                skillEvents.Add(skillChanged);
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
                employee.EmployeeData.Skills[i].skillEvent.RemoveListener(skillEvents[i]);
            }
            
            foreach (Transform go in SkillContainer.transform)
            {
                Destroy(go.gameObject);
            }
            
            skillEvents.Clear();
        }
    }
}
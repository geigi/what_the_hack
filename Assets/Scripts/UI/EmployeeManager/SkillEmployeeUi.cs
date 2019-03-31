using System;
using System.Collections.Generic;
using System.Linq;
using UE.Common;
using UE.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wth.ModApi.Employees;

namespace UI.EmployeeWindow
{
    /// <summary>
    /// This component contains logic for the employee skiller window.
    /// </summary>
    public class SkillEmployeeUi: MonoBehaviour
    {
        [Header("UI Elements")] 
        public Text EmployeeName;
        public GameObject SkillContainer;
        public Text AvailablePoints;

        [Header("Prefab")] 
        public GameObject SkillPrefab;

        [Header("State")] 
        public State SkillEmployeeState;

        private Employee employee;
        private Dictionary<Button.ButtonClickedEvent, UnityAction> buttonEvents = new Dictionary<Button.ButtonClickedEvent, UnityAction>();
        private Dictionary<Skill, Text> skillCostDictionary = new Dictionary<Skill, Text>();
        
        private void Awake()
        {
        }

        /// <summary>
        /// Set the information of the given employee to the UI.
        /// Enter the skill employee state after doing this.
        /// </summary>
        /// <param name="employee"></param>
        public void Show(Employee employee)
        {
            RemoveSkillPointsChangedListener();
            this.employee = employee;
            EmployeeName.text = employee.Name;
            employee.EmployeeData.SkillPointsChanged.AddListener(onPointsChanged);
            
            foreach (var keyValuePair in buttonEvents)
            {
                keyValuePair.Key.RemoveListener(keyValuePair.Value);
            }
            buttonEvents.Clear();
            skillCostDictionary.Clear();
            
            SkillContainer.transform.DestroyChildren();
            
            foreach (var skill in employee.EmployeeData.Skills)
            {
                var go = Instantiate(SkillPrefab, SkillContainer.transform, false);
                go.GetComponentsInChildren<SkillUIBuilder>().First().SetSkill(skill);

                var pointText = go.GetComponentsInChildren<Text>().First(c => c.name == "PointsRequired");
                pointText.text = skill.LevelUpCost.ToString();

                var button = go.GetComponentsInChildren<Button>().First();
                var action = new UnityAction(() => IncreaseSkill(employee, skill));
                buttonEvents.Add(button.onClick, action);
                skillCostDictionary.Add(skill, pointText);
                button.onClick.AddListener(action);
            }

            AvailablePoints.text = employee.EmployeeData.SkillPoints.ToString();
            
            SkillEmployeeState.Enter();
        }

        private void IncreaseSkill(Employee employee, Skill skill)
        {
            employee.IncrementSkill(skill);
            skillCostDictionary[skill].text = skill.LevelUpCost.ToString();
        }

        private void onPointsChanged(int points)
        {
            AvailablePoints.text = points.ToString();
        }

        private void OnDestroy()
        {
            RemoveSkillPointsChangedListener();
        }

        private void RemoveSkillPointsChangedListener()
        {
            if (employee != null)
                employee.EmployeeData.SkillPointsChanged.RemoveListener(onPointsChanged);
        }
    }
}
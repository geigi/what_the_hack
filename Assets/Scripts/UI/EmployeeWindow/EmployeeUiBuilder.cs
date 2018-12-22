﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wth.ModApi.Employees;
using Object = UnityEngine.Object;

namespace UI.EmployeeWindow
{
    /// <summary>
    /// Parent Class for the EmployeeUI
    /// </summary>
    public abstract class EmployeeUiBuilder : MonoBehaviour
    {
        /// <summary>
        /// Prefab for the SkillUI.
        /// </summary>
        public GameObject skillPrefab;
        /// <summary>
        /// Displays the current sprite of the employee.
        /// </summary>
        public Image empImage;
        /// <summary>
        /// Displays the name of the employee.
        /// </summary>
        public Text empName;
        /// <summary>
        /// Displays all skills of the employee.
        /// </summary>
        public GameObject skillPanel;
        /// <summary>
        /// Displays the salary of the employee.
        /// </summary>
        public Text salary, salaryTime;
        /// <summary>
        /// Displays all specials of the employee.
        /// </summary>
        public Text specialList;
        /// <summary>
        /// Functionality can be specific to each employee.
        /// </summary>
        public Button button;
        /// <summary>
        /// Data of the employee the UI is build for.
        /// </summary>
        public EmployeeData employeeData;

        private List<GameObject> skillUI;
        private readonly List<string> specialNames = new List<string>();
        private EmployeeFactory factory;

        /// <summary>
        /// Needs to be overridden by the subClass.
        /// To build the UI Elements specific to the child.
        /// </summary>
        public abstract void FillSpecificGUIElements();

        /// <summary>
        /// Called at the very beginning
        /// </summary>
        public void Awake() => factory = GameObject.FindWithTag("EmpFactory").GetComponent<EmployeeFactory>();
    
        /// <summary>
        /// Called Once Per Frame
        /// </summary>
        protected virtual void Update()
        {
            if (employeeData != null)
            {
                empName.text = employeeData.generatedData.name;
                salary.text = $"{employeeData.Salary}$";
                salaryTime.text = "a week";
                specialList.text = string.Join(",", specialNames);
                FillSpecificGUIElements();
            }
        }
    
        /// <summary>
        /// Sets the Employee for the UI
        /// </summary>
        /// <param name="_empData">EmployeeData of the Emp</param>
        /// <param name="buttonAction">The Action to be performed when the Button is clicked</param>
        public virtual void SetEmp(EmployeeData _empData, UnityAction buttonAction)
        {
            this.employeeData = _empData;
            GenerateSkillGui();
            empImage.material = factory.GetComponent<EmployeeFactory>().GenerateMaterialForEmployee(employeeData.generatedData);
            employeeData.Specials?.ForEach(special => specialNames.Add(special.GetDisplayName()));
            button.onClick.AddListener(buttonAction);
        }

        /// <summary>
        /// Builds the UI to display all skills of this employee. 
        /// </summary>
        private void GenerateSkillGui()
        {
            skillUI = new List<GameObject>();
            var skills = employeeData.Skills;
            if (skills == null) return;
            foreach (var s in skills)
            {
                GameObject skill = Object.Instantiate(skillPrefab);
                skill.transform.SetParent(skillPanel.gameObject.transform);
                skill.transform.localScale = Vector3.one;
                skill.GetComponent<SkillUIBuilder>().skill = s;
                skillUI.Add(skill);
            }
        }
    }
}

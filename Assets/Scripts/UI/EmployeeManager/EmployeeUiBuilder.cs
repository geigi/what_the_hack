﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wth.ModApi.Employees;
using Object = UnityEngine.Object;

[assembly: InternalsVisibleTo("Tests_PlayMode")]
namespace Assets.Scripts.UI.EmployeeWindow
{
    /// <summary>
    /// Parent Class for the EmployeeUI
    /// </summary>
    public abstract class EmployeeUiBuilder : MonoBehaviour
    {
        /// <summary>
        /// Prefab for the SkillUI.
        /// </summary>
        [Header("Prefab")]
        public GameObject skillPrefab;
        public GameObject SpecialPrefab;
        /// <summary>
        /// Displays the current sprite of the employee.
        /// </summary>
        [Header("General UI Elements")]
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
        public GameObject specialContainer;
        /// <summary>
        /// Functionality can be specific to each employee.
        /// </summary>
        public Button button;
        /// <summary>
        /// Data of the employee the UI is build for.
        /// </summary>
        [Header("Data")]
        public EmployeeData employeeData;

        private List<GameObject> skillUI;
        private EmployeeFactory factory;

        private UnityAction<EmployeeSpecial> specialsChangedAction;
    
        /// <summary>
        /// Sets the Employee for the UI
        /// </summary>
        /// <param name="_empData">EmployeeData of the Emp</param>
        /// <param name="buttonAction">The Action to be performed when the Button is clicked</param>
        public virtual void SetEmp(EmployeeData _empData, UnityAction buttonAction)
        {
            this.employeeData = _empData;
            factory = new EmployeeFactory();
            GenerateSkillGui();
            if (employeeData.generatedData != null)
            {
                empImage.material = factory.GenerateMaterialForEmployee(employeeData.generatedData, true);
                empName.text = employeeData.generatedData.name;
            }
            else
            {
                empName.text = employeeData.EmployeeDefinition.EmployeeName;
            }

            foreach (var special in employeeData.GetSpecials())
            {
                addSpecial(special);
            }
            
            button.onClick.AddListener(buttonAction);
            //EmployeeName, specials and Salary are not going to change, so they can be set once.
            salaryTime.text = "a Week";
            salary.text = $"{employeeData.Salary} $";

            specialsChangedAction = onSpecialsChanged;
            _empData.SpecialsChanged.AddListener(specialsChangedAction);
        }

        private void addSpecial(EmployeeSpecial special)
        {
            var go = Instantiate(SpecialPrefab, specialContainer.transform, false);
            go.GetComponentInChildren<Text>().text = special.GetDisplayName();
        }
        
        private void onSpecialsChanged(EmployeeSpecial special)
        {
            addSpecial(special);
        }

        /// <summary>
        /// Builds the UI to display all skills of this employee. 
        /// </summary>
        internal virtual void GenerateSkillGui()
        {
            skillUI = new List<GameObject>();
            var skills = employeeData.Skills;
            if (skills == null) return;
            foreach (var s in skills)
            {
                GameObject skill = Object.Instantiate(skillPrefab);
                skill.transform.SetParent(skillPanel.gameObject.transform);
                skill.transform.localScale = Vector3.one;
                skill.GetComponent<SkillUIBuilder>().SetSkill(s);
                skillUI.Add(skill);
            }
        }

        private void OnDestroy()
        {
            if (specialsChangedAction != null)
                employeeData?.SpecialsChanged?.RemoveListener(specialsChangedAction);
        }
    }
}

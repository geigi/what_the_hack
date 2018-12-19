using System;
using SaveGame;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;

namespace Employees
{
    /// <summary>
    /// This class manages all Employees for the game and keeps track, of employees that can be hired,
    /// employees which are hired and ex-employees.
    /// </summary>
    public class EmployeeManager : Manager
    {
        private static readonly Lazy<EmployeeManager> lazy =
            new Lazy<EmployeeManager>(() => new EmployeeManager());

        /// <summary>
        /// The single instance of this class.
        /// </summary>
        public static EmployeeManager Instance
        {
            get { return lazy.Value; }
        }

        public static int dayThreshold = 10;

        private EmployeeManager()
        {
        }


        /// <summary>
        /// List to store all employees that can be hired.
        /// </summary>
        public List<EmployeeData> employeesForHire { get; private set; }

        /// <summary>
        /// List to store all hired Employees.
        /// </summary>
        public List<EmployeeData> hiredEmplyoees { get; private set; }

        /// <summary>
        /// List to store all ex-employees.
        /// </summary>
        public List<EmployeeData> exEmplyoees { get; private set; }

        /// <summary>
        /// EmployeeFactory to generate random EmployeeData.
        /// </summary>
        private EmployeeFactory factory;

        private bool usedSpecialEmployee;
        private EmployeeList specialEmployees;
        private int daysPassed;
        private SkillSet standardSkillSet;
        private NameLists standardNameLists;
        private Material empMaterial;
        private AnimationClip[] maleAnim;
        private AnimationClip[] femaleAnim;

        public void InitReferences()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initializes the EmployeeManager. This Method should be called before using the Manager.
        /// Generates 4 employees for hire.
        /// </summary>
        /// <param name="_specialEmployee">The special employee, which can appear.</param>
        /// <param name="saveGame">Optional Parameter, when the game is loaded from a save game.</param>
        public void init( Material mat, AnimationClip[] male, AnimationClip[] female,
            MainSaveGame saveGame = null)
        {
            this.empMaterial = mat;
            this.femaleAnim = female;
            this.maleAnim = male;
            factory = new EmployeeFactory();
            this.daysPassed = 0;
            InitDefaultState();
            if (saveGame != null)
                LoadState(saveGame);
        }

        /// <summary>
        /// Initializes the EmployeeManager to the default state. This Method must be called before using the Manager.
        /// </summary>
        public void InitDefaultState()
        {
            Debug.Log("In Init");
            var contentHub = GameObject.FindWithTag("GameManager").GetComponent<ContentHub>();
            factory = new EmployeeFactory();

            this.standardSkillSet = contentHub.GetSkillSet();
            this.standardNameLists = contentHub.GetNameLists();
            this.specialEmployees = contentHub.GetEmployeeLists();

            this.employeesForHire = new List<EmployeeData>();
            this.hiredEmplyoees = new List<EmployeeData>();
            this.exEmplyoees = new List<EmployeeData>();
        }

        /// <summary>
        /// Load state from a given savegame.
        /// </summary>
        /// <param name="mainSaveGame"></param>
        public void LoadState(MainSaveGame mainSaveGame)
        {
            var contentHub = GameObject.FindWithTag("GameManager").GetComponent<ContentHub>();
            factory = new EmployeeFactory();

            this.standardSkillSet = contentHub.GetSkillSet();
            this.standardNameLists = contentHub.GetNameLists();
            this.specialEmployees = contentHub.GetEmployeeLists();

            this.employeesForHire = mainSaveGame.employeesForHire;
            this.hiredEmplyoees = mainSaveGame.employeesHired;
            this.exEmplyoees = mainSaveGame.exEmployees;
        }

        /// <summary>
        /// Puts a new employee in the employeeForHire List.
        /// </summary>
        public EmployeeData GenerateEmployeeForHire()
        {
            EmployeeData newEmployee = new EmployeeData();
            if (this.daysPassed >= dayThreshold && !usedSpecialEmployee)
            {
                newEmployee.EmployeeDefinition = specialEmployees.employeeList[0];
                this.employeesForHire.Add(newEmployee);
            }
            else
            {
                newEmployee = factory.GenerateEmployee(standardSkillSet, standardNameLists, maleAnim.Length / 3);
                this.employeesForHire.Add(newEmployee);
            }

            return newEmployee;
        }

        public void Cleanup()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Hires the first employee in the employeesForHire List.
        /// </summary>
        /// <returns>EmployeeData of the hired Employee.</returns>
        public Employee HireEmployee()
        {
            var gameObject = new GameObject("Employee");
            var emp = gameObject.AddComponent<Employee>();
            EmployeeData empData = employeesForHire[0];
            hiredEmplyoees.Add(empData);
            employeesForHire.Remove(empData);
            emp.init(empData, empMaterial, maleAnim, femaleAnim);
            return emp;
        }

        /// <summary>
        /// Hires the specified employee.
        /// If the employee does not exist in the employeesForHire List this method will do nothing;
        /// </summary>
        /// <param name="empData">The employee to hire.</param>
        public Employee HireEmployee(EmployeeData empData)
        {
            if (!this.employeesForHire.Contains(empData)) return null;
            var gameObject = new GameObject("Employee");
            var emp = gameObject.AddComponent<Employee>();
            hiredEmplyoees.Add(empData);
            employeesForHire.Remove(empData);
            emp.init(empData, empMaterial, maleAnim, femaleAnim);
            return emp;
        }

        /// <summary>
        /// Fires an employee. The employee will be removed from hiredEmployees and placed in exEmployees.  
        /// </summary>
        /// <param name="emp"></param>
        public void FireEmployee(EmployeeData emp)
        {
            if (!hiredEmplyoees.Contains(emp)) return;
            exEmplyoees.Add(emp);
            hiredEmplyoees.Remove(emp);
        }

        /// <summary>
        /// Will be used later as a listener method.
        /// Removes the first Employee from the EmployeeForHire List and creates a new one.
        /// </summary>
        public void newDay()
        {
            this.employeesForHire.RemoveAt(0);
            GenerateEmployeeForHire();
        }
    }
}

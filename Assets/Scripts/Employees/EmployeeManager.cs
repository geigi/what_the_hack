using System;
using SaveGame;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;

namespace Employees
{
    /// <summary>
    /// This class manages all Employees for the game and keeps track, of employees that can be hired,
    /// employees which are hired and ex-employees.
    /// </summary>
    public class EmployeeManager: MonoBehaviour {
        
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
        /// EmployeeFactory to generate the employees. 
        /// </summary>
        public EmployeeFactory factoryObject;
        
        /// <summary>
        /// List of all special employees.
        /// </summary>
        private EmployeeList specialEmployees;

        private ContentHub contentHub;

        private void Awake()
        {   
            if(GameSettings.NewGame)
                InitDefaultState();
            else
                LoadState();
        }

        /// <summary>
        /// Initializes the EmployeeManager. This Method should be called before using the Manager.
        /// Generates 4 employees for hire.
        /// </summary>
        private void InitDefaultState()
        {
            contentHub = ContentHub.Instance;

            this.specialEmployees = contentHub.GetEmployeeLists();

            this.employeesForHire = new List<EmployeeData>();
            this.hiredEmplyoees = new List<EmployeeData>();
            this.exEmplyoees = new List<EmployeeData>();
        }

        /// <summary>
        /// Load state from a given savegame.
        /// </summary>
        /// <param name="mainSaveGame"></param>
        private void LoadState()
        {
            var mainSaveGame = gameObject.GetComponent<SaveGameSystem>().GetCurrentSaveGame();

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
            /*if (this.daysPassed >= dayThreshold && !usedSpecialEmployee)
            {
                newEmployee.EmployeeDefinition = specialEmployees.employeeList[0];
                this.employeesForHire.Add(newEmployee);
            }*/
            newEmployee = factoryObject.GetComponent<EmployeeFactory>().GenerateEmployee();
            this.employeesForHire.Add(newEmployee);

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
            emp.init(empData);
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
            emp.init(empData);
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

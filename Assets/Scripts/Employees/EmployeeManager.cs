using System;
using System.Collections.Generic;
using Interfaces;
using SaveGame;
using UnityEngine;
using Wth.ModApi.Employees;

namespace Employees
{
    /// <summary>
    /// This class manages all Employees for the game and keeps track, of employees that can be hired,
    /// employees which are hired and ex-employees.
    /// </summary>
    public class EmployeeManager: Manager{
        private static readonly Lazy<EmployeeManager> lazy =
            new Lazy<EmployeeManager>(() => new EmployeeManager());

        /// <summary>
        /// The single instance of this class.
        /// </summary>
        public static EmployeeManager Instance { get { return lazy.Value; } }

        private EmployeeManager() { }
    
    
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

        /// <summary>
        /// The special Employee.
        /// Can only be hired once and does not reappear.
        /// </summary>
        private EmployeeList specialEmployees;

        private SkillSet skillSet;
        private NameLists nameList;

        public void InitReferences()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initializes the EmployeeManager to the default state. This Method must be called before using the Manager.
        /// Generates 4 employees for hire.
        /// </summary>
        public void InitDefaultState()
        {
            var contentHub = GameObject.FindWithTag("GameManager").GetComponent<ContentHub>();
            factory = new EmployeeFactory();
        
            this.skillSet  = contentHub.GetSkillSet();
            this.nameList = contentHub.GetNameLists();
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
        
            this.skillSet  = contentHub.GetSkillSet();
            this.nameList = contentHub.GetNameLists();
            this.specialEmployees = contentHub.GetEmployeeLists();
        
            this.employeesForHire = mainSaveGame.employeesForHire;
            this.hiredEmplyoees = mainSaveGame.employeesHired;
            this.exEmplyoees = mainSaveGame.exEmployees;
        }

        public void Cleanup()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Puts a new employee in the employeeForHire List.
        /// </summary>
        public void GenerateEmployeeForHire()
        {
            EmployeeData newEmployee = new EmployeeData();
            this.employeesForHire.Add(factory.GenerateEmployee(skillSet, nameList));
        }

        /// <summary>
        /// Hires the first employee in the employeesForHire List.
        /// </summary>
        /// <returns>EmployeeData of the hired Employee.</returns>
        public EmployeeData HireEmployee()
        {
            EmployeeData emp = employeesForHire[0];
            hiredEmplyoees.Add(emp);
            employeesForHire.Remove(emp);
            return emp;
        }

        /// <summary>
        /// Hires the specified employee.
        /// If the employee does not exist in the employeesForHire List this method will do nothing;
        /// </summary>
        /// <param name="emp">The employee to hire.</param>
        public void HireEmployee(EmployeeData emp)
        {
            if (!this.employeesForHire.Contains(emp)) return;
            hiredEmplyoees.Add(emp);
            employeesForHire.Remove(emp);
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

﻿using System;
using SaveGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Employees;
using GameSystem;
using GameTime;
using Interfaces;
using UE.Events;
using UI.EmployeeWindow;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;
using Object = UnityEngine.Object;

namespace Employees
{
    /// <summary>
    /// This class manages all Employees for the game and keeps track, of employees that can be hired,
    /// employees which are hired and ex-employees.
    /// </summary>
    public class EmployeeManager: MonoBehaviour, Saveable<EmployeeManagerData> {
        /// <summary>
        /// EmployeeFactory to generate the employees. 
        /// </summary>
        public EmployeeFactory factoryObject;

        /// <summary>
        /// Prefab that displays an employee for hire.
        /// </summary>
        public GameObject EmployeeForHirePrefab;

        /// <summary>
        /// Prefab that displays an hired employee.
        /// </summary>
        public GameObject EmployeeHiredPrefab;

        /// <summary>
        /// GameObject that will contain a list of all employees for hire.
        /// </summary>
        public GameObject EmployeeForHireContent;

        /// <summary>
        /// GameObject that will contain a list of all hired employees.
        /// </summary>
        public GameObject EmployeeHiredContent;

        /// <summary>
        /// Event that will be fired when a day changes.
        /// </summary>
        public ObjectEvent GameTimeDayTickEvent;

        private EmployeeManagerData data;
        
        /// <summary>
        /// List of all special employees.
        /// </summary>
        private EmployeeList specialEmployees;

        /// <summary>
        /// This dictionary maps an employeeData object to its GUI GameObject.
        /// </summary>
        private Dictionary<EmployeeData, GameObject> EmployeeToGuiMap;

        private ContentHub contentHub;

        private UnityAction<Object> dayChangedAction;

        private void Awake()
        {
            if  (GameSettings.NewGame)
                InitDefaultState();
            else
                LoadState();

            dayChangedAction += DayChanged;
            GameTimeDayTickEvent.AddListener(dayChangedAction);
        }
        
        void Start()
        {
            gameObject.transform.parent = this.gameObject.transform;
            for (int i = 0; i < 4; i++)
            {
                EmployeeData empData = GenerateEmployeeForHire();
                AddEmployeeForHire(empData);
            }
        }

        /// <summary>
        /// Initializes the EmployeeManager. This Method should be called before using the Manager.
        /// Generates 4 employees for hire.
        /// </summary>
        private void InitDefaultState()
        {
            contentHub = ContentHub.Instance;

            this.specialEmployees = contentHub.GetEmployeeLists();

            data = new EmployeeManagerData();
            data.employeesForHire = new List<EmployeeData>();
            data.hiredEmployees = new List<EmployeeData>();
            data.exEmplyoees = new List<EmployeeData>();
            EmployeeToGuiMap = new Dictionary<EmployeeData, GameObject>();
        }

        /// <summary>
        /// Load state from a given savegame.
        /// </summary>
        private void LoadState()
        {
            var mainSaveGame = gameObject.GetComponent<SaveGameSystem>().GetCurrentSaveGame();
            this.specialEmployees = contentHub.GetEmployeeLists();
            data = mainSaveGame.employeeManagerData;

            foreach (var employeeData in data.employeesForHire)
            {
                AddEmployeeForHireToGui(employeeData);
            }
        }

        /// <summary>
        /// Puts a new employee in the employeeForHire List.
        /// </summary>
        public EmployeeData GenerateEmployeeForHire()
        {
            EmployeeData newEmployee = new EmployeeData();
            newEmployee = factoryObject.GetComponent<EmployeeFactory>().GenerateEmployee();

            return newEmployee;
        }

        public void AddEmployeeForHire(EmployeeData employeeData)
        {
            data.employeesForHire.Add(employeeData);
            AddEmployeeForHireToGui(employeeData);
        }

        public void AddHiredEmployee(EmployeeData employeeData)
        {
            data.hiredEmployees.Add(employeeData);
            
            var employeeGameObject = new GameObject("Employee");
            var emp = employeeGameObject.AddComponent<Employee>();
            
            emp.init(employeeData);
            var employeeGUI = Instantiate(EmployeeHiredPrefab);
            employeeGUI.transform.parent = EmployeeHiredContent.transform;
            //For whatever Reason the scale is set to 0.6. So we change it back to 1
            employeeGUI.transform.localScale = Vector3.one;
            employeeGUI.GetComponent<HiredEmployeeUiBuilder>().SetEmp(emp, () =>
            {
                FireEmployee(emp.EmployeeData);
                Destroy(emp.gameObject);
                Destroy(employeeGUI);
            });
        }
        
        public void RemoveEmployeeForHire(EmployeeData employeeData)
        {
            data.employeesForHire.Remove(employeeData);
            Destroy(EmployeeToGuiMap[employeeData]);
        }
        
        public void Cleanup()
        {
            GameTimeDayTickEvent.RemoveListener(dayChangedAction);
        }

        /// <summary>
        /// Hires the specified employee.
        /// If the employee does not exist in the employeesForHire List this method will do nothing;
        /// </summary>
        /// <param name="empData">The employee to hire.</param>
        public void HireEmployee(EmployeeData empData, GameObject employeeForHireGui)
        {
            if (!data.employeesForHire.Contains(empData)) return;
            
            RemoveEmployeeForHire(empData);
            AddHiredEmployee(empData);
        }

        /// <summary>
        /// Fires an employee. The employee will be removed from hiredEmployees and placed in exEmployees.  
        /// </summary>
        /// <param name="emp"></param>
        public void FireEmployee(EmployeeData emp)
        {
            if (!data.hiredEmployees.Contains(emp)) return;
            data.exEmplyoees.Add(emp);
            data.hiredEmployees.Remove(emp);
        }

        /// <summary>
        /// Removes the first Employee from the EmployeeForHire List and creates a new one.
        /// TODO: Pay Employees
        /// </summary>
        public void DayChanged(Object date)
        {
            var gameDate = (GameDate) date;
            
            RemoveEmployeeForHire(data.employeesForHire[0]);
            var employeeData = GenerateEmployeeForHire();
            AddEmployeeForHire(employeeData);
        }
        
        public EmployeeManagerData GetData()
        {
            return data;
        }

        private void AddEmployeeForHireToGui(EmployeeData employeeData)
        {
            GameObject empGUI = Instantiate(EmployeeForHirePrefab);
            empGUI.transform.SetParent(EmployeeForHireContent.transform);
            empGUI.transform.localScale = Vector3.one;
            empGUI.GetComponent<HireableEmployeeUiBuilder>().SetEmp(employeeData, () => HireEmployee(employeeData, empGUI));
            
            EmployeeToGuiMap.Add(employeeData, empGUI);
        }
    }
}

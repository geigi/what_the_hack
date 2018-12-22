using System;
using SaveGame;
using System.Collections;
using System.Collections.Generic;
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
                GameObject empGUI = Instantiate(EmployeeForHirePrefab);
                empGUI.transform.parent = EmployeeForHireContent.transform;
                empGUI.transform.localScale = Vector3.one;
                empGUI.GetComponent<HireableEmployeeUiBuilder>().SetEmp(empData, () => HireEmployee(empData, empGUI));
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
        }

        /// <summary>
        /// Load state from a given savegame.
        /// </summary>
        private void LoadState()
        {
            var mainSaveGame = gameObject.GetComponent<SaveGameSystem>().GetCurrentSaveGame();

            this.specialEmployees = contentHub.GetEmployeeLists();

            data = mainSaveGame.employeeManagerData;
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
            data.employeesForHire.Add(newEmployee);

            return newEmployee;
        }

        public void AddEmployeeForHire(EmployeeData employeeData)
        {
            
        }
        
        public void RemoveEmployeeForHire(EmployeeData employeeData)
        {
            
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
            var employeeGameObject = new GameObject("Employee");
            var emp = employeeGameObject.AddComponent<Employee>();
            
            data.hiredEmployees.Add(empData);
            data.employeesForHire.Remove(empData);
            emp.init(empData);
            
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
            Destroy(employeeForHireGui);
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
            
            data.employeesForHire.RemoveAt(0);
            var employeeData = GenerateEmployeeForHire();
            AddEmployeeForHire(employeeData);
        }
        
        public EmployeeManagerData GetData()
        {
            return data;
        }
    }
}

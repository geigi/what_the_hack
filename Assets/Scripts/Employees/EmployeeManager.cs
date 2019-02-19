using System;
using SaveGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI.EmployeeWindow;
using Base;
using Extensions;
using GameSystem;
using GameTime;
using Interfaces;
using UE.Common;
using UE.Events;
using UI.EmployeeWindow;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;
using DayOfWeek = GameTime.DayOfWeek;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Employees
{
    /// <summary>
    /// This class manages all Employees for the game and keeps track, of employees that can be hired,
    /// employees which are hired and ex-employees.
    /// </summary>
    public class EmployeeManager: Singleton<EmployeeManager>, ISaveable<EmployeeManagerData> {
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
        /// Skill employee ui instance.
        /// </summary>
        public SkillEmployeeUi SkillEmployeeUi;
        
        /// <summary>
        /// Event that will be fired when a day changes.
        /// </summary>
        public NetObjectEvent GameTimeDayTickEvent;

        public IntEvent EmployeesNumChangedEvent;

        /// <summary>
        /// The bank object.
        /// </summary>
        public Bank bank;

        /// <summary>
        /// The number of currently employed employees.
        /// </summary>
        public int HiredEmployees => data.hiredEmployees.Count;

        /// <summary>
        /// The maximum number of hired Employees a player can have at the same time
        /// </summary>
        public const int MaxNumberOfHiredEmployees = 4;

        /// <summary>
        /// The maximum number of hireable Employees, that are in the list at the same time.
        /// </summary>
        public const int MaxNumberOfHireableEmployees = 4;

        /// <summary>
        /// Chance that a new Hireable Employee appears per Day
        /// </summary>
        public const float chanceNewEmpForHirePerDay = 1.0f;

        internal EmployeeManagerData data;

        internal static System.Random rand = new Random();

        /// <summary>
        /// List of all special employees.
        /// </summary>
        private EmployeeList specialEmployees;

        /// <summary>
        /// This dictionary maps an employeeData object to its GUI GameObject.
        /// </summary>
        protected internal Dictionary<EmployeeData, GameObject> EmployeeToGuiMap;

        private ContentHub contentHub;

        private UnityAction<object> dayChangedAction;

        private void Awake()
        {
            contentHub = ContentHub.Instance;
            EmployeeToGuiMap = new Dictionary<EmployeeData, GameObject>();
            this.specialEmployees = contentHub.GetEmployeeLists();
            factoryObject = new EmployeeFactory();
            
            if  (GameSettings.NewGame)
                InitDefaultState();
            else
                LoadState();
            
            bank = contentHub.bank;
            dayChangedAction += DayChanged;
            GameTimeDayTickEvent.AddListener(dayChangedAction);
        }
        
        void Start()
        {
            gameObject.transform.parent = this.gameObject.transform;
            
            var rect = EmployeeForHireContent.GetComponent<RectTransform>();
            rect.ResetPosition();
            rect = EmployeeHiredContent.GetComponent<RectTransform>();
            rect.ResetPosition();

            if (GameSettings.NewGame)
            {
                foreach (var startSpecial in GenerateStartEmployees())
                {
                    AddHiredEmployee(startSpecial);
                }
                for (int i = data.employeesForHire.Count; i < 4; i++)
                {
                    EmployeeData empData = GenerateEmployeeForHire();
                    AddEmployeeForHire(empData);
                }
            }
        }

        /// <summary>
        /// Initializes the EmployeeManager. This Method should be called before using the Manager.
        /// Generates 4 employees for hire.
        /// </summary>
        protected internal void InitDefaultState()
        {
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
            var mainSaveGame = SaveGameSystem.Instance.GetCurrentSaveGame();
            data = mainSaveGame.employeeManagerData;

            foreach (var employeeData in data.employeesForHire)
            {
                AddEmployeeForHireToGui(employeeData);
            }

            foreach (var employeeData in data.hiredEmployees)
            {
                SpawnEmployee(employeeData, false);
            }
        }

        public List<EmployeeData> GenerateStartEmployees()
        {
            return specialEmployees.employeeList.FindAll(empDef => empDef.StartEmployee)
                .ConvertAll(empDef => factoryObject.GenerateSpecialEmployee(empDef));
        }

        /// <summary>
        /// Puts a new employee in the employeeForHire List.
        /// </summary>
        public EmployeeData GenerateEmployeeForHire()
        {
            EmployeeData newEmployee = new EmployeeData();
            newEmployee = factoryObject.GetNewEmployee();
            
            return newEmployee;
        }

        public void AddEmployeeForHire(EmployeeData employeeData)
        {
            data.employeesForHire.Add(employeeData);
            AddEmployeeForHireToGui(employeeData);
            if (HiredEmployees >= MaxNumberOfHiredEmployees)
                EmployeeToGuiMap[employeeData].GetComponent<HireableEmployeeUiBuilder>().DisableHireButton(true);
        }

        public void AddHiredEmployee(EmployeeData employeeData)
        {
            data.hiredEmployees.Add(employeeData);

            SpawnEmployee(employeeData, true);
        }

        private void SpawnEmployee(EmployeeData employeeData, bool isFreshman)
        {
            var employeeGameObject = new GameObject("Employee");
            var emp = employeeGameObject.AddComponent<Employee>();

            emp.init(employeeData, isFreshman);
            var employeeGUI = Instantiate(EmployeeHiredPrefab);
            employeeGUI.transform.SetParent(EmployeeHiredContent.transform, false);
            employeeGUI.GetComponent<HiredEmployeeUiBuilder>().SetEmp(emp, emp.stateEvent, () =>
            {
                FireEmployee(emp.EmployeeData);
                Destroy(emp.gameObject);
                Destroy(employeeGUI);
            }, SkillEmployeeUi);
        }

        public virtual void RemoveEmployeeForHire(EmployeeData employeeData)
        {
            data.employeesForHire.Remove(employeeData);
            Destroy(EmployeeToGuiMap[employeeData]);
            EmployeeToGuiMap.Remove(employeeData);
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
            if (HiredEmployees >= MaxNumberOfHiredEmployees)
            {
                data.employeesForHire.ForEach(emp =>
                    EmployeeToGuiMap[emp].GetComponent<HireableEmployeeUiBuilder>().DisableHireButton(true));
            }
            EmployeesNumChangedEvent.Raise(data.hiredEmployees.Count);
        }

        /// <summary>
        /// Fires an employee. The employee will be removed from hiredEmployees and placed in exEmployees.  
        /// </summary>
        /// <param name="emp"></param>
        public void FireEmployee(EmployeeData emp)
        {
            if (!data.hiredEmployees.Contains(emp)) return;
            data.hiredEmployees.Remove(emp);

            if (emp.EmployeeDefinition?.SpawnLikelihood == 1)
            {
                AddEmployeeForHire(emp);
            }
            else
            {
                data.exEmplyoees.Add(emp);
            }

            if (HiredEmployees < MaxNumberOfHiredEmployees)
            {
                data.employeesForHire.ForEach(empGUI =>
                    EmployeeToGuiMap[empGUI].GetComponent<HireableEmployeeUiBuilder>().DisableHireButton(false));
            }
            EmployeesNumChangedEvent.Raise(data.hiredEmployees.Count);
        }

        public int minimumNumberOfEmployees = 2;

        /// <summary>
        /// Removes some Employees from the EmployeeForHire List and creates a new one.
        /// </summary>
        public void DayChanged(object date)
        {
            var gameDate = (GameDate) date;
            data.employeesForHire.FindAll(data => data.hireableDays > 0).ForEach(data => data.hireableDays--);
            data.employeesForHire.FindAll(data => data.hireableDays == 0).ForEach(RemoveEmployeeForHire);

            while (data.employeesForHire.Count < minimumNumberOfEmployees || (data.employeesForHire.Count < MaxNumberOfHireableEmployees && 
                rand.NextDouble() < chanceNewEmpForHirePerDay))
            {
                var employeeData = GenerateEmployeeForHire();
                AddEmployeeForHire(employeeData);
            }

            //Pay Employees, at the start of each week.
            if (gameDate.DayOfWeek == DayOfWeek.Monday)
                data.hiredEmployees.ForEach(emp => bank.Pay(emp.Salary));
        }

        public Employee GetEmployee(EmployeeData data)
        {
            Employee employee = null;
            foreach (Transform c in GameObject.FindWithTag("EmployeeLayer").transform)
            {
                var employeeComponent = c.gameObject.GetComponent<Employee>();
                if (employeeComponent != null && employeeComponent.EmployeeData == data)
                    employee = employeeComponent;
            }

            return employee;
        }

        /// <summary>
        /// Get the highest across all employees.
        /// </summary>
        /// <returns></returns>
        public int GetCurrentMaxEmployeeLevel()
        {
            int highestLevel = 0;
            
            foreach (var employee in data.hiredEmployees)
            {
                if (employee.Level > highestLevel)
                    highestLevel = employee.Level;
            }

            return highestLevel;
        }
        
        public virtual EmployeeManagerData GetData()
        {
            return data;
        }

        protected internal virtual void AddEmployeeForHireToGui(EmployeeData employeeData)
        {
            GameObject empGUI = Instantiate(EmployeeForHirePrefab);
            empGUI.transform.SetParent(EmployeeForHireContent.transform);
            empGUI.transform.localScale = Vector3.one;
            empGUI.GetComponent<HireableEmployeeUiBuilder>().SetEmp(employeeData, () =>
            {
                HireEmployee(employeeData, empGUI);
                bank.Pay(employeeData.Prize);
            });
            
            EmployeeToGuiMap.Add(employeeData, empGUI);
        }
    }
}

using System;
using Base;
using Employees;
using Interfaces;
using Items;
using Missions;
using Team;
using UE.Events;
using UE.StateMachine;
using UI;
using UI.EmployeeWindow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utils;
using Wth.ModApi.Employees;
using Object = UnityEngine.Object;

namespace World
{
    public class GameSelectionManager : Singleton<GameSelectionManager>
    {
        public State SelectMissionState;
        public EmployeeInfoUi EmployeeInfo;
        public WorkplaceInfoUi WorkplaceInfo;
        public ObjectEvent EmployeeFiredEvent;
        
        private Workplace workplace;
        private UnityAction<Object> employeeFiredAction;
        
        public Workplace Workplace
        {
            get => workplace;
            set
            {
                if (workplaceSelected)
                {
                    WorkplaceInfo.Deselect();
                    workplace.OnDeselect();
                }
                
                workplaceSelected = true;
                workplace = value;
                WorkplaceInfo.Select(workplace);
            }
        }
        
        private bool workplaceSelected = false;

        public bool WorkplaceSelected => workplace;

        public void ClearWorkplace()
        {
            if (workplaceSelected)
            {
                WorkplaceInfo.Deselect();
                workplace.OnDeselect();
            }
            workplace = null;
            workplaceSelected = false;
        }
        
        private Employee employee;
        public Employee Employee
        {
            get => employee;
            set
            {
                if (employeeSelected)
                {
                    employee.OnDeselect();
                    EmployeeInfo.Deselect();
                }
                
                employeeSelected = true;
                employee = value;
                EmployeeInfo.Select(employee);
            }
        }
        
        private bool employeeSelected = false;
        public bool EmployeeSelected => employeeSelected;

        private void Awake()
        {
            employeeFiredAction = onEmployeeFired;
            EmployeeFiredEvent.AddListener(onEmployeeFired);
        }

        public void ClearEmployee()
        {
            if (employeeSelected)
            {
                employee.OnDeselect();
                EmployeeInfo.Deselect();
            }
            employee = null;
            employeeSelected = false;
        }
        
        public void MissionSelected(Mission mission)
        {
            if (EmployeeSelected && WorkplaceSelected)
            {
                employee.GoToWorkplace(workplace, mission);
                ClearEmployee();
                ClearWorkplace();
                SelectMissionState.stateManager.InitialState.Enter();
            }
        }

        private void onEmployeeFired(Object emp)
        {
            if (employeeSelected && employee == (Employee)emp)
                ClearEmployee();
        }
    }
}
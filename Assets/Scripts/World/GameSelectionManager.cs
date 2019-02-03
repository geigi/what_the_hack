using System;
using Base;
using Interfaces;
using Items;
using Missions;
using Team;
using UE.StateMachine;
using UI;
using UI.EmployeeWindow;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace World
{
    public class GameSelectionManager : Singleton<GameSelectionManager>
    {
        public State SelectMissionState;
        public EmployeeInfoUi EmployeeInfo;

        private Workplace workplace;
        public Workplace Workplace
        {
            get => workplace;
            set
            {
                if (workplaceSelected) workplace.OnDeselect();
                workplaceSelected = true;
                workplace = value;
            }
        }
        
        private bool workplaceSelected = false;

        public bool WorkplaceSelected => workplace;

        public void ClearWorkplace()
        {
            if (workplaceSelected) workplace.OnDeselect();
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
    }
}
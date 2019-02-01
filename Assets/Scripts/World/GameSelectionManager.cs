using System;
using Interfaces;
using Items;
using Missions;
using Team;
using UE.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace World
{
    public class GameSelectionManager : MonoBehaviour
    {
        #region Singleton
        private static readonly Lazy<GameSelectionManager> lazy = 
            new Lazy<GameSelectionManager>(() => GameObject.FindWithTag("Managers").GetComponent<GameSelectionManager>());

        /// <summary>
        /// The single Instance of this class
        /// </summary>
        public static GameSelectionManager Instance => lazy.Value;

        private GameSelectionManager() { }
        #endregion

        public State SelectMissionState;

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
                if (employeeSelected) employee.OnDeselect();
                employeeSelected = true;
                employee = value;
            }
        }
        
        private bool employeeSelected = false;
        public bool EmployeeSelected => employeeSelected;

        public void ClearEmployee()
        {
            if (employeeSelected) employee.OnDeselect();
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
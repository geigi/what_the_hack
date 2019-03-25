using System;
using Assets.Scripts.NotificationSystem;
using Wth.ModApi.Employees;
using Wth.ModApi.Tools;

namespace Employees.Specials
{
    /// <summary>
    /// This employee special makes an employee leave the team randomly at a small chance.
    /// </summary>
    [Serializable]
    public class Unreliable: EmployeeSpecial
    {
        private const float chance = 0.02f;
        
        public override string GetDisplayName()
        {
            return "Unreliable";
        }

        public override string GetDescription()
        {
            return "May leave the company any day.";
        }

        public override float GetScoreCost()
        {
            return -14;
        }

        public override float GetSalaryRelativeFactor()
        {
            return 0.6f;
        }

        public override float GetHiringCostRelativeFactor()
        {
            return 0.6f;
        }

        public override void OnDayChanged(EmployeeData employeeData)
        {
            if (RandomUtils.rand() > chance) return;
            
            NotificationCenter.Instance.Warning("An unreliable employee " + employeeData.Name + " has left the team.");
            EmployeeManager.Instance.FireEmployee(employeeData);
        }
    }
}
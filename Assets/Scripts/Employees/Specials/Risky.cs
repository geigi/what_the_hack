using System;
using Wth.ModApi.Employees;

namespace Employees.Specials
{
    /// <summary>
    /// This employee special makes critical fail and success rolls more likely.
    /// </summary>
    [Serializable]
    public class Risky: EmployeeSpecial
    {
        public override string GetDisplayName()
        {
            return "Risky";
        }

        public override string GetDescription()
        {
            return "The 'all-or-nothing' way of life." ;
        }

        public override float GetScoreCost()
        {
            return 0;
        }

        public override int GetCriticalSuccessChance()
        {
            return 2;
        }
        
        public override int GetCriticalFailureChance()
        {
            return 2;
        }
    }
}
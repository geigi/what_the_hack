using System;
using Wth.ModApi.Employees;

namespace Employees.Specials
{
    /// <summary>
    /// This employee special improves employees chance for a critical success roll.
    /// </summary>
    [Serializable]
    public class LuckyDevil: EmployeeSpecial
    {
        public override string GetDisplayName()
        {
            return "Lucky Devil";
        }

        public override string GetDescription()
        {
            return "Wins every game of poker.";
        }

        public override float GetScoreCost()
        {
            return 6;
        }

        public override int GetCriticalSuccessChance()
        {
            return 1;
        }
    }
}
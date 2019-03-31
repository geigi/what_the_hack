using System;
using Wth.ModApi.Employees;
using Wth.ModApi.Tools;

namespace Employees.Specials
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Twitter: EmployeeSpecial
    {
        public override string GetDisplayName()
        {
            return "Covfefe";
        }

        public override string GetDescription()
        {
            return "Despite the constant negative press covfefe.";
        }

        public override float GetScoreCost()
        {
            return 0;
        }

        public override void OnTimeStepChanged(object employeeData)
        {
            if (RandomUtils.RollDice(10) == 1)
                EmojiBubbleFactory.Instance.EmpReaction(EmojiBubbleFactory.EmojiType.TWITTER, 
                    EmployeeManager.Instance.GetEmployee((EmployeeData) employeeData), 
                    EmojiBubbleFactory.EMPLYOEE_OFFSET, EmojiBubbleFactory.StandardDisplayTime);
        }
    }
}
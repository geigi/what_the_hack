using Wth.ModApi.Employees;

namespace Employees.Specials
{
    /// <summary>
    /// This employee special improves employees learning ability by offering some extra score points for completed missions.
    /// </summary>
    public class FastLearner: EmployeeSpecial
    {
        public override string GetDisplayName()
        {
            return "Fast Learner";
        }

        public override string GetDescription()
        {
            return "Quickly gets into things.";
        }

        public override float GetScoreCost()
        {
            return 7;
        }

        public override float GetLearningMultiplier()
        {
            return 0.5f;
        }
    }
}
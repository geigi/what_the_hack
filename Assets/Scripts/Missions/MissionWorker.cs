using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;
using Wth.ModApi.Tools;
using Random = System.Random;

namespace Missions
{
    /// <summary>
    /// This class is responsible for working on a mission.
    /// </summary>
    public class MissionWorker
    {
        /// <summary>
        /// This value is the most important tuning value for balancing.
        /// </summary>
        public const float BASE_VALUE = 10f;
        /// <summary>
        /// This value influences the random part of the progress value.
        /// </summary>
        public const float RANDOM_FACTOR = 10f;
        /// <summary>
        /// This value defines the dice sides for critical success/failure chances.
        /// </summary>
        public const int DICE_SIDES = 40;

        private List<EmployeeData> employees;
        private Mission mission;

        private Random random;
        private UnityAction<int> gameTickAction;

        public MissionWorker(Mission mission)
        {
            this.mission = mission;
            random = new Random();
            gameTickAction += OnTimeStep;
            employees = new List<EmployeeData>();
            ContentHub.Instance.GameStepEvent.AddListener(gameTickAction);
        }

        /// <summary>
        /// Call this before destroying an instance.
        /// It releases event listeners.
        /// </summary>
        public void Cleanup()
        {
            ContentHub.Instance.GameStepEvent.RemoveListener(gameTickAction);
        }

        /// <summary>
        /// Add an employee to this mission worker.
        /// The employee will start working on the mission at the next time step event.
        /// </summary>
        /// <param name="employee"></param>
        public void AddEmployee(EmployeeData employee)
        {
            employees.Add(employee);
        }

        /// <summary>
        /// Remove an employee from this mission worker.
        /// </summary>
        /// <param name="employee"></param>
        public void RemoveEmployee(EmployeeData employee)
        {
            employees.Remove(employee);
        }

        /// <summary>
        /// This method calculates the mission progress for a given skill.
        /// The calculation can be manipulated and tuned with the <see cref="BASE_VALUE"/> and <see cref="RANDOM_FACTOR"/> constants.
        /// Calculation must be balanced for the entire length of a game.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="skill"></param>
        /// <param name="skillValue"></param>
        /// <returns></returns>
        private float WorkOnSkill(EmployeeData employee, SkillDefinition skill, float skillValue)
        {
            int employeeValue;
            employeeValue = employee.HasSkill(skill)
                ? employee.GetSkill(skill).level
                // General Purpose should be weaker than a specific skill
                : employee.GetGeneralPurpose().level / 2;

            var stepValue = employeeValue * (1f / (mission.SkillDifficulty[skill] + mission.Difficulty)) *
                            (BASE_VALUE + (float) random.NextDouble() * RANDOM_FACTOR) * 1f /
                            mission.TotalTicks;

            return skillValue + stepValue * GetCricitalChanceFactor(employee);
        }

        /// <summary>
        /// Roll a dice to determine, whether this step is a critical success or failure.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Factor which must be multiplied to the skill step value</returns>
        private float GetCricitalChanceFactor(EmployeeData employee)
        {
            var dice = RandomUtils.RollDice(DICE_SIDES);

            if (dice < employee.CriticalFailureChance)
                return 0.5f;
            else if (dice > DICE_SIDES - employee.CriticalSuccessChance)
                return 2f;
            else
                return 1f;
        }

        /// <summary>
        /// This method starts the calculation of the mission progress.
        /// It also notifies ProgressChanged event listeners.
        /// </summary>
        /// <param name="step"></param>
        private void OnTimeStep(int step)
        {
            foreach (var employee in employees)
            {
                List<SkillDefinition> skills = new List<SkillDefinition>(mission.Progress.Keys);
                foreach (var skill in skills)
                {
                    var value = mission.Progress[skill];
                    if (value >= 1.0f)
                    {
                        // Skill is already fulfilled
                        continue;
                    }

                    var newValue = WorkOnSkill(employee, skill, value);
                    
                    
                    mission.Progress[skill] = newValue;
                    mission.ProgressChanged.Invoke(new KeyValuePair<SkillDefinition, float>(skill, newValue));
                }
            }

            if (mission.Completed())
            {
                mission.Finish();
            }
        }
    }
}
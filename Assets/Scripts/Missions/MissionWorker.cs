using System;
using System.Collections.Generic;
using System.Linq;
using UI.EmployeeWindow;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;
using Wth.ModApi.Missions;
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
        public const float BASE_VALUE = 1.4f;
        /// <summary>
        /// This value influences the random part of the progress value.
        /// </summary>
        public const float RANDOM_FACTOR = 1f;
        /// <summary>
        /// This value defines the dice sides for critical success/failure chances.
        /// </summary>
        public const int DICE_SIDES = 40;
        /// <summary>
        /// When a mission requires multiple skills and some of them are finished,
        /// the remaining skill points will be generated x times faster by this factor.
        /// </summary>
        public const float SKILLS_COMPLETED_BOOST_FACTOR = 1.5f;

        /// <summary>
        /// This dictionary reflects whether each skill is fulfilled by an employee skill (true) or by all purpose (false).
        /// </summary>
        public Dictionary<SkillDefinition, bool> SkillsFulfilled
        {
            get
            {
                Dictionary<SkillDefinition, bool> dic = new Dictionary<SkillDefinition, bool>();
                
                foreach (var skill in mission.Progress)
                    dic[skill.Key] = employees.Any(e => e.HasSkill(skill.Key));

                return dic;
            }
        }
        
        /// <summary>
        /// Gets invoked when the employees working on this mission changed.
        /// </summary>
        public UnityEvent EmployeesChanged;

        private List<EmployeeData> employees;
        private Mission mission;
        private MissionHook hook;

        private float AverageProgress => mission.Progress.Average(m => m.Value);

        private Random random;
        private UnityAction<int> gameTickAction;
        private UnityAction<bool> hookCompletedAction;

        public MissionWorker(Mission mission)
        {
            EmployeesChanged = new UnityEvent();
            this.mission = mission;
            random = new Random();
            gameTickAction += OnTimeStep;
            hookCompletedAction = OnHookFinished;
            employees = new List<EmployeeData>();
            
            if (AverageProgress > 0f)
                testMissionHooks();
            
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
            mission.WorkStarted = true;
            employees.Add(employee);
            EmployeesChanged.Invoke();
        }

        /// <summary>
        /// Remove an employee from this mission worker.
        /// </summary>
        /// <param name="employee"></param>
        public void RemoveEmployee(EmployeeData employee)
        {
            employees.Remove(employee);
            EmployeesChanged.Invoke();
        }

        /// <summary>
        /// Is a given employee working on this mission?
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public bool HasEmployee(EmployeeData emp)
        {
            return employees.Contains(emp);
        }

        /// <summary>
        /// Estimates, whether this mission will complete successfully or not.
        /// </summary>
        /// <returns></returns>
        public bool WillCompleteSuccessfully()
        {
            int completedSkills = mission.Progress.Count(s => s.Value >= 1f);
            Dictionary<SkillDefinition, float> predictedProgress = mission.Progress.ToDictionary(entry => entry.Key,
                entry => entry.Value);
            
            for (int i = 0; i < mission.RemainingTicks; i++)
            {
                List<SkillDefinition> skills = new List<SkillDefinition>(predictedProgress.Keys);
                
                foreach (var employee in employees)
                {
                    foreach (var skill in skills)
                    {
                        var value = predictedProgress[skill];
                        if (value >= 1.0f)
                        {
                            // Skill is already fulfilled
                            continue;
                        }

                        var newValue = WorkOnSkill(employee, skill, value, completedSkills);
                        predictedProgress[skill] = newValue;
                    }
                }
            }

            if (predictedProgress.Values.All(v => v >= 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method calculates the mission progress for a given skill.
        /// The calculation can be manipulated and tuned with the <see cref="BASE_VALUE"/> and <see cref="RANDOM_FACTOR"/> constants.
        /// Calculation must be balanced for the entire length of a game.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="skill"></param>
        /// <param name="skillValue"></param>
        /// <param name="completedSkills"></param>
        /// <returns></returns>
        private float WorkOnSkill(EmployeeData employee, SkillDefinition skill, float skillValue, int completedSkills)
        {
            int employeeValue;
            employeeValue = employee.HasSkill(skill)
                ? employee.GetSkill(skill).Level
                // General Purpose should be weaker than a specific skill
                : employee.GetGeneralPurpose().Level / 2;
            
            var stepValue = Math.Max(employeeValue, 1) * (1f / (mission.SkillDifficulty[skill] * mission.Difficulty)) *
                            (RandomUtils.mult_var(0.1f) * RANDOM_FACTOR) * 1f /
                            mission.TotalTicks * BASE_VALUE * Math.Max(1f, (float) Math.Pow(SKILLS_COMPLETED_BOOST_FACTOR, completedSkills));

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
            if (mission.Paused) return;
            
            int completedSkills = mission.Progress.Count(s => s.Value >= 1f);
            
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

                    var newValue = WorkOnSkill(employee, skill, value, completedSkills);
                    
                    mission.Progress[skill] = newValue;
                    mission.ProgressChanged.Invoke(new KeyValuePair<SkillDefinition, float>(skill, newValue));
                }
            }

            testMissionHooks();

            if (mission.Completed())
            {
                mission.Finish();
            }
        }

        private void testMissionHooks()
        {
            var missionHooks = mission.Definition.MissionHooks.MissionHooks;
            foreach (var missionHook in missionHooks)
            {
                if (!mission.HookStatus[missionHook] && missionHook.Appear <= AverageProgress && hook == null)
                {
                    // Pause mission and fire interaction
                    mission.Paused = true;
                    hook = missionHook;
                    missionHook.Completed.AddListener(hookCompletedAction);
                    mission.MissionHookSpawn.Invoke(missionHook);
                }
            }
        }

        private void OnHookFinished(bool success)
        {
            mission.MissionHookCompleted.Invoke(success);
            
            if (success)
            {
                mission.HookStatus[hook] = true;
                mission.Paused = false;
            }
            else
            {
                MissionManager.Instance.AbortMission(mission);
            }
            
            hook.Completed.RemoveListener(hookCompletedAction);
            hook = null;
        }
    }
}
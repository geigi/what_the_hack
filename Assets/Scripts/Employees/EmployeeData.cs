using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Wth.ModApi.Employees
{
    /// <summary>
    /// This class is a data container class for an employee. Do not implement any logic in here.
    /// All data that needs to be persistent should be in this class to be serialized.
    /// </summary>
    [Serializable]
    public class EmployeeData
    {
        /// <summary>
        /// Level of the employee > 0.
        /// </summary>
        public int Level = 1;
        
        /// <summary>
        /// Salary of the employee.
        /// </summary>
        public int Salary = 100;

        /// <summary>
        /// Prize for buying the employee.
        /// </summary>
        public int Prize = 100;
        
        /// <summary>
        /// List of learned skills.
        /// </summary>
        public List<Skill> Skills;
        
        /// <summary>
        /// List of specials.
        /// </summary>
        public List<EmployeeSpecial> Specials;
        
        /// <summary>
        /// World position of the employee.
        /// </summary>
        public Vector2Int Position;
        
        /// <summary>
        /// This is null when the employee is a normal generated employee.
        /// Only populated for special created Employees.
        /// </summary>
        public EmployeeDefinition EmployeeDefinition;

        /// <summary>
        /// This is null when the Employee is a special emplyoee.
        /// Only Populated for genderated employees.
        /// </summary>
        public EmployeeGeneratedData generatedData;

        /// <summary>
        /// This enum represents the current state of the employee.
        /// </summary>
        public Enums.EmployeeState State;

        /// <summary>
        /// Represents how many days the Employee wil continue to stay in the "for hire" list until he is removed.
        /// </summary>
        public int hireableDays;
        
        /// <summary>
        /// The critical failure bonus of this employee.
        /// Only EmployeeSpecials can modify this value.
        /// </summary>
        public int CriticalFailureChance
        {
            get
            {
                int result = 0;

                foreach (var special in Specials)
                {
                    result += special.GetCriticalFailureChance();
                }
            
                return result;
            }
        }
    
        /// <summary>
        /// The critical success bonus of this employee.
        /// Only EmployeeSpecials can modify this value.
        /// </summary>
        public int CriticalSuccessChance
        {
            get
            {
                int result = 0;

                foreach (var special in Specials)
                {
                    result += special.GetCriticalSuccessChance();
                }
            
                return result;
            }
        }
        
        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public EmployeeData()
        {
        }
        
        /// <summary>
        /// Tests whether an employee has a specific skill.
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool HasSkill(SkillDefinition skill)
        {
            return Skills.Any(s => s.skillData == skill);
        }

        /// <summary>
        /// Get a specific Skill object.
        /// </summary>
        /// <param name="skill">The skill definition</param>
        /// <returns></returns>
        public Skill GetSkill(SkillDefinition skill)
        {
            return Skills.First(s => s.skillData == skill);
        }

        public Skill GetGeneralPurpose()
        {
            return Skills.First(s => s.skillData == ContentHub.Instance.GeneralPurposeSkill);
        }
    }
}
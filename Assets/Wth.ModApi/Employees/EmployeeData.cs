using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        public int Salary;
        
        /// <summary>
        /// List of learned skills.
        /// </summary>
        public List<SkillDefinition> Skills;
        
        /// <summary>
        /// List of specials.
        /// </summary>
        public List<EmployeeSpecial> Specials;
        
        /// <summary>
        /// World position of the employee.
        /// </summary>
        public Vector2 Position;
        
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
        /// Empty Constructor.
        /// </summary>
        public EmployeeData() {}
    }
}
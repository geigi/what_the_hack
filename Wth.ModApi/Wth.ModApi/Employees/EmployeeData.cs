using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using Wth.ModApi.Tools;

namespace Wth.ModApi
{
    /// <summary>
    /// This class is a data container class for an employee. Do not implement any logic in here.
    /// All data that needs to be persistent should be in this class to be serialized.
    /// </summary>
    [Serializable]
    public class EmployeeData: ISerializable
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

        public EmployeeData()
        {
            
        }
        
        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EmployeeData(SerializationInfo info, StreamingContext context)
        {
            
        }
        
        /// <summary>
        /// Gets called on serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("level", Level, typeof(int));
            info.AddValue("salary", Salary, typeof(int));

            info.AddValue("skills", ScriptableObjectManager.Instance.GetKeys(Skills), typeof(List<string>));
            info.AddValue("specials", Specials, typeof(List<EmployeeSpecial>));
            info.AddValue("x", Position.x, typeof(float));
            info.AddValue("y", Position.y, typeof(float));
            info.AddValue("definition", Level, typeof(string));
        }
    }
}
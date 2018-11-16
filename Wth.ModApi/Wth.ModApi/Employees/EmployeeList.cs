using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi.Employees {
    /// <summary>
    /// This class contains a list of all custom employees for this mod.
    /// </summary>
    [CreateAssetMenu(fileName = "EmployeeList", menuName = "What_The_Hack ModApi/Employees/Employee List", order = -301)]
    public class EmployeeList : ScriptableObject
    {
        /// <summary>
        /// List of all custom employees.
        /// </summary>
        public List<EmployeeDefinition> employeeList;
    }
}
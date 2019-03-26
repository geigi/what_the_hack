using System;
using System.Collections.Generic;
using System.Security.Policy;
using Wth.ModApi.Employees;

namespace Employees
{
    [Serializable]
    public class EmployeeManagerData
    {
        /// <summary>
        /// This boolean tracks, whether the first employee was hired by the user.
        /// </summary>
        public bool FirstEmployeeHired = false;
        
        /// <summary>
        /// List to store all employees that can be hired.
        /// </summary>
        public List<EmployeeData> employeesForHire;

        /// <summary>
        /// List to store all hired Employees.
        /// </summary>
        public List<EmployeeData> hiredEmployees;

        /// <summary>
        /// List to store all ex-employees.
        /// </summary>
        public List<EmployeeData> exEmplyoees;
    }
}
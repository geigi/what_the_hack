using System;
using System.Collections.Generic;
using Wth.ModApi.Employees;

namespace Assets.Scripts.Employees
{
    [Serializable]
    public class EmployeeManagerData
    {
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
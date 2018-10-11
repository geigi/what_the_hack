﻿using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi.Employees {
[CreateAssetMenu(fileName = "EmployeeList", menuName = "What_The_Hack ModApi/Employees/Employee List", order = 1)]
public class EmployeeList : ScriptableObject
{
    public List<EmployeeDefinition> employeeList;
}
}
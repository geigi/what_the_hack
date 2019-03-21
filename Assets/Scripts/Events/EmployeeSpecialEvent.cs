using System;
using UnityEngine.Events;
using Wth.ModApi.Employees;

namespace DefaultNamespace
{
    [Serializable]
    public class EmployeeSpecialEvent : UnityEvent<EmployeeSpecial>
    {
    }
}
using System;
using Missions;
using UnityEngine;
using Wth.ModApi.Employees;

namespace Team
{
    [Serializable]
    public class WorkplaceData
    {
        public Vector2Int Position;
        public EmployeeData OccupyingEmployee;
        public Mission Mission;
    }
}
using System;
using System.Collections.Generic;

namespace Missions
{
    /// <summary>
    /// This class contains all data by the mission manager that needs to be serialized.
    /// </summary>
    [Serializable]
    public class MissionManagerData
    {
        public bool FirstMissionAccepted = false;
        
        public List<Mission> InProgress;
        public List<Mission> Completed;
        public List<Mission> Available;
        public List<Mission> ForceAppearPool;

        public MissionManagerData()
        {
            InProgress = new List<Mission>();
            Available = new List<Mission>();
            Completed = new List<Mission>();
            ForceAppearPool = new List<Mission>();
        }
    }
}
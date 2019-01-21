using System;
using System.Collections.Generic;

namespace Missions
{
    [Serializable]
    public class MissionManagerData
    {
        public List<Mission> InProgress;
        public List<Mission> Completed;
        public List<Mission> Available;

        public MissionManagerData()
        {
            InProgress = new List<Mission>();
            Available = new List<Mission>();
            Completed = new List<Mission>();
        }
    }
}
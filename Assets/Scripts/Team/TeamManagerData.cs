using System;
using System.Collections.Generic;

namespace Team
{
    [Serializable]
    public class TeamManagerData
    {
        public int Floors = 1;
        public int Workplaces = 1;
        public List<WorkplaceData> WorkplaceDatas;
    }
}
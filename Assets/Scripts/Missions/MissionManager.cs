using Interfaces;
using UnityEngine;

namespace Missions
{
    public class MissionManager : MonoBehaviour, Saveable<MissionManagerData>
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public MissionManagerData GetData()
        {
            throw new System.NotImplementedException();
        }
    }
}

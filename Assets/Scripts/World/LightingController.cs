using UnityEngine;

namespace World
{
    public class LightingController : MonoBehaviour
    {
        public Light EnvironmentLight;
        public Light[] RoomLights;

        [SerializeField]
        private LightModes lightMode;
        public LightModes LightMode
        {
            get { return lightMode; }
            set
            {
                lightMode = value;
                if (value == LightModes.Day)
                {
                    EnvironmentLight.intensity = WorldMax;
                    foreach (var l in RoomLights)
                    {
                        l.intensity = RoomMin;
                    }
                }
                else if (value == LightModes.Night)
                {
                    EnvironmentLight.intensity = WorldMin;
                    foreach (var l in RoomLights)
                    {
                        l.intensity = RoomMax;
                    }
                }
            }
        }

        public float WorldMin = 0f;
        public float WorldMax = 0.6f;
        public float RoomMin = 0.15f;
        public float RoomMax = 0.6f;

        public enum LightModes
        {
            Day,
            Night,
        }
    }
}
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

namespace World
{
    public class LightingController : MonoBehaviour
    {
        public Light EnvironmentLight;
        public Light[] RoomLights;
        public Light[] FloorLampSpots;
        public Light[] FloorLampPoints;

        public float WorldMin = 0f;
        public float WorldMax = 0.6f;
        public float RoomMin = 0.15f;
        public float RoomMax = 0.6f;
        public float FloorLampMax = 1f;
        public float FloorLampMin = 0f;

        public void SetDayTime(float daytime)
        {
            SettingsManager.SetDayTime(daytime);
            
            var worldRange = WorldMax - WorldMin;
            EnvironmentLight.intensity = worldRange * daytime + WorldMin;

            var roomRange = RoomMax - RoomMin;
            foreach (var roomLight in RoomLights)
            {
                roomLight.intensity = RoomMax - roomRange * daytime;
            }
            
            var floorLampRange = FloorLampMax - FloorLampMin;
            foreach (var floorLamp in FloorLampSpots)
            {
                floorLamp.intensity = FloorLampMax - floorLampRange * daytime;
            }
            
            floorLampRange = (FloorLampMax / 2) - FloorLampMin;
            foreach (var floorLamp in FloorLampPoints)
            {
                floorLamp.intensity = (FloorLampMax / 2) - floorLampRange * daytime;
            }
        }
    }
}
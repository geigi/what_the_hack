using GameSystem;
using UnityEngine;
using UnityEngine.UI;

namespace World
{
    public class LightingController : MonoBehaviour
    {
        public Light EnvironmentLight;
        public Light[] RoomLights;
        public Slider DaytimeSlider;

        public float WorldMin = 0f;
        public float WorldMax = 0.6f;
        public float RoomMin = 0.15f;
        public float RoomMax = 0.6f;

        private void Start()
        {
            DaytimeSlider.value = SettingsManager.GetDayTime();
            UpdateDayTime();
            DaytimeSlider.onValueChanged.AddListener(delegate {UpdateDayTime();});
        }

        private void UpdateDayTime()
        {
            SettingsManager.SetDayTime(DaytimeSlider.value);
            
            var worldRange = WorldMax - WorldMin;
            EnvironmentLight.intensity = worldRange * DaytimeSlider.value + WorldMin;

            var roomRange = RoomMax - RoomMin;
            foreach (var roomLight in RoomLights)
            {
                roomLight.intensity = RoomMax - roomRange * DaytimeSlider.value;
            }
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using World;

namespace GameSystem
{
    /// <summary>
    /// This class simulates night and day according to the device time.
    /// </summary>
    public class EarthRotation: MonoBehaviour
    {
        public LightingController LightingController;
        [Range(0, 23)]
        public int SunriseHour, SunsetHour;
        [Range(1, 60)]
        public int TransitionMinutes;

        private bool running;
        private Coroutine coroutine;

        private void Start()
        {
            running = true;
            coroutine = StartCoroutine(LightingRoutine());
        }

        private IEnumerator LightingRoutine()
        {
            while (running)
            {
                var date = DateTime.Now;

                if (date.Hour < SunriseHour || date.Hour > SunsetHour)
                {
                    // Night
                    LightingController.SetDayTime(0f);
                }
                else if (date.Hour > SunriseHour && date.Hour < SunsetHour)
                {
                    // Day
                    LightingController.SetDayTime(1f);
                }
                else if (date.Hour == SunriseHour)
                {
                    // Sunrise
                    var value = Math.Max((float) date.Minute / (float) TransitionMinutes, 1f);
                    LightingController.SetDayTime(value);
                }
                else if (date.Hour == SunsetHour)
                {
                    // Sunset
                    var value = Math.Max(1f - ((float) date.Minute / (float) TransitionMinutes), 0f);
                    LightingController.SetDayTime(value);
                }
                
                yield return new WaitForSeconds(15);
            }
        }
    }
}
using UnityEngine;

namespace GameSystem
{
    public class SettingsManager: MonoBehaviour
    {
        /// <summary>
        /// String for pixel perfect camera setting representation.
        /// </summary>
        public const string PixelPerfectCameraKey = "Pixel_Perfect";
        /// <summary>
        /// String for game time mode setting representation.
        /// </summary>
        public const string GameTimeKey = "Game_Time";
        /// <summary>
        /// String for daytime setting representation.
        /// </summary>
        public const string DayTimeKey = "Day_Time";
        
        public enum PixelPerfectCameraValue {
            Off=0,
            Automatic=1,
            ForceOn=2
        }
        
        /// <summary>
        /// This enum represents the two different game modes.
        /// </summary>
        public enum GameTimeMode { Classic, Realtime }

        /// <summary>
        /// Save pixel perfect camera dropdown value to PlayerPrefs.
        /// </summary>
        /// <param name="dropdownValue"></param>
        public void SetPixelPerfectCamera(int dropdownValue) {
            PlayerPrefs.SetInt(PixelPerfectCameraKey, dropdownValue);
        }
        
        /// <summary>
        /// Save game time mode dropdown value to PlayerPrefs.
        /// </summary>
        /// <param name="dropdownValue"></param>
        public static void SetGameTime(int dropdownValue) {
            PlayerPrefs.SetInt(GameTimeKey, dropdownValue);
        }
        
        /// <summary>
        /// Save daytime slider value to PlayerPrefs.
        /// </summary>
        /// <param name="sliderValue">0.0f-1.0f</param>
        public static void SetDayTime(float sliderValue) {
            PlayerPrefs.SetFloat(DayTimeKey, sliderValue);
        }

        /// <summary>
        /// Get the current game time mode.
        /// </summary>
        /// <returns>Current game time mode</returns>
        public static GameTimeMode GetGameTime()
        {
            return (GameTimeMode) PlayerPrefs.GetInt(GameTimeKey);
        }
        
        /// <summary>
        /// Get the daytime slider value from PlayerPrefs.
        /// </summary>
        public static float GetDayTime() {
            return PlayerPrefs.GetFloat(DayTimeKey);
        }
    }
}
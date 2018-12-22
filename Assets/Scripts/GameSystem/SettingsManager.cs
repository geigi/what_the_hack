using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    /// <summary>
    /// This class handles all global application settings.
    /// </summary>
    public class SettingsManager : MonoBehaviour {
        /// <summary>
        /// Dropdown object for the pixel perfect camera setting.
        /// </summary>
        public Dropdown pixelPerfectDropdown;
        /// <summary>
        /// Dropdown object for the game time mode setting.
        /// </summary>
        public Dropdown gameTimeDropdown;

        /// <summary>
        /// Enum for pixel perfect camera setting representation.
        /// </summary>
        public const string PixelPerfectCameraKey = "Pixel_Perfect";
        /// <summary>
        /// Enum for game time mode setting representation.
        /// </summary>
        public const string GameTimeKey = "Game_Time";
        
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
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        public	void Start()
        {
            if (PlayerPrefs.HasKey(PixelPerfectCameraKey)) {
                pixelPerfectDropdown.value = PlayerPrefs.GetInt(PixelPerfectCameraKey);
            }
            else {
                PlayerPrefs.SetInt(PixelPerfectCameraKey, 1);
            }
		    
            if (PlayerPrefs.HasKey(GameTimeKey)) {
                gameTimeDropdown.value = PlayerPrefs.GetInt(GameTimeKey);
            }
            else {
                PlayerPrefs.SetInt(GameTimeKey, 0);
            }
        }

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
        public void SetGameTime(int dropdownValue) {
            PlayerPrefs.SetInt(GameTimeKey, dropdownValue);
        }

        /// <summary>
        /// Get the current game time mode.
        /// </summary>
        /// <returns>Current game time mode</returns>
        public static GameTimeMode GetGameTime()
        {
            return (GameTimeMode) PlayerPrefs.GetInt(GameTimeKey);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    /// <summary>
    /// This class handles all global application settings.
    /// </summary>
    public class SettingsManager : MonoBehaviour {
        public Dropdown pixelPerfectDropdown;
        public Dropdown gameTimeDropdown;

        public const string PixelPerfectCameraKey = "Pixel_Perfect";
        public const string GameTimeKey = "Game_Time";
        public enum PixelPerfectCameraValue {
            Off=0,
            Automatic=1,
            ForceOn=2
        }

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

        public void SetPixelPerfectCamera(int dropdownValue) {
            PlayerPrefs.SetInt(PixelPerfectCameraKey, dropdownValue);
        }
        
        public void SetGameTime(int dropdownValue) {
            PlayerPrefs.SetInt(GameTimeKey, dropdownValue);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    /// <summary>
    /// This class handles all global application settings.
    /// </summary>
    public class MenuSettingsManager : MonoBehaviour {
        /// <summary>
        /// Dropdown object for the pixel perfect camera setting.
        /// </summary>
        public Dropdown pixelPerfectDropdown;
        /// <summary>
        /// Dropdown object for the game time mode setting.
        /// </summary>
        public Dropdown gameTimeDropdown;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        public	void Start()
        {
            if (PlayerPrefs.HasKey(SettingsManager.PixelPerfectCameraKey)) {
                pixelPerfectDropdown.value = PlayerPrefs.GetInt(SettingsManager.PixelPerfectCameraKey);
            }
            else {
                PlayerPrefs.SetInt(SettingsManager.PixelPerfectCameraKey, 1);
            }
		    
            if (PlayerPrefs.HasKey(SettingsManager.GameTimeKey)) {
                gameTimeDropdown.value = PlayerPrefs.GetInt(SettingsManager.GameTimeKey);
            }
            else {
                PlayerPrefs.SetInt(SettingsManager.GameTimeKey, 0);
            }
        }
    }
}
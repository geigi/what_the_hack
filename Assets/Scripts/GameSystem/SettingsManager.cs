using UnityEngine;

namespace GameSystem
{
    public class SettingsManager: MonoBehaviour
    {
        public const float DEFAULT_MUSIC_VOLUME = 1.0f;
        public const float DEFAULT_FX_VOLUME = 1.0f;
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
        /// <summary>
        /// String for music volume setting representation.
        /// </summary>
        public const string MusicVolumeKey = "Music_Volume";
        /// <summary>
        /// String for sound fx volume setting representation.
        /// </summary>
        public const string SoundFxVolumeKey = "Sound_Fx_Volume";
        /// <summary>
        /// String for windowed / fullscreen setting representation (desktop only)
        /// </summary>
        public const string WindowStateKey = "Fullscreen";
        /// <summary>
        /// String for tutorial setting representation
        /// </summary>
        public const string TutorialKey = "Tutorial";
        
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
        /// Save music volume slider value to PlayerPrefs.
        /// </summary>
        /// <param name="sliderValue">0.0f-1.0f</param>
        public static void SetMusicVolume(float sliderValue) {
            PlayerPrefs.SetFloat(MusicVolumeKey, sliderValue);
        }
        
        /// <summary>
        /// Save sound fx volume slider value to PlayerPrefs.
        /// </summary>
        /// <param name="sliderValue">0.0f-1.0f</param>
        public static void SetSoundFxVolume(float sliderValue) {
            PlayerPrefs.SetFloat(SoundFxVolumeKey, sliderValue);
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
        
        /// <summary>
        /// Get the music volume slider value from PlayerPrefs.
        /// </summary>
        public static float GetMusicVolume() {
            if (PlayerPrefs.HasKey(MusicVolumeKey))
                return PlayerPrefs.GetFloat(MusicVolumeKey);
            else
            {
                SetMusicVolume(DEFAULT_MUSIC_VOLUME);
                return DEFAULT_MUSIC_VOLUME;
            }
        }
        
        /// <summary>
        /// Get the sound fx volume slider value from PlayerPrefs.
        /// </summary>
        public static float GetSoundFxVolume() {
            if (PlayerPrefs.HasKey(SoundFxVolumeKey))
                return PlayerPrefs.GetFloat(SoundFxVolumeKey);
            else
            {
                SetSoundFxVolume(DEFAULT_FX_VOLUME);
                return DEFAULT_FX_VOLUME;
            }
        }
        
        /// <summary>
        /// Save tutorial state to PlayerPrefs.
        /// </summary>
        /// <param name="state"></param>
        public static void SetTutorialState(bool state) {
            PlayerPrefs.SetInt(TutorialKey, state ? 1 : 0);
        }
        
        /// <summary>
        /// Get the tutorial value from PlayerPrefs.
        /// </summary>
        public static bool GetTutorialState()
        {
            if (PlayerPrefs.HasKey(TutorialKey))
            {
                var val = PlayerPrefs.GetInt(TutorialKey);
                if (val < 1)
                    return false;
                else return true;
            }
            else return true;
        }
        
#if UNITY_STANDALONE
        /// <summary>
        /// Save window state to PlayerPrefs.
        /// </summary>
        /// <param name="state"></param>
        public static void SetWindowState(bool state) {
            PlayerPrefs.SetInt(WindowStateKey, state ? 1 : 0);
        }
        
        /// <summary>
        /// Get the window state value from PlayerPrefs.
        /// </summary>
        public static bool GetWindowState()
        {
            var val = PlayerPrefs.GetInt(WindowStateKey);
            if (val < 1)
                return false;
            else return true;
        }
#endif
    }
}
using GameSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// This component applies options from ui elements to the game.
    /// </summary>
    public class OptionApplier : MonoBehaviour
    {
        [Header("UI Elements")] 
        public Dropdown GameTimeMode;
        public Dropdown Difficulty;
        public Slider MusicVolumeSlider;
        public Slider SoundFxVolumeSlider;
        public GameObject GraphicsContainer;
        public Toggle WindowModeToggle;
        public Toggle TutorialToggle;

        [Header("Objects to change")] 
        public AudioMixer AudioMixer;

        private UnityAction<int> gameTimeModeAction;
        private UnityAction<float> musicVolumeAction, soundFxVolumeAction;
        private UnityAction<bool> windowModeAction, tutorialAction;
        private UnityAction<int> difficultyChangedAction;
        
        private void Start()
        {
            gameTimeModeAction = gameTimeModeChanged;
            GameTimeMode.onValueChanged.AddListener(gameTimeModeAction);

            if (Difficulty != null)
            {
                difficultyChangedAction = DifficultyChanged;
                Difficulty.onValueChanged.AddListener(difficultyChangedAction);
            }

            MusicVolumeSlider.value = SettingsManager.GetMusicVolume();
            SoundFxVolumeSlider.value = SettingsManager.GetSoundFxVolume();
            GameTimeMode.value = (int) SettingsManager.GetGameTime();
            
            musicVolumeAction = musicVolumeChanged;
            MusicVolumeSlider.onValueChanged.AddListener(musicVolumeAction);
            
            soundFxVolumeAction = soundFxVolumeChanged;
            SoundFxVolumeSlider.onValueChanged.AddListener(soundFxVolumeAction);

            tutorialAction = tutorialChanged;
            TutorialToggle.onValueChanged.AddListener(tutorialAction);
            TutorialToggle.isOn = SettingsManager.GetTutorialState();
            
            #if UNITY_STANDALONE
            GraphicsContainer.SetActive(true);
            WindowModeToggle.isOn = SettingsManager.GetWindowState();

            windowModeAction = windowModeChanged;
            WindowModeToggle.onValueChanged.AddListener(windowModeAction);
            #endif
        }

        private void OnDestroy()
        {
            GameTimeMode.onValueChanged?.RemoveListener(gameTimeModeAction);
            MusicVolumeSlider.onValueChanged?.RemoveListener(musicVolumeAction);
            SoundFxVolumeSlider.onValueChanged?.RemoveListener(soundFxVolumeAction);
#if UNITY_STANDALONE
            WindowModeToggle.onValueChanged?.RemoveListener(windowModeAction);
#endif
        }

        private void gameTimeModeChanged(int state)
        {
            SettingsManager.SetGameTime(state);
        }

        private void DifficultyChanged(int difficulty) => SettingsManager.SetDifficulty(difficulty);

        private void musicVolumeChanged(float value)
        {
            SettingsManager.SetMusicVolume(value);
            AudioMixer.SetFloat("MusicVol", Mathf.Log(value) * 20);
        }

        private void soundFxVolumeChanged(float value)
        {
            SettingsManager.SetSoundFxVolume(value);
            AudioMixer.SetFloat("FxVol", Mathf.Log(value) * 20);
        }

        private void tutorialChanged(bool state)
        {
            SettingsManager.SetTutorialState(state);
        }
        
#if UNITY_STANDALONE
        private void windowModeChanged(bool state)
        {
            if (state)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
            else
                Screen.fullScreenMode = FullScreenMode.Windowed;
            SettingsManager.SetWindowState(state);
        }
#endif
    }
}
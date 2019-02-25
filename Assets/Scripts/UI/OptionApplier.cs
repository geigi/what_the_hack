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
        public Slider MusicVolumeSlider, SoundFxVolumeSlider;

        [Header("Objects to change")] 
        public AudioMixer AudioMixer;

        private UnityAction<float> musicVolumeAction, soundFxVolumeAction;
        
        private void Start()
        {
            MusicVolumeSlider.value = SettingsManager.GetMusicVolume();
            SoundFxVolumeSlider.value = SettingsManager.GetSoundFxVolume();
            
            musicVolumeAction = musicVolumeChanged;
            MusicVolumeSlider.onValueChanged.AddListener(musicVolumeAction);
            
            soundFxVolumeAction = soundFxVolumeChanged;
            SoundFxVolumeSlider.onValueChanged.AddListener(soundFxVolumeAction);
        }

        private void OnDestroy()
        {
            MusicVolumeSlider.onValueChanged.RemoveListener(musicVolumeAction);
            SoundFxVolumeSlider.onValueChanged.RemoveListener(soundFxVolumeAction);
        }

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
    }
}
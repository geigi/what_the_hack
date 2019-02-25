using GameSystem;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Restores audio volumes from player settings.
/// </summary>
public class RestoreAudioVolumes : MonoBehaviour
{
    public AudioMixer Mixer;
    private void Start()
    {
        Mixer.SetFloat("MusicVol", Mathf.Log(SettingsManager.GetMusicVolume()) * 20);
        Mixer.SetFloat("FxVol", Mathf.Log(SettingsManager.GetSoundFxVolume()) * 20);
    }
}
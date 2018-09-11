using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles all global application settings.
/// </summary>
public class SettingsManager : MonoBehaviour {
	public Dropdown pixelPerfectDropdown;

	public const string PixelPerfectCameraKey = "Pixel_Perfect";
	public enum PixelPerfectCameraValue {
		Off=0,
		Automatic=1,
		ForceOn=2
	}

	public /// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		if (PlayerPrefs.HasKey(PixelPerfectCameraKey)) {
			pixelPerfectDropdown.value = PlayerPrefs.GetInt(PixelPerfectCameraKey);
		}
		else {
			PlayerPrefs.SetInt(PixelPerfectCameraKey, 1);
		}
		
	}

	public void SetPixelPerfectCamera(int dropdownValue) {
		PlayerPrefs.SetInt(PixelPerfectCameraKey, dropdownValue);
	}
}

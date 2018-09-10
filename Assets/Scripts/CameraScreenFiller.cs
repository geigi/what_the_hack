using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraScreenFiller : MonoBehaviour {
	public Camera mainCamera;
	public PixelPerfectCamera pixelPerfectCamera;
	public SpriteRenderer background;

	// Use this for initialization
	void Start () {
		if (pixelPerfectCamera != null) {
			var pixelCameraSetting = PlayerPrefs.GetInt(SettingsManager.PixelPerfectCameraKey);
			if (pixelCameraSetting == (int)SettingsManager.PixelPerfectCameraValue.Off) {
				FillScreen();
			}
			else if (pixelCameraSetting == (int)SettingsManager.PixelPerfectCameraValue.Automatic) {
				if (mainCamera.orthographicSize < 3.3f) {
					FillScreen();
				}
			}
			else if (pixelCameraSetting == (int)SettingsManager.PixelPerfectCameraValue.ForceOn) {
				// Don't do anything, Pixel Perfect Camera is already enabled
			}
		}
	}

	private void FillScreen() {
		if (pixelPerfectCamera != null) {
			pixelPerfectCamera.enabled = false;
		}

		//Fill ultra widescreen devices
		float screenAspect = (float) Screen.width / (float) Screen.height;
		if (screenAspect > 1.7777f) {
			var width = background.sprite.bounds.size.x;
			var height = width / screenAspect;
			mainCamera.orthographicSize = height / 2;
		}
	}
}

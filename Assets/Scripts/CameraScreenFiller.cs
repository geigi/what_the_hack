using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;
using UnityEngine.U2D;

public class CameraScreenFiller : MonoBehaviour {
	public Camera mainCamera;
	public PixelPerfectCamera pixelPerfectCamera;
	public SpriteRenderer background;

	void Start () {
		if (pixelPerfectCamera != null) {
			var pixelCameraSetting = PlayerPrefs.GetInt(SettingsManager.PixelPerfectCameraKey);
			if (pixelCameraSetting == (int)SettingsManager.PixelPerfectCameraValue.Off) {
				FillScreen();
			}
			else if (pixelCameraSetting == (int)SettingsManager.PixelPerfectCameraValue.Automatic) {
				Debug.Log(mainCamera.orthographicSize);
				if (mainCamera.orthographicSize < 3.5f) {
					FillScreen();
					if (mainCamera.orthographicSize < 3.5f) { 
						pixelPerfectCamera.enabled = true;
					}
				}
			}
			else if (pixelCameraSetting == (int)SettingsManager.PixelPerfectCameraValue.ForceOn) {
				if (pixelPerfectCamera != null) {
					pixelPerfectCamera.enabled = true;
				}
			}
		}
	} 

	private void FillScreen() {
		//Fill ultra widescreen devices
		float screenAspect = (float) Screen.width / (float) Screen.height;
		if (screenAspect > 1.7777f) {
			var width = background.sprite.bounds.size.x;
			var height = width / screenAspect;
			mainCamera.orthographicSize = height / 2;
		}
		else {
			var height = background.sprite.bounds.size.y;
			mainCamera.orthographicSize = height / 2;
		}
	}
}

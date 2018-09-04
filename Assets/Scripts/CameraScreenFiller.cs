using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenFiller : MonoBehaviour {
	public Camera mainCamera;
	public SpriteRenderer background;

	// Use this for initialization
	void Start () {
		//Fill ultra widescreen devices
		float screenAspect = (float) Screen.width / (float) Screen.height;
		if (screenAspect > 1.7777f) {
			var width = background.sprite.bounds.size.x;
			var height = width / screenAspect;
			mainCamera.orthographicSize = height / 2;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class AnalogGlitchModulator : MonoBehaviour {
	public AnalogGlitch glitchObject;

	[Range(0.0f, 1.0f)]
	public float disturbedIntensity;
	[Range(0.5f, 5.0f)]
	public float normalLength;
	[Range(0.5f, 5.0f)]
	public float disturbedLength;

	private float lowIntensity;

	private bool run = true;

	// Use this for initialization
	void Start () {
		lowIntensity = glitchObject.scanLineJitter;
		StartCoroutine("Modulate");
	}

	IEnumerator Modulate() {
		while (run) {
			yield return new WaitForSeconds(Random.value * normalLength);
			glitchObject.scanLineJitter = disturbedIntensity;
			yield return new WaitForSeconds(Random.value / 2 * disturbedLength);
			glitchObject.scanLineJitter = lowIntensity;
		}
	}
}

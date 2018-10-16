using System.Collections;
using System.Collections.Generic;
using Android;
using UnityEngine;

public class ModManagerTest : MonoBehaviour
{
	private ModAppManager manager;
	// Use this for initialization
	void Start () {
		Debug.Log("Start ModAppManager");
		manager = new ModAppManager();
		manager.CopyMods();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

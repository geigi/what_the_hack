using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSaveOnLoad : MonoBehaviour
{
	public bool saved = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!saved)
		{
			saved = true;
			var saveGame = new MainSaveGame();
			SaveGameSystem.SaveGame(saveGame, "test1");
		}
	}
}

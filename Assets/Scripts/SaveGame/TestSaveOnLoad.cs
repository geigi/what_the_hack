using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi;
using Wth.ModApi.Tools;

public class TestSaveOnLoad : MonoBehaviour
{
	public bool saved = false;
	public SkillSet SkillSet;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!saved)
		{
			saved = true;
			var saveGame = SaveGameSystem.CreateNewSaveGame("test1");
			saveGame.employeesHired = new List<EmployeeData>();
			var employee = new EmployeeData();
			employee.Skills = SkillSet.keys;
			SaveGameSystem.SaveGame(saveGame);
		}
	}
}

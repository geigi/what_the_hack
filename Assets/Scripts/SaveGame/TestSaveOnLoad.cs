using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi.Employees;
using Wth.ModApi.Skills;

namespace SaveGame
{
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
				employee.Specials = new List<EmployeeSpecial>();
				saveGame.employeesHired.Add(employee);
				SaveGameSystem.SaveGame(saveGame);
				var test = SaveGameSystem.LoadGame("test1");
				Debug.Log(test);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using Employees;
using UnityEngine;

/// <summary>
/// This class is responsible for the main game management tasks like getting the game up and running.
/// </summary>
public class GameManager : MonoBehaviour
{
	// Use this for initialization
	void Init () {
		EmployeeManager.Instance.InitDefaultState();
	}
}

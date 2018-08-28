using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeAI : MonoBehaviour
{
	public EmployeeData employeeData;

	private GameObject employeeGameObject;
	private List<Employee> employees;

	private bool createdEmployee = false;

	void Awake()
	{
		
	}
	
	// Use this for initialization
	void Start ()
	{
		this.employees = new List<Employee>();
		this.employeeGameObject = new GameObject("Employee");
		for (int i = 0; i < 4; i++)
		{
			var gameObject = new GameObject("Employee");
			var employee = gameObject.AddComponent<Employee>();
			employee.init(employeeData);
			employees.Append(employee);
		}
	}

	void WalkEmployee()
	{
		foreach (var employee in employees)
		{
			employee.idleWalking(true);
		}
	}

	
	
	
	
	// Update is called once per frame
	void Update () {
		if (!createdEmployee)
		{
			createdEmployee = true;
			WalkEmployee();
		}
	}
}

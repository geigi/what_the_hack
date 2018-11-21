using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using Wth.ModApi;
using Wth.ModApi.Employees;

public class EmployeeAI : MonoBehaviour
{
    public Material standardEmployeeMaterial;
	public EmployeeDefinition employeeData;

	private GameObject employeeGameObject;
	private List<Employee> employees;

    private EmployeeManager manager;
	private bool createdEmployee = false;
	
	// Use this for initialization
	void Start ()
	{
  this.manager = new EmployeeManager();
		this.employees = new List<Employee>();
		this.employeeGameObject = new GameObject("Employee");
  manager.init(employeeData);
		for (int i = 0; i < 4; i++)
		{
   manager.GenerateEmployeeForHire();
			var gameObject = new GameObject("Employee");
			var employee = gameObject.AddComponent<Employee>();
			employee.init(manager.HireEmployee(), standardEmployeeMaterial);
			employees.Add(employee);
		}
	}

	void WalkEmployee()
	{
		foreach (var employee in employees)
		{
			employee.IdleWalking(true);
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

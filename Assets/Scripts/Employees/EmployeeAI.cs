﻿using System;
using System.Collections;
using System.Collections.Generic;
using Employees;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;

public class EmployeeAI : MonoBehaviour
{
    /// <summary>
    /// All Animation Clips that a male generated Employee can use.
    /// It is Required, that there are the same number of idle, walking and working animations.
    /// The first indices of the array, should hold the idle Animations, then the walking and finally the working Animations. 
    /// </summary>
    public AnimationClip[] maleAnimationClips;
    /// <summary>
    /// All Animation Clips that a female generated Employee can use.
    /// It is Required, that there are the same number of idle, walking and working animations.
    /// The first indices of the array, should hold the idle Animations, then the walking and finally the working Animations. 
    /// </summary>
    public AnimationClip[] femaleAnimationClips;

    public Material standardEmployeeMaterial;

    public GameObject hiredEmpGUIPrefab;
    public GameObject hireableEmpGUIPrefab;
    public GameObject hiredEmpContent;
    public GameObject hireableEmpContent;
    public EmployeeManager manager;

	   private List<Employee> employees;

    private bool createdEmployee = false;
	
	// Use this for initialization
    void Start()
    {
    this.manager = EmployeeManager.Instance;
	    gameObject.transform.parent = this.gameObject.transform;
		this.employees = new List<Employee>();
  manager.init(standardEmployeeMaterial, maleAnimationClips, femaleAnimationClips);
		for (int i = 0; i < 4; i++)
		{
      EmployeeData empData = manager.GenerateEmployeeForHire();
		    GameObject empGUI = Instantiate(hireableEmpGUIPrefab);
		    empGUI.transform.parent = hireableEmpContent.transform;
		    empGUI.transform.localScale = Vector3.one;
      empGUI.GetComponent<HireableEmployeeGUI>().SetEmp(empData, () => hireEmployee(empData, empGUI));
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

    private void hireEmployee(EmployeeData empData, GameObject gui)
    {
        Employee emp = manager.HireEmployee(empData);
        GameObject employeeGUI = Instantiate(hiredEmpGUIPrefab);
        employeeGUI.transform.parent = hiredEmpContent.transform;
        //For whatever Reason the scale is set to 0.6. So we change it back to 1
        employeeGUI.transform.localScale = Vector3.one;
        employeeGUI.GetComponent<HiredEmployeeGUI>().SetEmp(emp, () =>
        {
            employees.Remove(emp);
            manager.FireEmployee(emp.EmployeeData);
            Destroy(emp.gameObject);
            Destroy(employeeGUI);
        });
        employees.Add(emp);
        Destroy(gui);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Employees;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;

public class EmployeeAI : MonoBehaviour
{
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
        gameObject.transform.parent = this.gameObject.transform;
        this.employees = new List<Employee>();
        for (int i = 0; i < 4; i++)
        {
            EmployeeData empData = manager.GenerateEmployeeForHire();
            GameObject empGUI = Instantiate(hireableEmpGUIPrefab);
            empGUI.transform.parent = hireableEmpContent.transform;
            empGUI.transform.localScale = Vector3.one;
            empGUI.GetComponent<HireableEmployeeUiBuilder>().SetEmp(empData, () => hireEmployee(empData, empGUI));
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
        employeeGUI.GetComponent<HiredEmployeeUiBuilder>().SetEmp(emp, () =>
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
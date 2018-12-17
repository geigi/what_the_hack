using System.Collections;
using System.Collections.Generic;
using Employees;
using UnityEngine;

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

    /// <summary>
    /// The global employee manager.
    /// </summary>
    public EmployeeManager employeeManager;

    private GameObject employeeGameObject;
    private List<Employee> employees;
    
    private bool createdEmployee = false;
	
    // Use this for initialization
    void Start ()
    {
        this.employees = new List<Employee>();
        this.employeeGameObject = new GameObject("Employee");
        
        for (int i = 0; i < 4; i++)
        {
            employeeManager.GenerateEmployeeForHire();
            var gameObject = new GameObject("Employee");
            var employee = gameObject.AddComponent<Employee>();
            employee.init(employeeManager.HireEmployee(), standardEmployeeMaterial, 
                maleAnimationClips, femaleAnimationClips);
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
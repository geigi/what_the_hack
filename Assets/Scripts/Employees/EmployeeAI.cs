using System.Collections;
using System.Collections.Generic;
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

    public SkillSet standardSkills;
    public NameLists standardNames;

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
  manager.init(employeeData, standardSkills, standardNames);
		for (int i = 0; i < 4; i++)
		{
   manager.GenerateEmployeeForHire();
			var gameObject = new GameObject("Employee");
			var employee = gameObject.AddComponent<Employee>();
			employee.init(manager.HireEmployee(), standardEmployeeMaterial, 
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

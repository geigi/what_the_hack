using System;
using SaveGame;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;

/// <summary>
/// This class manages all Employees for the game and keeps track, of employees that can be hired,
/// employees which are hired and ex-employees.
/// </summary>
public class EmployeeManager : MonoBehaviour {

    private static readonly Lazy<EmployeeManager> lazy =
        new Lazy<EmployeeManager>(() => new EmployeeManager());

    public static EmployeeManager Instance => lazy.Value;

    /// <summary>
    /// The Number of days should pass, before a specialEmployee can be hired. 
    /// </summary>
    public static int dayThreshold = 10;
    /// <summary>
    /// The Number of days passed.
    /// </summary>
    private int daysPassed;
    /// <summary>
    /// Indicates whether a special can or has already been hired.  
    /// </summary>
    public static bool usedSpecialEmployee = false;

    /// <summary>
    /// List to store all employees that can be hired.
    /// </summary>
    public List<EmployeeData> employeesForHire { get; private set; }
    /// <summary>
    /// List to store all hired Employees.
    /// </summary>
    public List<EmployeeData> hiredEmplyoees { get; private set; }
    /// <summary>
    /// List to store all ex-employees.
    /// </summary>
    public List<EmployeeData> exEmplyoees { get; private set; }

    /// <summary>
    /// EmployeeFactory to generate random EmployeeData.
    /// </summary>
    private EmployeeFactory factory;

    /// <summary>
    /// The special Employee.
    /// Can only be hired once and does not reappear.
    /// </summary>
    private EmployeeDefinition specialEmployee;

    private SkillSet standardSkillSet;
    private NameLists standardNameLists;
    private Material empMaterial;
    private AnimationClip[] maleAnim;
    private AnimationClip[] femaleAnim;

    private EmployeeManager() { }

    /// <summary>
    /// Initializes the EmployeeManager. This Method should be called before using the Manager.
    /// Generates 4 employees for hire.
    /// </summary>
    /// <param name="_specialEmployee">The special employee, which can appear.</param>
    /// <param name="saveGame">Optional Parameter, when the game is loaded from a save game.</param>
    public void init(EmployeeDefinition _specialEmployee, SkillSet standardSkills,
        NameLists standardNames, 
        Material mat, AnimationClip[] male, AnimationClip[] female, 
        MainSaveGame saveGame = null)
    {
        this.empMaterial = mat;
        this.femaleAnim = female;
        this.maleAnim = male;
        this.standardSkillSet  = standardSkills;
        this.standardNameLists = standardNames;
        factory = new EmployeeFactory();
        this.specialEmployee = _specialEmployee;
        this.daysPassed = 0;
        if(saveGame != null)
        {
            this.employeesForHire = saveGame.employeesForHire;
            this.hiredEmplyoees = saveGame.employeesHired;
        } else
        {
            this.employeesForHire = new List<EmployeeData>();
            this.hiredEmplyoees = new List<EmployeeData>();
        }
        this.exEmplyoees = new List<EmployeeData>();
    }

    /// <summary>
    /// Puts a new employee in the employeeForHire List.
    /// </summary>
    public EmployeeData GenerateEmployeeForHire()
    {
        EmployeeData newEmployee = new EmployeeData();
        if(this.daysPassed >= dayThreshold && !usedSpecialEmployee)
        {
            newEmployee.EmployeeDefinition = specialEmployee;
            this.employeesForHire.Add(newEmployee);
        }
        else
        {
            newEmployee = factory.GenerateEmployee(standardSkillSet, standardNameLists, maleAnim.Length / 3);
            this.employeesForHire.Add(newEmployee);
        }

        return newEmployee;
    }

    /// <summary>
    /// Hires the first employee in the employeesForHire List.
    /// </summary>
    /// <returns>EmployeeData of the hired Employee.</returns>
    public Employee HireEmployee()
    {
        var gameObject = new GameObject("Employee");
        var emp = gameObject.AddComponent<Employee>(); 
        EmployeeData empData = employeesForHire[0];
        hiredEmplyoees.Add(empData);
        employeesForHire.Remove(empData);
        emp.init(empData, empMaterial, maleAnim, femaleAnim);
        return emp;
    }

    /// <summary>
    /// Hires the specified employee.
    /// If the employee does not exist in the employeesForHire List this method will do nothing;
    /// </summary>
    /// <param name="empData">The employee to hire.</param>
    public Employee HireEmployee(EmployeeData empData)
    {
        if (!this.employeesForHire.Contains(empData)) return null;
        var gameObject = new GameObject("Employee");
        var emp = gameObject.AddComponent<Employee>();
        hiredEmplyoees.Add(empData);
        employeesForHire.Remove(empData);
        emp.init(empData, empMaterial, maleAnim, femaleAnim);
        return emp;
    }

    /// <summary>
    /// Fires an employee. The employee will be removed from hiredEmployees and placed in exEmployees.  
    /// </summary>
    /// <param name="emp"></param>
    public void FireEmployee(EmployeeData emp)
    {
        if (!hiredEmplyoees.Contains(emp)) return;
        exEmplyoees.Add(emp);
        hiredEmplyoees.Remove(emp);
    }

    /// <summary>
    /// Will be used later as a listener method.
    /// Removes the first Employee from the EmployeeForHire List and creates a new one.
    /// </summary>
    public void newDay()
    {
        this.daysPassed++;
        this.employeesForHire.RemoveAt(0);
        GenerateEmployeeForHire();
    }
}

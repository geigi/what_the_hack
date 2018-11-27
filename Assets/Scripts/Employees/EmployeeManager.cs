using SaveGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi.Employees;

/// <summary>
/// This class manages all Employees for the game and keeps track, of employees that can be hired,
/// employees which are hired and ex-employees.
/// </summary>
public class EmployeeManager {

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

    /// <summary>
    /// Initializes the EmployeeManager. This Method should be called before using the Manager.
    /// Generates 4 employees for hire.
    /// </summary>
    /// <param name="_specialEmployee">The special employee, which can appear.</param>
    /// <param name="saveGame">Optional Parameter, when the game is loaded from a save game.</param>
    public void init(EmployeeDefinition _specialEmployee, SkillSet standardSkills,
        NameLists standardNames, MainSaveGame saveGame = null)
    {
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
    public void GenerateEmployeeForHire()
    {
        EmployeeData newEmployee = new EmployeeData();
        if(this.daysPassed >= dayThreshold && !usedSpecialEmployee)
        {
            newEmployee.EmployeeDefinition = specialEmployee;
            this.employeesForHire.Add(newEmployee);
        }
        else
        {
            this.employeesForHire.Add(factory.GenerateEmployee(standardSkillSet, standardNameLists));
        }
    }

    /// <summary>
    /// Hires the first employee in the employeesForHire List.
    /// </summary>
    /// <returns>EmployeeData of the hired Employee.</returns>
    public EmployeeData HireEmployee()
    {
        EmployeeData emp = employeesForHire[0];
        hiredEmplyoees.Add(emp);
        employeesForHire.Remove(emp);
        return emp;
    }

    /// <summary>
    /// Hires the specified employee.
    /// If the employee does not exist in the employeesForHire List this method will do nothing;
    /// </summary>
    /// <param name="emp">The employee to hire.</param>
    public void HireEmployee(EmployeeData emp)
    {
        if (!this.employeesForHire.Contains(emp)) return;
        hiredEmplyoees.Add(emp);
        employeesForHire.Remove(emp);
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
}

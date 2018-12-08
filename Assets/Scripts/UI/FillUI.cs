using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Internal.Experimental.UIElements;
using UnityEngine.UI;
using Wth.ModApi.Employees;
using Object = UnityEngine.Object;

public class FillUI : MonoBehaviour
{
    public GameObject skillPrefab;
    public GameObject specialPrefab;

    public Image empImage;
    public Text empName;
    public GameObject skillPanel;
    public Text salary;
    public Text salaryTime;
    public Text specialList;

    public Employee emp;

    private List<GameObject> skillUI;
    private List<string> specialNames = new List<string>();

    public void Update()
    {
        empImage.sprite = emp?.GetComponent<SpriteRenderer>().sprite;
        empName.text = emp?.EmployeeData.generatedData.name;
        salary.text = $"{emp?.EmployeeData.Salary}$";
        salaryTime.text = "s week";
        specialList.text = string.Join(",", specialNames);
    }

    public void SetEmp(Employee _emp)
    {
        this.emp = _emp;
        GenerateSkillGui();
        emp.EmployeeData.Specials?.ForEach(special => specialNames.Add(special.GetDisplayName()));
    }

    private void GenerateSkillGui()
    {
        skillUI = new List<GameObject>();
        var skills = emp.EmployeeData.Skills;
        if (skills == null) return;
        foreach (var s in skills)
        {
            GameObject skill = Object.Instantiate(skillPrefab);
            skill.transform.parent = skillPanel.gameObject.transform;
            skill.GetComponent<SkillUi>().skill = s;
            skillUI.Add(skill);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal.Experimental.UIElements;
using UnityEngine.UI;
using Wth.ModApi.Employees;
using Object = UnityEngine.Object;

public abstract class FillUI : MonoBehaviour
{
    public Material standardMat;
    public GameObject skillPrefab;

    public Image empImage;
    public Text empName;
    public GameObject skillPanel;
    public Text salary, salaryTime, specialList;
    public Button DismissButton;

    internal EmployeeData empData;

    private List<GameObject> skillUI;
    private readonly List<string> specialNames = new List<string>();

    public abstract void FillSpecificGUIElements();

    protected virtual void Update()
    {
        if (empData != null)
        {
            empName.text = empData.generatedData.name;
            salary.text = $"{empData.Salary}$";
            salaryTime.text = "a week";
            specialList.text = string.Join(",", specialNames);
            FillSpecificGUIElements();
        }
    }
    
    public virtual void SetEmp(EmployeeData _empData, UnityAction buttonAction)
    {
        this.empData = _empData;
        GenerateSkillGui();
        empImage.material = new EmployeeFactory().GenerateMaterialForEmployee(standardMat, empData.generatedData);
        empData.Specials?.ForEach(special => specialNames.Add(special.GetDisplayName()));
        DismissButton.onClick.AddListener(buttonAction);
    }

    private void GenerateSkillGui()
    {
        skillUI = new List<GameObject>();
        var skills = empData.Skills;
        if (skills == null) return;
        foreach (var s in skills)
        {
            GameObject skill = Object.Instantiate(skillPrefab);
            skill.transform.parent = skillPanel.gameObject.transform;
            skill.GetComponent<SkillUI>().skill = s;
            skillUI.Add(skill);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Employees;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumEmployeesChangedListener : MonoBehaviour
{
    public IntEvent NumChangedEvent;
    public UnityAction<int> evtAction;
    public GameObject managerObject;
    private Text text;
    private EmployeeManager manager;

    internal void Awake()
    {
        manager = managerObject.GetComponent<EmployeeManager>();
        text = GetComponent<Text>();
        evtAction += changeEmpNum;
        NumChangedEvent.AddListener(evtAction);
        changeEmpNum(EmployeeManager.Instance.HiredEmployees);
    }

    private void changeEmpNum(int empNum)
    {
        text.text = $"{empNum} / {EmployeeManager.MaxNumberOfHiredEmployees}";
    }
}

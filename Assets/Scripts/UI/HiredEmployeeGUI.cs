using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HiredEmployeeGUI : FillUI
{
    internal Employee emp;
    public Text employeeState;

    public override void FillSpecificGUIElements()
    {
        //Debug.LogError(emp.GetComponent<SpriteRenderer>().sprite);
        empImage.sprite = emp.GetComponent<SpriteRenderer>().sprite;
        employeeState.text = emp.walking ? "idle" : "Working";
    }

    public void SetEmp(Employee _emp, UnityAction buttonAction)
    {
        this.emp = _emp;
        base.SetEmp(emp.EmployeeData, buttonAction);
    }
}

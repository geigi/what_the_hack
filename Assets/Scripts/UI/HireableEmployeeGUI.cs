using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wth.ModApi.Employees;

public class HireableEmployeeGUI : FillUI
{
    public Sprite[] sprites;
    public Text prize;

    private Sprite sprite;
    public override void FillSpecificGUIElements()
    {
        empImage.sprite = sprite;
        prize.text = $"{empData.Prize} $";
    }

    public override void SetEmp(EmployeeData _empData, UnityAction buttonAction)
    {
        base.SetEmp(_empData, buttonAction);
        sprite = (empData.generatedData.gender == "male")
            ? sprites[empData.generatedData.idleClipIndex]
            : sprites[4 + empData.generatedData.idleClipIndex];
    }
}

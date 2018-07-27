using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmployeeData : ScriptableObject{
    public string name = "Max Mustermann";
    public Sprite sprite = null;
    public int level = 1;
}

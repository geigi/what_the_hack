using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmployeeData : ScriptableObject{
    public string name = "Max Mustermann";
    public Texture2D itemIcon = null;
    public GameObject itemObject = null;
    public int level = 1;
}

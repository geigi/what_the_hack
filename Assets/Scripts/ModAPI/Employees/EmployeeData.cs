using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wth.ModApi {
[System.Serializable]
public class EmployeeData : ScriptableObject{
    public string employeeName = "Max Mustermann";
    public AnimationClip idleAnimation = null;
    public AnimationClip walkingAnimation = null;
    public AnimationClip workingAnimation = null;
    public int level = 1;
}
}
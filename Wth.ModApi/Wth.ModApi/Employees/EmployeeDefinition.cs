using UnityEngine;

namespace Wth.ModApi.Employees {
    [System.Serializable]
    [CreateAssetMenu(fileName = "EmployeeDefinition", menuName = "What_The_Hack ModApi/Employees/Employee Definition", order = 1)]
    public class EmployeeDefinition : ScriptableObject{
        public string EmployeeName = "Max Mustermann";
        
        public int Level = 1;
        
        public AnimationClip IdleAnimation = null;
        public AnimationClip WalkingAnimation = null;
        public AnimationClip WorkingAnimation = null;
    }
}
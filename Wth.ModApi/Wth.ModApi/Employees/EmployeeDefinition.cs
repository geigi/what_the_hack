using UnityEngine;

namespace Wth.ModApi.Employees {
    /// <summary>
    /// This class represents a definition of an Employee. It is used to create special Employees with custom sprites
    /// and specific specials.
    /// It contains no data related to a running game.
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "EmployeeDefinition", menuName = "What_The_Hack ModApi/Employees/Employee Definition", order = 1)]
    public class EmployeeDefinition : ScriptableObject{
        /// <summary>
        /// Name of the employee.
        /// </summary>
        public string EmployeeName = "Max Mustermann";
        
        /// <summary>
        /// Starting level of the employee.
        /// </summary>
        public int Level = 1;
        
        /// <summary>
        /// Idle Animation. This is played when the employee is doing nothing.
        /// </summary>
        public AnimationClip IdleAnimation = null;
        
        /// <summary>
        /// The walking animation.
        /// </summary>
        public AnimationClip WalkingAnimation = null;
        
        /// <summary>
        /// The working animation. This is used when an employee is sitting at a desk.
        /// </summary>
        public AnimationClip WorkingAnimation = null;
    }
}
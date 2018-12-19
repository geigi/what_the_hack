using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace Wth.ModApi.Employees
{
    /// <summary>
    /// Class for holding data of a generatedEmplyoee.
    /// </summary>
    [System.Serializable]
    public class EmployeeGeneratedData
    {
        /// <summary>
        /// The name of this Employee.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The gender of this employee.
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// The hair color of this Employee.
        /// </summary>
        public Color32 hairColor { get; private set; }
        /// <summary>
        /// The skin color of this Employee.
        /// </summary>
        public Color32 skinColor { get; private set; }
        /// <summary>
        /// The eye color of this employee.
        /// </summary>
        public Color32 eyeColor { get; private set; }
        /// <summary>
        /// The shirt color of this Employee. 
        /// </summary>
        public Color32 shirtColor { get; private set; }
        /// <summary>
        /// The shorts color of this Employee.
        /// </summary>
        public Color32 shortsColor { get; private set; }
        /// <summary>
        /// The shoe color of this Employee.
        /// </summary>
        public Color32 shoeColor { get; private set; }

        public int idleClipIndex { get; set; }
        public int walkingClipIndex { get; set; }
        public int workingClipIndex { get; set; }

        public void AssignRandomGender()
        {
            this.gender = (UnityEngine.Random.value > 0.5) ? "female" : "male";
        }

        public void SetColorToPart(Color32 col, EmployeePart part)
        {
            switch (part)
            {
                case EmployeePart.EYES:
                    this.eyeColor = col;
                    break;
                case EmployeePart.HAIR:
                    this.hairColor = col;
                    break;
                case EmployeePart.SHIRT:
                    this.shirtColor = col;
                    break;
                case EmployeePart.SHOES:
                    this.shoeColor = col;
                    break;
                case EmployeePart.SHORTS:
                    this.shortsColor = col;
                    break;
                default:
                    this.skinColor = col;
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeFactory : MonoBehaviour {

    /// <summary>
    /// Generates a Random Skin Color for an Employee. 
    /// There is a 50% Chance this Employee has white or black skin.
    /// </summary>
    /// <returns>The skin color for the Employee</returns>
    public Color GenerateRandomSkinColor()
    {
        float val = Random.value;
        return (val > 0.5) ? Random.ColorHSV(55, 60, 0, 40, 100, 80)
            : Random.ColorHSV(60, 60, 20, 22, 20, 30);
    }

    /// <summary>
    /// Generates a Random Hair Color for this Employee.
    /// There is a 90% Chance it is a Color which is either black, brown or grey and a 10% chance that its
    /// some other Color.
    /// </summary>
    /// <returns>The hair color for this Employee</returns>
    public Color GenerateRandomHairColor()
    {
        float val = Random.value;
        return (val > 0.9) ? Random.ColorHSV(0, 360, 0, 30, 0, 20)
            : Random.ColorHSV(0, 360, 30, 100, 20, 100);
    }

    /// <summary>
    /// Generates a Random ShirtColor for this Employee
    /// The color of the shirt is not restricted by anything.
    /// </summary>
    /// <returns>The shirt color for this Emplyoee.</returns>
    public Color GenerateRandomShirtColor()
    {
        return Random.ColorHSV();
    }

    /// <summary>
    /// Generates a Random Color for the Shorts
    /// There is a 80 Percent Chance, the short color is blue-ish and a 20% Chance that it is something else.
    /// </summary>
    /// <returns>The shorts Color for this Employee</returns>
    public Color GenerateRandomShortsColor()
    {
        float val = Random.value;
        return (val > 0.8) ? Random.ColorHSV(200, 270, 50, 100, 50, 100)
            : Random.ColorHSV();   
    }

    /// <summary>
    /// Generates a random shoe color for this Employee.
    /// The shoe color is not restricted by anyhing.
    /// </summary>
    /// <returns>The shoe color for this Emplyoee.</returns>
    public Color GenerateRandomShoeColor()
    {
        return Random.ColorHSV();
    }
}

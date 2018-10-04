using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeFactory : MonoBehaviour {

    #region Colors

    //Default Colors
    public static Color32 defaultShorts = new Color32(64, 64, 64, 255);
    public static Color32 defaultShoes = new Color32(45, 45, 45, 255);
    public static Color32 defaultShirt = new Color32(113, 113, 113, 255);
    public static Color32 defaultSkin = new Color32(210, 210, 210, 255);
    public static Color32 defaultEyes = new Color32(124, 124, 124, 255);
    public static Color32 defaultHair = new Color32(150, 150, 150, 255);

    //List of possible Colors
    public static Dictionary<Color32, float> shortsColors = new Dictionary<Color32, float> {
        { new Color32(70, 73, 195, 255), 0.25f }, { new Color32(70, 135, 195, 255), 0.25f}, 
        { new Color32(110, 66, 8, 255), 0.25f }, { new Color32(41, 16, 4, 255), 025f }
    };
    public static Dictionary<Color32, float> shoesColors = new Dictionary<Color32, float> {
        { new Color32(68, 8, 73, 255), 0.25f }, { new Color32(62, 28, 9, 255), 0.25f },
        { new Color32(211, 0, 0, 255), 0.25f }, { new Color32(62, 62, 62, 255), 0.25f }
    };
    public static Dictionary<Color32, float> shirtColors = new Dictionary<Color32, float> {
        { new Color32(131, 159, 223, 255), 0.1f }, { new Color32(40, 202, 162, 255), 0.1f },
        { new Color32(158, 202, 40, 255), 0.1f }, { new Color32(116, 40, 202, 255), 0.1f },
        { new Color32(138, 10, 30, 255), 0.1f }, { new Color32(85, 12, 138, 255), 0.1f },
        { new Color32(17, 17, 17, 255), 0.1f }, { new Color32(238, 238, 238, 255), 0.2f },
        { new Color32(255, 0, 238, 255), 0.1f }
    };
    public static Dictionary<Color32, float> skinColors = new Dictionary<Color32, float> {
        { new Color32(233, 195, 140, 255), 1/7f }, { new Color32(223, 202, 131, 255), 1/7f },
        { new Color32(238, 195, 154, 255), 1/7f }, { new Color32(59, 29, 4, 255), 1/7f },
        { new Color32(122, 60, 9, 255), 1/7f }, { new Color32(229, 216, 206, 255), 1/7f },
        { new Color32(173, 141, 110, 255), 1/7f }
    };
    public static Dictionary<Color32, float> eyesColors = new Dictionary<Color32, float> {
        { new Color32(22, 83, 148, 255), 0.2f }, { new Color32(20, 87, 71, 255), 0.2f },
        { new Color32(43, 9, 0, 255), 0.2f }, { new Color32(36, 66, 17, 255), 0.2f },
        { new Color32(255, 255, 255, 255), 0.1f }, {new Color32(212, 11, 11, 255), 0.1f }
    };
    public static Dictionary<Color32, float> hairColors = new Dictionary<Color32, float> {
        { new Color32(233, 223, 62, 255), 1/14f }, { new Color32(212, 11, 11, 255), 1/7f },
        { new Color32(186, 180, 99, 255), 1/7f }, { new Color32(233, 163, 62, 255), 1/14f },
        { new Color32(84, 50, 2, 255), 1/7f }, { new Color32(174, 96, 18, 255), 1/7f },
        { new Color32(0, 0, 0, 255), 1/7f }, { new Color32(7, 172, 186, 255), 1/14f },
        { new Color32(238, 110, 225, 255), 1/14f }
    };

    #endregion

    /// <summary>
    /// Generates a random Color for the specific part.
    /// </summary>
    /// <param name="parts">The part a Color should be generated for</param>
    /// <returns>The generated Color</returns>
    private static Color32 GenerateColor(EmployeeParts parts)
    {
        Dictionary<Color32, float> current = GetCurrentDictionary(parts);
        // Generate a Random weighted Color
        float rand = Random.value;
        float totalWeight = 0;
        Color32 chosenColor = Color.black;
        foreach (KeyValuePair<Color32, float> color in current)
        {
            totalWeight += color.Value;
            if(rand < totalWeight)
            {
                chosenColor = color.Key;
                return chosenColor;
            }
        }
        return chosenColor;
    }

    /// <summary>
    /// Generates a new Material, with random Colors, based on the Material provided as a Parameter.
    /// The Material provided as a parameter is not changed during the process.
    /// </summary>
    /// <param name="standardMaterial">The standard Material, from which a new Material should be generated.</param>
    /// <returns>A new Material with random colors.</returns>
    public static Material GenerateMaterial(Material standardMaterial)
    {
        Material newMat = new Material(standardMaterial);
        newMat.SetColor("_HairGreyColor", defaultHair);
        newMat.SetColor("_HairColor", GenerateColor(EmployeeParts.HAIR));
        newMat.SetColor("_SkinGreyColor", defaultSkin);
        newMat.SetColor("_SkinColor", GenerateColor(EmployeeParts.SKIN));
        newMat.SetColor("_ShirtGreyColor", defaultShirt);
        newMat.SetColor("_ShirtColor", GenerateColor(EmployeeParts.SHIRT));
        newMat.SetColor("_ShortsGreyColor", defaultShorts);
        newMat.SetColor("_ShortsColor", GenerateColor(EmployeeParts.SHORTS));
        newMat.SetColor("_ShoeGreyColor", defaultShoes);
        newMat.SetColor("_ShoeColor", GenerateColor(EmployeeParts.SHOES));
        newMat.SetColor("_EyeGreyColor", defaultEyes);
        newMat.SetColor("_EyeColor", GenerateColor(EmployeeParts.EYES));
        return newMat;
    }

    /// <summary>
    /// Gets the Color Dictioanry for the EmployeePart.
    /// </summary>
    /// <param name="part">The part of the Employee.</param>
    /// <returns>The Color Dictionary</returns>
    private static Dictionary<Color32, float> GetCurrentDictionary(EmployeeParts part)
    {
        if (part == EmployeeParts.HAIR)
            return hairColors;
        else if (part == EmployeeParts.EYES)
            return eyesColors;
        else if (part == EmployeeParts.SHIRT)
            return shirtColors;
        else if (part == EmployeeParts.SHOES)
            return shoesColors;
        else if (part == EmployeeParts.SHORTS)
            return shortsColors;
        else
            return skinColors;
    }
}

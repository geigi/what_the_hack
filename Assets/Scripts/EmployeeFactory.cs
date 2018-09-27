using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeFactory {

    #region Colors

    //Default Colors
    public static Color defaultShorts = new Color(64, 64, 64);
    public static Color defaultShoes = new Color(45, 45, 45);
    public static Color defaultShirt = new Color(113, 113, 113);
    public static Color defualtSkin = new Color(210, 210, 210);
    public static Color defaultEyes = new Color(124, 124, 124);
    public static Color defaultHair = new Color(150, 150, 150);

    //List of possible Colors
    public static Dictionary<Color, float> shortsColors = new Dictionary<Color, float> {
        { new Color(70, 73, 195), 0.25f }, { new Color(70, 135, 195), 0.25f}, 
        { new Color(110, 66, 8), 0.25f }, { new Color(41, 16, 4), 025f }
    };
    public static Dictionary<Color, float> shoesColors = new Dictionary<Color, float> {
        { new Color(68, 8, 73), 0.25f }, { new Color(62, 28, 9), 0.25f },
        { new Color(211, 0, 0), 0.25f }, { new Color(62, 62, 62), 0.25f }
    };
    public static Dictionary<Color, float> shirtColors = new Dictionary<Color, float> {
        { new Color(131, 159, 223), 0.1f }, { new Color(40, 202, 162), 0.1f },
        { new Color(158, 202, 40), 0.1f }, { new Color(116, 40, 202), 0.1f },
        { new Color(138, 10, 30), 0.1f }, { new Color(85, 12, 138), 0.1f },
        { new Color(17, 17, 17), 0.1f }, { new Color(238, 238, 238), 0.1f },
        { new Color(85, 12, 138), 0.1f }, { new Color(255, 0, 238), 0.1f }
    };
    public static Dictionary<Color, float> skinColors = new Dictionary<Color, float> {
        { new Color(233, 195, 140), 1/7 }, { new Color(223, 202, 131), 1/7 },
        { new Color(238, 195, 154), 1/7 }, { new Color(59, 29, 4), 1/7 },
        { new Color(122, 60, 9), 1/7 }, { new Color(229, 216, 206), 1/7 },
        { new Color(173, 141, 110), 1/7 }
    };
    public static Dictionary<Color, float> eyesColors = new Dictionary<Color, float> {
        { new Color(22, 83, 148), 0.2f }, { new Color(20, 87, 71), 0.2f },
        { new Color(43, 9, 0), 0.2f }, { new Color(36, 66, 17), 0.2f },
        { new Color(255, 255, 255), 0.1f }, {new Color(212, 11, 11), 0.1f }
    };
    public static Dictionary<Color, float> hairColors = new Dictionary<Color, float> {
        { new Color(233, 223, 62), 1/14 }, { new Color(212, 11, 11), 1/7 },
        { new Color(186, 180, 99), 1/7 }, { new Color(233, 163, 62), 1/14 },
        { new Color(84, 50, 2), 1/7 }, { new Color(174, 96, 18), 1/7 },
        { new Color(0, 0, 0), 1/7 }, { new Color(7, 172, 186), 1/14 },
        { new Color(238, 110, 225), 1/14 }
    };

    #endregion

    /// <summary>
    /// Generates a random Color for the specific part.
    /// </summary>
    /// <param name="parts">The part a Color should be generated for</param>
    /// <returns>The generated Color</returns>
    public Color GenerateColor(EmployeeParts parts)
    {
        Dictionary<Color, float> current = GetCurrentDictionary(parts);
        // Generate a Random weighted Color
        float rand = Random.value;
        float totalWeight = 0;
        Color chosenColor = Color.black;
        foreach (KeyValuePair<Color, float> color in current)
        {
            totalWeight += color.Value;
            if(rand < totalWeight)
            {
                chosenColor = color.Key;
            }
        }
        return chosenColor;
    }

    private Dictionary<Color, float> GetCurrentDictionary(EmployeeParts part)
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

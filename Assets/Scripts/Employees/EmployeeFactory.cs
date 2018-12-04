using System;
using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi.Employees;
using Wth.ModApi.Names;

/// <summary>
/// Class for generating random EmployeeData and setting the Material for an Employee.
/// </summary>
public class EmployeeFactory {

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

    private ModHolder holder;
    private bool modIsLoaded;

    private static int numberOfBeginningSkills = 2;

    /// <summary>
    /// Generates a random Color for the specific part.
    /// </summary>
    /// <param name="parts">The part a Color should be generated for</param>
    /// <returns>The generated Color</returns>
    private Color32 GenerateColor(EmployeePart parts)
    {
        Dictionary<Color32, float> current = GetCurrentDictionary(parts);
        // Generate a Random weighted Color
        float rand = UnityEngine.Random.value;
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
    /// Sets each grey_color of the material, to the default color, for that part.
    /// </summary>
    /// <param name="mat">The material, where the default colors should be set.</param>
    public void SetDefaultMaterialColors(Material mat)
    {
        mat.SetColor("_HairGreyColor", defaultHair);
        mat.SetColor("_SkinGreyColor", defaultSkin);
        mat.SetColor("_ShirtGreyColor", defaultShirt);
        mat.SetColor("_ShortsGreyColor", defaultShorts);
        mat.SetColor("_ShoeGreyColor", defaultShoes);
        mat.SetColor("_EyeGreyColor", defaultEyes);
    }

    /// <summary>
    /// Generates a new Material, with random Colors, based on the Material provided as a Parameter.
    /// The Material provided as a parameter is not changed during the process.
    /// </summary>
    /// <param name="standardMaterial">The standard Material, from which a new Material should be generated.</param>
    /// <returns>A new Material with random colors.</returns>
    public Material GenerateMaterial(Material standardMaterial)
    {
        var newMat = new Material(standardMaterial);
        SetDefaultMaterialColors(newMat);
        newMat.SetColor("_HairColor", GenerateColor(EmployeePart.HAIR));
        newMat.SetColor("_SkinColor", GenerateColor(EmployeePart.SKIN));
        newMat.SetColor("_ShirtColor", GenerateColor(EmployeePart.SHIRT));
        newMat.SetColor("_ShortsColor", GenerateColor(EmployeePart.SHORTS));
        newMat.SetColor("_ShoeColor", GenerateColor(EmployeePart.SHOES));
        newMat.SetColor("_EyeColor", GenerateColor(EmployeePart.EYES));
        return newMat;
    }

    /// <summary>
    /// Generates a new Material, with the Colors specified in empData.
    /// The Material provided as a parameter is not changed during the process.
    /// </summary>
    /// <param name="standardMaterial">The standard Material, from which a new Material should be generated.</param>
    /// <param name="empData">The generated Data, where the colors are specified.</param>
    /// <returns></returns>
    public Material GenerateMaterialForEmployee(Material standardMaterial, EmployeeGeneratedData empData)
    {
        Material newMat = new Material(standardMaterial);
        SetDefaultMaterialColors(newMat);
        newMat.SetColor("_HairColor", empData.hairColor);
        newMat.SetColor("_SkinColor", empData.skinColor);
        newMat.SetColor("_ShirtColor", empData.shirtColor);
        newMat.SetColor("_ShortsColor", empData.shortsColor);
        newMat.SetColor("_ShoeColor", empData.shoeColor);
        newMat.SetColor("_EyeColor", empData.eyeColor);
        return newMat;
    }

    /// <summary>
    /// Gets the Color Dictioanry for the EmployeePart.
    /// </summary>
    /// <param name="part">The part of the Employee.</param>
    /// <returns>The Color Dictionary</returns>
    private Dictionary<Color32, float> GetCurrentDictionary(EmployeePart part)
    {
        switch (part)
        {
            case EmployeePart.HAIR:
                return hairColors;
            case EmployeePart.EYES:
                return eyesColors;
            case EmployeePart.SHIRT:
                return shirtColors;
            case EmployeePart.SHOES:
                return shoesColors;
            case EmployeePart.SHORTS:
                return shortsColors;
            default:
                return skinColors;
        }
    }

    /// <summary>
    /// Generates new and random EmployeeData.
    /// </summary>
    /// <returns>The generated EmployeeData.</returns>
    public EmployeeData GenerateEmployee(SkillSet skillSet, NameLists names)
    {
        EmployeeData employee = new EmployeeData();
        EmployeeGeneratedData generatedData = new EmployeeGeneratedData();
        //Skills
        List<SkillDefinition> skills = (this.modIsLoaded) ? holder.GetSkills().keys : skillSet.keys;
        List<Skill> skillList = new List<Skill>();
        //2 Random Skills am Anfang
        for (int i = 0; i < numberOfBeginningSkills; i++)
        {
            Skill s = new Skill(skills[new System.Random().Next(skills.Count)]);
            s.AddSkillPoints(new System.Random().Next(150, 400));
            skillList.Add(s);
        }
        employee.Skills = skillList;
        //Color
        var employeeParts = Enum.GetValues(typeof(EmployeePart));
        foreach (EmployeePart part in employeeParts)
        {
            generatedData.SetColorToPart(GenerateColor(part), part);
        }
        //Name
        generatedData.AssignRandomGender();
        NameLists employeeNames = names;
        generatedData.name = (generatedData.gender == "female") ? employeeNames.RandomName(Lists.surNamesFemale) :
            employeeNames.RandomName(Lists.surNamesMale);
        generatedData.name += " " + employeeNames.RandomName(Lists.lastNames);

        employee.generatedData = generatedData;
        return employee;
    }

    /// <summary>
    /// Checks if a mod is loaded.
    /// </summary>
    private bool ModIsLoaded()
    {
        holder = ModHolder.Instance;
        holder.GetLoadedMod();
        return holder.GetModInfo() != null;
    }
}

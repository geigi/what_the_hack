using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameSystem;
using SaveGame;
using UnityEngine;
using Utils;
using Wth.ModApi.Employees;
using Wth.ModApi.Names;
using Random = System.Random;

[assembly: InternalsVisibleTo("Tests")]
/// <summary>
/// Class for generating random EmployeeData and setting the Material for an Employee.
/// </summary>
public class EmployeeFactory {

    #region Colors

    //Default Colors
    
    public enum SwapIndex
    {
        shorts = 64,
        shoes = 45,
        shirt = 113,
        skin = 210,
        eyes = 124,
        hair = 150,
    }

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
        { new Color32(233, 195, 140, 255), 1/6f }, { new Color32(223, 202, 131, 255), 1/6f },
        { new Color32(238, 195, 154, 255), 1/6f }, { new Color32(59, 29, 4, 255), 1/6f },
        { new Color32(122, 60, 9, 255), 1/6f }, { new Color32(173, 141, 110, 255), 1/6f }
    };
    public static Dictionary<Color32, float> eyesColors = new Dictionary<Color32, float> {
        { new Color32(22, 83, 148, 255), 0.3f }, { new Color32(20, 87, 71, 255), 0.3f },
        { new Color32(43, 9, 0, 255), 0.2f }, { new Color32(36, 66, 17, 255), 0.2f }
    };
    public static Dictionary<Color32, float> hairColors = new Dictionary<Color32, float> {
        { new Color32(233, 223, 62, 255), 1/14f }, { new Color32(212, 11, 11, 255), 1/7f },
        { new Color32(186, 180, 99, 255), 1/7f }, { new Color32(233, 163, 62, 255), 1/14f },
        { new Color32(84, 50, 2, 255), 1/7f }, { new Color32(174, 96, 18, 255), 1/7f },
        { new Color32(0, 0, 0, 255), 1/7f }, { new Color32(7, 172, 186, 255), 1/14f },
        { new Color32(238, 110, 225, 255), 1/14f }
    };

    #endregion

    private static int numberOfBeginningSkills = 3;

    private static Random rnd = new Random();

    private ContentHub contentHub;
    private NameLists names;
    private List<SkillDefinition> skills;
    private SkillDefinition allPurpSkillDef;
    private Material empMaterial;

    private Texture2D colorSwapTex;
    private Color[] spriteColors;

    public EmployeeFactory()
    {
        contentHub = ContentHub.Instance;
        names = contentHub.GetNameLists();
        skills = contentHub.GetSkillSet().keys;
        allPurpSkillDef = skills.Find(x => x.skillName.Equals("All Purpose"));
        empMaterial = contentHub.DefaultEmpMaterial;
        InitColorSwapTex();
        spriteColors = new Color[colorSwapTex.width];
    }

    private void InitColorSwapTex()
    {
        colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false)
        {
            filterMode = FilterMode.Point
        };
        for (int i = 0; i < colorSwapTex.width; ++i)
            colorSwapTex.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

        colorSwapTex.Apply();
        empMaterial.SetTexture("_SwapTex", colorSwapTex);
    }

    private void SwapColor(SwapIndex index, Color color)
    {
        spriteColors[(int)index] = color;
        colorSwapTex.SetPixel((int)index, 0, color);
    }

    /// <summary>
    /// Generates a random Color for the specific part.
    /// </summary>
    /// <param name="parts">The part a Color should be generated for</param>
    /// <returns>The generated Color</returns>
    internal Color32 GenerateColor(EmployeePart parts)
    {
        Dictionary<Color32, float> current = GetCurrentDictionary(parts);
        // Generate a Random weighted Color
        float rand = UnityEngine.Random.value;
        float totalWeight = 0;
        Color32 chosenColor = Color.black;
        foreach (var color in current)
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
    /// Generates a new Material, with random Colors.
    /// </summary>
    /// <returns>A new Material with random colors.</returns>
    public Material GenerateMaterial()
    {
        var newMat = new Material(empMaterial);
        SwapColor(SwapIndex.hair, GenerateColor(EmployeePart.HAIR));
        SwapColor(SwapIndex.skin, GenerateColor(EmployeePart.SKIN));
        SwapColor(SwapIndex.shirt, GenerateColor(EmployeePart.SHIRT));
        SwapColor(SwapIndex.shorts, GenerateColor(EmployeePart.SHORTS));
        SwapColor(SwapIndex.shoes, GenerateColor(EmployeePart.SHOES));
        SwapColor(SwapIndex.eyes, GenerateColor(EmployeePart.EYES));
        colorSwapTex.Apply();
        return newMat;
    }

    /// <summary>
    /// Generates a new Material, with the Colors specified in empData.
    /// The Material provided as a parameter is not changed during the process.
    /// </summary>
    /// <param name="standardMaterial">The standard Material, from which a new Material should be generated.</param>
    /// <param name="empData">The generated Data, where the colors are specified.</param>
    /// <returns></returns>
    public Material GenerateMaterialForEmployee( EmployeeGeneratedData empData)
    {
        Material newMat = new Material(empMaterial);
        SwapColor(SwapIndex.skin, empData.skinColor);
        SwapColor(SwapIndex.hair, empData.hairColor);
        SwapColor(SwapIndex.shirt, empData.shirtColor);
        SwapColor(SwapIndex.eyes, empData.eyeColor);
        SwapColor(SwapIndex.shoes, empData.shoeColor);
        SwapColor(SwapIndex.shorts, empData.shortsColor);
        colorSwapTex.Apply();
        return newMat;
    }

    /// <summary>
    /// Gets the Color Dictionary for the EmployeePart.
    /// </summary>
    /// <param name="part">The part of the Employee.</param>
    /// <returns>The Color Dictionary</returns>
    internal Dictionary<Color32, float> GetCurrentDictionary(EmployeePart part)
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
    public virtual EmployeeData GenerateEmployee()
    {
        EmployeeData employee = new EmployeeData();
        EmployeeGeneratedData generatedData = new EmployeeGeneratedData();
        //Skills
        employee.Skills = GenerateSkills();
        //Color
        var employeeParts = Enum.GetValues(typeof(EmployeePart));
        foreach (EmployeePart part in employeeParts)
        {
            generatedData.SetColorToPart(GenerateColor(part), part);
        }
        //Name
        generatedData.AssignRandomGender();
        GenerateName(ref generatedData);

        //AnimationClips
        int numDiffClips = contentHub.maleAnimationClips.Length / 3;
        int clipIndex = rnd.Next(numDiffClips);
        generatedData.idleClipIndex = clipIndex;
        generatedData.walkingClipIndex = clipIndex + numDiffClips;
        generatedData.workingClipIndex = clipIndex + 2 * numDiffClips;

        employee.generatedData = generatedData;
        employee.State = Enums.EmployeeState.PAUSED;
        return employee;
    }

    internal void GenerateName(ref EmployeeGeneratedData generatedData)
    {
        NameLists employeeNames = names;
        generatedData.name = (generatedData.gender == "female") ? employeeNames.PersonName(PersonNames.FemaleFirstName) :
            employeeNames.PersonName(PersonNames.MaleFirstName);
        generatedData.name += " " + employeeNames.PersonName(PersonNames.LastName);
    }

    internal List<Skill> GenerateSkills()
    {
        Skill newSkill = new Skill(allPurpSkillDef);
        newSkill.AddSkillPoints(rnd.Next(100, 1000));
        List<Skill> skillList = new List<Skill> {newSkill};

        for (int i = 1; i < numberOfBeginningSkills; i++)
        {
            Skill s;
            do
            {
                int index = rnd.Next(maxValue: skills.Count);
                s = new Skill(skills[index]);
                s.AddSkillPoints(rnd.Next(100, 1000));
            } while (skillList.Exists(x => x.skillData.skillName.Equals(s.skillData.skillName)));
            skillList.Add(s);
        }

        return skillList;
    } 
}

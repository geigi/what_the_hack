using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Employees;
using Employees.Specials;
using GameSystem;
using GameTime;
using Missions;
using SaveGame;
using Team;
using UE.Common;
using UnityEngine;
using Utils;
using Wth.ModApi.Employees;
using Wth.ModApi.Names;
using Wth.ModApi.Tools;
using Random = System.Random;

[assembly: InternalsVisibleTo("Tests")]
/// <summary>
/// Class for generating random EmployeeData and setting the Material for an Employee.
/// </summary>
public class EmployeeFactory {

    public static readonly List<Type> EmployeeSpecials = new List<Type>
    {
        typeof(FastLearner),
        typeof(LuckyDevil),
        typeof(Risky),
        typeof(Unreliable)
    };
    
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

    protected internal static Random rnd = new Random();

    private const float SalaryInvariance = 1.1f;

    private const float PrizeInvariance = 1.1f;
    
    protected internal ContentHub contentHub;
    protected internal TeamManager teamManager;
    protected internal MissionManager missionManager;
    protected internal GameTime.GameTime gameTime;
    protected internal EmployeeManager employeeManager;
    protected internal NameLists names;
    private List<SkillDefinition> skills;
    private SkillDefinition allPurpSkillDef;
    private Material empMaterial, empUiMaterial;

    private Texture2D colorSwapTex;
    private Color[] spriteColors;

    protected internal List<EmployeeDefinition> specialEmployeesToSpawn;

    public EmployeeFactory()
    {
        contentHub = ContentHub.Instance;
        teamManager = TeamManager.Instance;
        missionManager = MissionManager.Instance;
        gameTime = GameTime.GameTime.Instance;
        employeeManager = EmployeeManager.Instance;
        names = contentHub.GetNameLists();
        skills = contentHub.GetSkillSet().keys;
        allPurpSkillDef = skills.Find(x => x.skillName.Equals("All Purpose"));
        empMaterial = contentHub.DefaultEmpMaterial;
        empUiMaterial = contentHub.DefaultEmpUiMaterial;
        InitColorSwapTex();
        spriteColors = new Color[colorSwapTex.width];
        specialEmployeesToSpawn = new List<EmployeeDefinition>();
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
        empUiMaterial.SetTexture("_SwapTex", colorSwapTex);
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
    /// <param name="empData">The generated Data, where the colors are specified.</param>
    /// <param name="ui"></param>
    /// <returns></returns>
    public Material GenerateMaterialForEmployee(EmployeeGeneratedData empData, bool ui = false)
    {
        Material newMat;
        newMat = ui ? new Material(empUiMaterial) : new Material(empMaterial);
        
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
    /// Adds all SpecialEmployees to the special EmployeesToSpawn List, iff all of their conditions are met and their are currently not
    /// hireable or hired. If they are an ExEmployee they must also have the recurring trade to be put in the List.
    /// </summary>
    public virtual void AddSpecialEmployees() =>
        specialEmployeesToSpawn.AddRange(contentHub.GetEmployeeLists().employeeList.FindAll(empDef =>
            ConditionsMet(empDef) && !(employeeManager.GetData().employeesForHire
                                           .Any(empData => empData.EmployeeDefinition == empDef) ||
                                       employeeManager.GetData().hiredEmployees 
                                           .Any(empData => empData.EmployeeDefinition == empDef) ||
                                       employeeManager.GetData().exEmplyoees
                                           .Any(empData => empData.EmployeeDefinition == empDef) && !empDef.Recurring)));
    

    /// <summary>
    /// Checks if all the Conditions for an EmployeeDefinition is Met.
    /// </summary>
    /// <param name="empDef">The EmployeeDefinition to check</param>
    /// <returns>True iff all Conditions for this EmployeeDefinition are met</returns>
    protected internal virtual bool ConditionsMet(EmployeeDefinition empDef)
    {
        if (!empDef.SpawnWhenAllConditionsAreMet) return true;
        if (empDef.MissionSucceeded.Any(mission => !missionManager.GetData().Completed
            .Any(completedMission => completedMission.Definition = mission)))
                return false;

        if (empDef.GameProgress > teamManager.calcGameProgress()) return false;
        return !((gameTime.GetData().Date.GetDateTime() - new DateTime(1, 1, 1)).TotalDays <
                 empDef.NumberOfDaysTillEmpCanSpawn);
    }
    
    /// <summary>
    /// Returns either a SpecialEmployee, if one can be spawned, or null if no Special Employee can be spwaned
    /// </summary>
    /// <returns>A Special Employee, or null</returns>
    public virtual EmployeeData SpecialEmployee()
    {
        AddSpecialEmployees();
        for (var index = specialEmployeesToSpawn.Count - 1; index >= 0; index--)
        {
            var data = specialEmployeesToSpawn[index];
            if (rnd.NextDouble() < data.SpawnLikelihood)
            {
                specialEmployeesToSpawn.Remove(data);
                return GenerateSpecialEmployee(data);
            }
        }
        return null;
    }

    /// <summary>
    /// Generates a Special Employee, by generating Skills Special, etc. for the Employee.
    /// </summary>
    /// <param name="empDef">The EmployeeDefinition this Employee is built upon.</param>
    /// <returns>Employee Data for the EmployeeDefinition</returns>
    public virtual EmployeeData GenerateSpecialEmployee(EmployeeDefinition empDef)
    {
        EmployeeData employee = new EmployeeData
        {
            EmployeeDefinition = empDef,
            Skills = GenerateSkills(),
            hireableDays = empDef.SpawnLikelihood == 1 ? -1 : rnd.Next(3, 7)
        };
        LevelUpSkills(employee.Skills);
        employee.Salary = calcSalary(employee);
        employee.Prize = calcPrize(employee);
        return employee;
    }

    /// <summary>
    /// Gets a new Employee. The Employee returned can either be special or generated
    /// </summary>
    /// <returns>The new EmployeeData</returns>
    public virtual EmployeeData GetNewEmployee() => SpecialEmployee() ?? GenerateRandomEmployee();
    

    /// <summary>
    /// Generates new and random EmployeeData.
    /// </summary>
    /// <returns>The generated EmployeeData.</returns>
    public virtual EmployeeData GenerateRandomEmployee()
    {
        EmployeeData employee = new EmployeeData();
        EmployeeGeneratedData generatedData = new EmployeeGeneratedData();
        //Skills
        employee.Skills = GenerateSkills();
        LevelUpSkills(employee.Skills);
        if (true)
        {
            EmployeeSpecial special;
            do
            {
                special = (EmployeeSpecial) Activator.CreateInstance(EmployeeSpecials.RandomElement());
            } while (!special.IsLearnable() || employee.GetSpecials().Any(e => e.GetType() == special.GetType()));

            employee.AddSpecial(special);
        }
        //Color
        var employeeParts = Enum.GetValues(typeof(EmployeePart));
        foreach (EmployeePart part in employeeParts)
        {
            generatedData.SetColorToPart(GenerateColor(part), part);
        }
        //Name
        generatedData.AssignRandomGender();
        GenerateName(ref generatedData);

        //Set Salary and Prize
        employee.Salary = calcSalary(employee);
        employee.Prize = calcPrize(employee);

        //hireableDays
        employee.hireableDays = rnd.Next(1, 4);

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

    internal virtual void GenerateName(ref EmployeeGeneratedData generatedData)
    {
        NameLists employeeNames = names;
        generatedData.name = (generatedData.gender == "female") ? employeeNames.PersonName(PersonNames.FemaleFirstName) :
            employeeNames.PersonName(PersonNames.MaleFirstName);
        generatedData.name += " " + employeeNames.PersonName(PersonNames.LastName);
    }

    internal virtual List<Skill> GenerateSkills()
    {
        Skill newSkill = new Skill(allPurpSkillDef);
        List<Skill> skillList = new List<Skill> {newSkill};

        for (int i = 1; i < numberOfBeginningSkills; i++)
        {
            Skill s;
            do
            {
                int index = rnd.Next(maxValue: skills.Count);
                s = new Skill(skills[index]);
            } while (skillList.Exists(x => x.SkillData.skillName.Equals(s.SkillData.skillName)));
            skillList.Add(s);
        }

        return skillList;
    }

    /// <summary>
    /// Level up the skills of a freshman depending on the game progress
    /// </summary>
    internal virtual void LevelUpSkills(List<Skill> skills)
    {
        foreach (var s in skills)
        {
            for (int i = 0; i < teamManager.GetRandomSkillValue(skills.Count); i++)
            {
                s.LevelUp();
            }
        }
    }

    private const int basicSalary = 100;
    private static int adaptedSalary = 100;
    private static float MaxInvariance = 1.5f;
    private static int SkillLevelValue = 10;
    private static int SpecialValue = 50;

    /// <summary>
    /// Calculates the Salary of an Employee.
    /// </summary>
    /// <returns>The salary of an Employee</returns>
    internal virtual int calcSalary(EmployeeData empData)
    {
        AdjustSalaryValues();
        var salary = (int) Mathf.Abs(
            ((adaptedSalary + SkillLevelValue * CalculateSkillScore(empData) +
              SpecialValue * empData.GetSpecials().Count) *
             Mathf.Max(Convert.ToSingle(rnd.NextDouble() + 1), 1.5f)));
        
        foreach (var special in empData.GetSpecials())
        {
            salary = (int) (salary * special.GetSalaryRelativeFactor()) + special.GetSalaryAbsoluteBonus();
        }
        
        return salary;
    }

    /// <summary>
    /// Calculates the current Game Score
    /// </summary>
    /// <returns>The Game Score</returns>
    private void AdjustSalaryValues()
    {
       float progress =  Math.Max(TeamManager.Instance.calcGameProgress(), 1f);
       adaptedSalary = (int) (basicSalary * Math.Max((int) progress * 0.05f, 1f));
       if (rnd.NextDouble() < 0.5) SkillLevelValue++;
       else SpecialValue++;
    }

    /// <summary>
    /// Calculates the prize of the employee.
    /// </summary>
    /// <param name="empData">The employee for which the prize should be calculated</param>
    /// <returns>Prize for the employee</returns>
    internal virtual int calcPrize(EmployeeData empData)
    {
        var price = (int) Mathf.Abs(empData.Salary * Mathf.Max(Convert.ToSingle(rnd.NextDouble() + 1), 1.2f));
        
        foreach (var special in empData.GetSpecials())
        {
            price = (int) (price * special.GetHiringCostRelativeFactor()) + special.GetHiringCostAbsoluteBonus();
        }
        
        return price;
    }

    /// <summary>
    /// The SkillScore is the Sum of the levels of all Skills of an Employee.
    /// </summary>
    /// <param name="empData">The Employee of which the SkillScore should be calculated</param>
    /// <returns>The skill score</returns>
    private int CalculateSkillScore(EmployeeData empData) => empData.Skills.Sum(skill => skill.Level);
}

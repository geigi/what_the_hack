using System;
using System.Linq;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class for skill objects.
/// Skills are used to define the specialization of an Employee.
/// </summary>
[Serializable]
public class Skill
{
    /// <summary>
    /// This property is necessary because no constructor is being called on deserialization.
    /// </summary>
    [NonSerialized]
    private UnityEvent skillEvent;

    public UnityEvent SkillEvent
    {
        get => skillEvent ?? (skillEvent = new UnityEvent());
        private set => skillEvent = value;
    }

    /// <summary>
    /// enum that represents employees aptitude to do a certain thing well.
    /// The integer value for the constants corresponds to the  minimum level required to get the name.
    /// </summary>
    public enum LevelAptitudeName
    {
        Newbie = 0,
        Greenhorn = 4,
        Beginner = 6,
        Intermediate = 8,
        Experienced = 10,
        Professional = 12,
        Master = 14,
        GrandMaster = 16,
        Wizard = 18,
        God = 10,
        ChuckNorris = 22
    }

    /// <summary>
    /// This base is powed with the current level of this skill.
    /// </summary>
    protected internal static float levelFactor = 1.5f;

    /// <summary>
    /// Instance to store the skillData.
    /// </summary>
    public SkillDefinition SkillData { get; private set; }

    /// <summary>
    /// The Level of this skill.
    /// </summary>
    public int Level
    {
        get => level;
        private set => level = value;
    }
    private int level;

    /// <summary>
    /// The level aptitude name
    /// </summary>
    public LevelAptitudeName SkillLevelName { get; private set; } = LevelAptitudeName.Newbie;

    /// <summary>
    /// How much the next level costs in skill points.
    /// </summary>
    public int LevelUpCost => (int) (SkillData.LevelUpCost + SkillData.LevelUpCostFactor * level);

    public Skill()
    {
    }
    
    ///<summary>
    ///Constructor for this skill.
    ///</summary>
    ///<param name="data">The data for this skill.</param>
    public Skill(SkillDefinition data)
    {
        this.Level = 1;
        this.SkillData = data;
    }

    /// <summary>
    /// Cause a level up of this skill.
    /// </summary>
    public void LevelUp()
    {
        Level++;
        SkillLevelName = UpdateLevelAptitudeName();
        Debug.Log("Skill leveled up: " + GetName());
        SkillEvent.Invoke();
    }

    /// <summary>
    /// Get the name of this skill.
    /// This function is only left in for convenience.
    /// </summary>
    /// <returns>The name of this skill</returns>
    public string GetName() => SkillData.skillName;

    /// <summary>
    /// Get the sprite of this skill.
    /// This function is only left in for convenience.
    /// </summary>
    /// <returns>The Sprite of this Skill</returns>
    public Sprite GetSprite() => SkillData.skillSprite;

    /// <summary>
    /// Returns the corresponding aptitude name for the current level.
    /// </summary>
    /// <returns>The aptitude name.</returns>
    private LevelAptitudeName UpdateLevelAptitudeName()
    {
        System.Collections.IList list = Enum.GetValues(typeof (LevelAptitudeName));
        for (int i = list.Count - 1; i >= 1; i--)
        {
            var aptitude = list[i];
            if (Level >= (int) aptitude) return (LevelAptitudeName) list[i];
        }
        return LevelAptitudeName.Newbie;
    }
}

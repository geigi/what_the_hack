using System;
using System.Linq;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class for skill objects.
/// Skills are used to define the specialization of an Employee.
/// </summary>
public class Skill
{

    public UnityEvent skillEvent { get; } = new UnityEvent();

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
    /// A factor by which nextLevelPoints is multiplied each time, this skill levels up.
    /// </summary>
    private static float levelFactor = 1.9f;

    /// <summary>
    /// Instance to store the skillData.
    /// </summary>
    public SkillDefinition skillData { get; private set; }
    
    /// <summary>
    /// The points of this skill.
    /// </summary>
    public float points { get; private set; }

    /// <summary>
    /// The Level of this skill.
    /// </summary>
    public int level { get; private set; }

    /// <summary>
    /// The level aptitude name
    /// </summary>
    public LevelAptitudeName skillLevelName { get; private set; } = LevelAptitudeName.Newbie;

    /// <summary>
    /// The number of points needed to advance a Level.
    /// </summary>
    private float nextLevelPoints;

    ///<summary>
    ///Constructor for this skill.
    ///</summary>
    ///<param name="data">The data for this skill.</param>
    public Skill(SkillDefinition data)
    {
        this.points = 0;
        this.level = 0;
        this.nextLevelPoints = 100;
        this.skillData = data;
    }

    /// <summary>
    /// Adds skillLevel points to the points of this skill and advances a Level if this skill holds enough points.
    /// </summary>
    /// <param name="skillPoints">The number of points added to the points of this skill.</param>
    public void AddSkillPoints(float skillPoints)
    {
        points += skillPoints;
        while(points >= nextLevelPoints)
        {
            level++;
            nextLevelPoints = 100 * level * levelFactor;
            skillLevelName = UpdateLevelAptitudeName();
        }
        skillEvent.Invoke();
    }

    /// <summary>
    /// Get the name of this skill.
    /// This function is only left in for convenience.
    /// </summary>
    /// <returns>The name of this skill</returns>
    public string GetName() => skillData.skillName;

    /// <summary>
    /// Get the sprite of this skill.
    /// This function is only left in for convenience.
    /// </summary>
    /// <returns>The Sprite of this Skill</returns>
    public Sprite GetSprite() => skillData.skillSprite;

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
            if (level >= (int) aptitude) return (LevelAptitudeName) list[i];
        }
        return LevelAptitudeName.Newbie;
    }
}

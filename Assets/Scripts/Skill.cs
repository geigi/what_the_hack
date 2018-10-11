using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi;
using Wth.ModApi.Skills;

/// <summary>
/// A class for skill objects.
/// Skills are used to define the specialization of an Employee.
/// </summary>
public class Skill {

    /// <summary>
    /// A factor by which nextLevelPoints is multiplied each time, this skill levels up.
    /// </summary>
    private static float levelFactor = 1.1f;

    /// <summary>
    /// Instance to store the skillData.
    /// </summary>
    public SkillDefinition skillData { get; set; }
    
    /// <summary>
    /// The points of this skill.
    /// </summary>
    public float points { get; set; }

    /// <summary>
    /// The Level of this skill.
    /// </summary>
    public int level { get; set; }

    /// <summary>
    /// The number of points needed to advance a Level.
    /// </summary>
    private float nextLevelPoints;

    ///<summary>
    ///Needs to be called before the skill is used.
    ///</summary>
    ///<param name="data">The data for this skill.</param>
    public void Init(SkillDefinition data)
    {
        this.points = 0;
        this.level = 0;
        this.nextLevelPoints = 100;
        this.skillData = data;
    }

    /// <summary>
    /// Adds skillLevelPunkte to the points of this skill and advances a Level if this skill holds enough points.
    /// </summary>
    /// <param name="skillPoints">The number of points added to the points of this skill.</param>
    public void AddSkillPoints(float skillPoints)
    {
        points += skillPoints;
        while(points >= nextLevelPoints)
        {
            level++;
            nextLevelPoints *= level * levelFactor; 
        }
    }

    /// <summary>
    /// Get the name of this skill.
    /// This function is only left in for convenience.
    /// </summary>
    /// <returns>The name of this skill</returns>
    public string GetName()
    {
        return skillData.skillName;
    }

    /// <summary>
    /// Get the sprite of this skill.
    /// This function is only left in for convenience.
    /// </summary>
    /// <returns>The Sprite of this Skill</returns>
    public Sprite GetSprite()
    {
        return skillData.skillSprite;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

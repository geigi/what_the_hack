using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi;

public class Skill : MonoBehaviour {

    //A factor by which nextLevelPoints is multiplied each time, this skill levels up. 
    private static float levelFactor = 1.1f;

    //Instance to store the skillDate
    public SkillDefinition skillData { get; set; }

    //the Points of this skill.
    public float points { get; set; }

    //The Level of this skill.
    public int level { get; set; }

    //The number of points needed to advance a Level.
    private float nextLevelPoints;

    ///<summary>
    ///Needs to be called before the Employee is used
    ///</summary>
    ///<param name="data">The data for this skill</param>
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
    /// <param name="skillLevelPunkte">The number of points added to the points of this skill</param>
    public void AdjustPunkteZahl(float skillLevelPunkte)
    {
        points += skillLevelPunkte;
        while(points >= nextLevelPoints)
        {
            level++;
            nextLevelPoints *= level * levelFactor; 
        }
    }

    /// <summary>
    /// Get the name of this Skill
    /// This function is only left in for convenience.
    /// </summary>
    /// <returns>The name of this skill</returns>
    public string GetName()
    {
        return skillData.skillName;
    }

    /// <summary>
    /// Get the sprite of this Skill
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

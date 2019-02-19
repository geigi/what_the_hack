using UnityEngine;

/// <summary>
/// A Class for a Skill Scriptable Object.
/// </summary>
[CreateAssetMenu(fileName = "Skill", menuName = "What_The_Hack ModApi/Skills/Skill Definition", order = -700)]
public class SkillDefinition : ScriptableObject
{
    /// <summary>
    /// The skills name.
    /// </summary>
    public string skillName;

    /// <summary>
    /// The skills image / sprite.
    /// </summary>
    public Sprite skillSprite;

    /// <summary>
    /// The amount of skill points this skill requires for an level up.
    /// </summary>
    public int LevelUpCost = 1;

    /// <summary>
    /// The factor that defines how much each level up will cost more.
    /// </summary>
    public float LevelUpCostFactor = 0.0f;
}
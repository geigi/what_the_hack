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
}
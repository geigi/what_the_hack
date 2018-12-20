using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds the UI to display a single Skill
/// </summary>
public class SkillUIBuilder : MonoBehaviour
{
    /// <summary>
    /// Displays the image of this skill.
    /// </summary>
    public Image skillImage;
    /// <summary>
    /// Displays the name of the skill.
    /// </summary>
    public Text skillName;
    /// <summary>
    /// Displays the current level of the skill
    /// </summary>
    public Text skillLevel;
    /// <summary>
    /// Skill object for which the UI is build.
    /// </summary>
    public Skill skill;

    /// <summary>
    /// Called once per frame.
    /// Builds and draws the UI.
    /// </summary>
    public void Update()
    {
        if (skill != null)
        {
            skillImage.sprite = skill.GetSprite();
            skillName.text = skill.GetName();
            skillLevel.text = $"{skill.skillLevelName} {skill.level}";
        }
    }
}

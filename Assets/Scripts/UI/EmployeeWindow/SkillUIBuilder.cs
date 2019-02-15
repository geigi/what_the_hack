using System.Collections;
using System.Collections.Generic;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
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
    /// Displays the current level of the skill
    /// </summary>
    public Text skillLevelName;
    /// <summary>
    /// Skill object for which the UI is build.
    /// </summary>
    public Skill skill;

    public UnityEvent skillEvent;

    public void SetSkill(Skill _skill)
    {
        this.skill = _skill;
        skillImage.sprite = skill.GetSprite();
        skillName.text = skill.GetName();
        UpdateSkillUi();
        skillEvent = _skill.SkillEvent;
        skillEvent.AddListener(UpdateSkillUi);
    }

    private void UpdateSkillUi()
    {
        skillLevelName.text = $"{skill.SkillLevelName}";
        skillLevel.text = $"{skill.Level}";
    }

    private void OnDestroy()
    {
        skillEvent.RemoveListener(UpdateSkillUi);
    }
}

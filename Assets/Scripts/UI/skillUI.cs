using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{

    public Image skillImage;
    public Text skillName;
    public Text skillLevel;
    public Skill skill;

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

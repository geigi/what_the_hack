using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Base;
using SaveGame;
using UE.Events;
using UnityEngine;

/// <summary>
/// This class is the hub where other classes can get the game content.
/// If a mod is loaded it returns the mods content, otherwise the default content.
/// </summary>
public sealed class ContentHub: Singleton<ContentHub>
{
    /// <summary>
    /// The <see cref="SkillSet"/> of the base game.
    /// </summary>
    public SkillSet DefaultSkillSet;
    /// <summary>
    /// The <see cref="NameLists"/> of the base game.
    /// </summary>
    public NameLists DefaultNameLists;
    /// <summary>
    /// The <see cref="EmployeeList"/> (special employees) of the base game.
    /// </summary>
    public EmployeeList DefaultSpecialEmployees;
    /// <summary>
    /// The <see cref="MissionList"/> of the base game.
    /// </summary>
    public MissionList DefaultMissionList;
    /// <summary>
    /// The default Material for all Employees.
    /// </summary>
    public Material DefaultEmpMaterial;

    /// <summary>
    /// All Animation Clips that a male generated Employee can use.
    /// It is Required, that there are the same number of idle, walking and working animations.
    /// The first indices of the array, should hold the idle Animations, then the walking and finally the working Animations. 
    /// </summary>
    public AnimationClip[] maleAnimationClips;
    /// <summary>
    /// All Animation Clips that a female generated Employee can use.
    /// It is Required, that there are the same number of idle, walking and working animations.
    /// The first indices of the array, should hold the idle Animations, then the walking and finally the working Animations. 
    /// </summary>
    public AnimationClip[] femaleAnimationClips;

    /// <summary>
    /// Animator controller for employee animations.
    /// </summary>
    public RuntimeAnimatorController EmployeeAnimations;

    /// <summary>
    /// The game wide saveGameSystem instance.
    /// </summary>
    public SaveGameSystem SaveGameSystem;

    /// <summary>
    /// The game wide bank instance.
    /// </summary>
    public Bank bank;

    /// <summary>
    /// This event will be raised when a tile gets blocked by furniture.
    /// </summary>
    public Vector2Event TileBlockedEvent;
    
    /// <summary>
    /// Get the current <see cref="SkillSet"/>. 
    /// </summary>
    /// <returns>The base <see cref="SkillSet"/> if no mod is present, otherwise the one from the mod.</returns>
    public SkillSet GetSkillSet()
    {
        var modSkills = ModHolder.Instance.GetSkills();
        if (modSkills != null)
        {
            return modSkills;
        }
        else
        {
            return DefaultSkillSet;
        }
    }

    /// <summary>
    /// Get the current <see cref="NameLists"/>. 
    /// </summary>
    /// <returns>The base <see cref="NameLists"/> if no mod is present, otherwise the one from the mod.</returns>
    public NameLists GetNameLists() {
        var modNames = ModHolder.Instance.GetNameLists();
        if (modNames != null)
        {
            return modNames;
        }
        else
        {
            return DefaultNameLists;
        }
    }
                
    /// <summary>
    /// Get the current <see cref="EmployeeList"/>. 
    /// </summary>
    /// <returns>The base <see cref="EmployeeList"/> if no mod is present, otherwise the one from the mod.</returns>
    public EmployeeList GetEmployeeLists() {
        var modEmployees = ModHolder.Instance.GetEmployees();
        if (modEmployees != null)
        {
            return modEmployees;
        }
        else
        {
            return DefaultSpecialEmployees;
        }
    }

    /// <summary>
    /// Get the current <see cref="MissionList"/>. 
    /// </summary>
    /// <returns>The base <see cref="MissionList"/> if no mod is present, otherwise the one from the mod.</returns>
    public MissionList GetMissionList() {
        var modMissions = ModHolder.Instance.GetMissionList();
        if (modMissions != null)
        {
            return modMissions;
        }
        else
        {
            return DefaultMissionList;
        }
    }
}
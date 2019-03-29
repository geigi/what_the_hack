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
public class ContentHub: Singleton<ContentHub>
{
    /// <summary>
    /// The <see cref="NameLists"/> of the base game.
    /// </summary>
    [Header("Default Game Data")]
    public NameLists DefaultNameLists;
    /// <summary>
    /// The <see cref="SkillSet"/> of the base game.
    /// </summary>
    public SkillSet DefaultSkillSet;
    /// <summary>
    /// The <see cref="EmployeeList"/> (special employees) of the base game.
    /// </summary>
    public EmployeeList DefaultSpecialEmployees;
    /// <summary>
    /// The general purpose skill definition.
    /// </summary>
    public SkillDefinition GeneralPurposeSkill;
    /// <summary>
    /// The <see cref="MissionList"/> of the base game.
    /// </summary>
    public MissionList DefaultMissionList;
    
    /// <summary>
    /// The default Material for all generated Employees.
    /// </summary>
    [Header("Employees")]
    public Material DefaultEmpMaterial;
    /// <summary>
    /// The default Material for special employees.
    /// </summary>
    public Material DefaultSpecialEmpMaterial;
    /// <summary>
    /// The default material for employee UI images.
    /// </summary>
    public Material DefaultEmpUiMaterial;
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
    [Header("Singletons")]
    public SaveGameSystem SaveGameSystem;

    /// <summary>
    /// The game wide bank instance.
    /// </summary>
    public Bank bank;

    public Sprite InfoNotificationSprite;
    public Sprite WarningNotificationSprite;
    public Sprite FailNotificationSprite;
    public Sprite SuccessNotificationSprite;
    
    #region Events
    /// <summary>
    /// This event will be raised when a tile gets blocked by furniture.
    /// </summary>
    public Vector2Event TileBlockedEvent;

    /// <summary>
    /// This is the global event when a game step is happening.
    /// </summary>
    public IntEvent GameStepEvent;
    #endregion

    private NameLists cachedCombinedList;
    
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
            if (modNames.UseExclusively)
                return modNames;
            else
            {
                if (cachedCombinedList == null)
                {
                    cachedCombinedList = ScriptableObject.CreateInstance<NameLists>();
                    cachedCombinedList.firstNamesMale.AddRange(DefaultNameLists.firstNamesMale);
                    cachedCombinedList.firstNamesMale.AddRange(modNames.firstNamesMale);
                    
                    cachedCombinedList.firstNamesFemale.AddRange(DefaultNameLists.firstNamesFemale);
                    cachedCombinedList.firstNamesFemale.AddRange(modNames.firstNamesFemale);
                    
                    cachedCombinedList.lastNames.AddRange(DefaultNameLists.lastNames);
                    cachedCombinedList.lastNames.AddRange(modNames.lastNames);
                    
                    cachedCombinedList.companyNames.AddRange(DefaultNameLists.companyNames);
                    cachedCombinedList.companyNames.AddRange(modNames.companyNames);
                    
                    cachedCombinedList.passwordApplications.AddRange(DefaultNameLists.passwordApplications);
                    cachedCombinedList.passwordApplications.AddRange(modNames.passwordApplications);
                    
                    cachedCombinedList.universities.AddRange(DefaultNameLists.universities);
                    cachedCombinedList.universities.AddRange(modNames.universities);
                    
                    cachedCombinedList.webServices.AddRange(DefaultNameLists.webServices);
                    cachedCombinedList.webServices.AddRange(modNames.webServices);
                    
                    cachedCombinedList.software.AddRange(DefaultNameLists.software);
                    cachedCombinedList.software.AddRange(modNames.software);
                    
                    cachedCombinedList.towns.AddRange(DefaultNameLists.towns);
                    cachedCombinedList.towns.AddRange(modNames.towns);
                    
                    cachedCombinedList.countries.AddRange(DefaultNameLists.countries);
                    cachedCombinedList.countries.AddRange(modNames.countries);
                    
                    cachedCombinedList.institutions.AddRange(DefaultNameLists.institutions);
                    cachedCombinedList.institutions.AddRange(modNames.institutions);
                }

                return cachedCombinedList;
            }
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
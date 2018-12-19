using System;
using System.Collections;
using System.Collections.Generic;
using ModTool;
using UnityEngine;

/// <summary>
/// Class for holding the loaded mod.
/// </summary>
public sealed class ModHolder
{
    /// <summary>
    /// The loaded Mod.
    /// </summary>
    private ModInfo mod;
    private ModManager modManager = ModManager.instance;

    private static readonly Lazy<ModHolder> lazy =
    new Lazy<ModHolder>(() => new ModHolder());

    /// <summary>
    /// The single instance of this class.
    /// </summary>
    public static ModHolder Instance => lazy.Value;

    private ModHolder() { }

    /// <summary>
    /// Get and Save the loaded mod in the modInfo instance.
    /// </summary>
    public void GetLoadedMod()
    {
        foreach (Mod m in modManager.mods)
        {
            if (m.loadState == ResourceLoadState.Loaded)
            {
                this.mod = m.GetAsset<ModInfo>("modinfo");
                break;
            }
        }
    }

    /// <summary>
    /// Returns the modInfo of the currently loaded mod.
    /// </summary>
    /// <returns>The modInfo of the loaded mod, or null if no mod is loaded.</returns>
    public ModInfo GetModInfo()
    {
        return this.mod;
    }

    /// <summary>
    /// Returns the SkillSet used by the loaded mod.
    /// </summary>
    /// <returns>The mod specific SkillSet, or null if no mod is loaded.</returns>
    public SkillSet GetSkills()
    {
        if (mod != null)
        {
            return mod.skillSet;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the EmployeeList used by the loaded mod.
    /// </summary>
    /// <returns>The mod specific EmplyoeeList, or null if no mod is loaded.</returns>
    public EmployeeList GetEmployees()
    {
        if (mod != null)
        {
            return mod.EmployeeList;
        }
        else
        {
            return null;
        }
    }
    
    /// <summary>
    /// Returns the EmployeeList used by the loaded mod.
    /// </summary>
    /// <returns>The mod specific EmplyoeeList, or null if no mod is loaded.</returns>
    public NameLists GetNameLists()
    {
        if (mod != null)
        {
            return mod.NameLists;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the banner of the loaded mod
    /// </summary>
    /// <returns>The banner used by the mod, or null if no mod is loaded.</returns>
    public Sprite GetBanner()
    {
        if (mod != null)
        {
            return mod.banner;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the <see cref="MissionList"/> of the loaded mod
    /// </summary>
    /// <returns>The missionList used by the mod, or null if no mod is loaded.</returns>
    public MissionList GetMissionList()
    {
        if (mod != null)
        {
            return mod.MissionList;
        }
        else
        {
            return null;
        }
    }
}
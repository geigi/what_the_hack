using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using ModTool;
using UnityEngine;
using Wth.ModApi.Employees;

/// <summary>
/// Class for holding the loaded mod.
/// </summary>
public sealed class ModHolder: Singleton<ModHolder>
{
    /// <summary>
    /// The loaded Mod.
    /// </summary>
    private ModInfo mod;

    private Mod modObject;

    /// <summary>
    /// Get and Save the loaded mod in the modInfo instance.
    /// </summary>
    public void GetLoadedMod()
    {
        foreach (Mod m in ModManager.instance.mods)
        {
            if (m.loadState == ResourceLoadState.Loaded)
            {
                modObject = m;
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

    /// <summary>
    /// Get a list of all specials implemented by this mod.
    /// </summary>
    /// <returns></returns>
    public List<Type> GetCustomSpecials()
    {
        if (mod != null)
        {
            var specials = new List<Type>();
            
            foreach (var assembly in modObject.GetAssemblies())
            {
                specials.AddRange(assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(EmployeeSpecial)) && !t.IsAbstract));
            }

            return specials;
        }
        else
        {
            return new List<Type>();
        }
    }
}
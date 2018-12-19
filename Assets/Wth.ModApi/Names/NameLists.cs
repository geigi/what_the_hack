using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Wth.ModApi.Names;
using Random = System.Random;

/// <summary>
/// A Scriptable Object to store any number of first names (male / female) and last names.
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "NameList", menuName = "What_The_Hack ModApi/Name List", order = 21)]
public class NameLists : ScriptableObject
{
    /// <summary>
    /// A list containing all possible male first names.
    /// </summary>
    public List<string> surNamesMale = new List<string>();

    /// <summary>
    /// A list containing all possible female first names.
    /// </summary>
    public List<string> surNamesFemale = new List<string>();

    /// <summary>
    /// A list containing all possible last names.
    /// </summary>
    public List<string> lastNames = new List<string>();

    private static Random rng = new Random();

    public string RandomName(Lists list)
    {
        switch (list)
        {
            case Lists.lastNames:
                return lastNames[rng.Next(this.lastNames.Count())];
            case Lists.surNamesFemale:
                return surNamesFemale[rng.Next(this.surNamesFemale.Count())];
            default:
                return surNamesMale[rng.Next(this.surNamesMale.Count())];
        }
    }
}
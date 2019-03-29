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
    public List<string> firstNamesMale = new List<string>();

    /// <summary>
    /// A list containing all possible female first names.
    /// </summary>
    public List<string> firstNamesFemale = new List<string>();

    /// <summary>
    /// A list containing all possible last names.
    /// </summary>
    public List<string> lastNames = new List<string>();

    /// <summary>
    /// Company names.
    /// </summary>
    public List<string> companyNames = new List<string>();
    
    /// <summary>
    /// Password application names.
    /// </summary>
    public List<string> passwordApplications = new List<string>();
    
    /// <summary>
    /// University names.
    /// </summary>
    public List<string> universities = new List<string>();
    
    /// <summary>
    /// Web service names.
    /// </summary>
    public List<string> webServices = new List<string>();
    
    /// <summary>
    /// Software names.
    /// </summary>
    public List<string> software = new List<string>();
    
    /// <summary>
    /// Town names.
    /// </summary>
    public List<string> towns = new List<string>();
    
    /// <summary>
    /// Country names.
    /// </summary>
    public List<string> countries = new List<string>();
    
    /// <summary>
    /// Names of institutions.
    /// </summary>
    public List<string> institutions = new List<string>();

    /// <summary>
    /// Defines, whether this namelists should be used exclusively or in combination with the build in lists.
    /// </summary>
    public bool UseExclusively = false;

    private static Random rng = new Random();

    /// <summary>
    /// Return a random person name.
    /// Specify type of name with <see cref="PersonNames"/> enum.
    /// </summary>
    /// <param name="personName">Type of name.</param>
    /// <returns></returns>
    public virtual string PersonName(PersonNames personName)
    {
        switch (personName)
        {
            case PersonNames.LastName:
                return LastName();
            case PersonNames.FemaleFirstName:
                return FemaleFirstName();
            case PersonNames.MaleFirstName:
                return MaleFirstName();
            case PersonNames.UndecidedFullName:
                var gender = UnityEngine.Random.Range(0, 1);
                string firstName;
                if (gender < 1)
                    firstName = FemaleFirstName();
                else
                    firstName = MaleFirstName();
                return firstName + " " + LastName();
            case PersonNames.MaleFullName:
                return MaleFirstName() + " " + LastName();
            case PersonNames.FemaleFullName:
                return FemaleFirstName() + " " + LastName();
            default:
                return "Unexpected behaviour";
        }
    }

    /// <summary>
    /// Return a random male first name.
    /// </summary>
    /// <returns></returns>
    public string MaleFirstName()
    {
        return firstNamesMale[rng.Next(this.firstNamesMale.Count())];
    }

    /// <summary>
    /// Return a random female first name.
    /// </summary>
    /// <returns></returns>
    public string FemaleFirstName()
    {
        return firstNamesFemale[rng.Next(this.firstNamesFemale.Count())];
    }

    /// <summary>
    /// Return a random last name.
    /// </summary>
    /// <returns></returns>
    public string LastName()
    {
        return lastNames[rng.Next(this.lastNames.Count())];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Random Company name</returns>
    public string Company()
    {
        return companyNames[rng.Next(companyNames.Count)];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Random PW application name</returns>
    public string PasswordApplication()
    {
        return passwordApplications[rng.Next(passwordApplications.Count)];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Random university name</returns>
    public string University()
    {
        return universities[rng.Next(universities.Count)];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Random web service name</returns>
    public string WebService()
    {
        return webServices[rng.Next(webServices.Count)];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Random software name</returns>
    public string Software()
    {
        return software[rng.Next(software.Count)];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Random town name</returns>
    public string Town()
    {
        return towns[rng.Next(towns.Count)];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Random country name</returns>
    public string Country()
    {
        return countries[rng.Next(countries.Count)];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Random institution name</returns>
    public string Institution()
    {
        return institutions[rng.Next(institutions.Count)];
    }
}
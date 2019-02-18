using UnityEngine;
using Wth.ModApi.Items;

/// <summary>
/// This class represents a definition of an Employee. It is used to create special Employees with custom sprites
/// and specific specials.
/// It contains no data related to a running game.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "EmployeeDefinition", menuName = "What_The_Hack ModApi/Employees/Employee Definition",
    order = -300)]
public class EmployeeDefinition : ScriptableObject
{
    /// <summary>
    /// Name of the employee.
    /// </summary>
    public string EmployeeName = "Max Mustermann";

    /// <summary>
    /// Starting level of the employee.
    /// </summary>
    public int Level = 1;

    /// <summary>
    /// Idle Animation. This is played when the employee is doing nothing.
    /// </summary>
    public AnimationClip IdleAnimation = null;

    /// <summary>
    /// The walking animation.
    /// </summary>
    public AnimationClip WalkingAnimation = null;

    /// <summary>
    /// The working animation. This is used when an employee is sitting at a desk.
    /// </summary>
    public AnimationClip WorkingAnimation = null;

    /// <summary>
    /// The static image of the employee, which is shown when the employee is hire able.
    /// </summary>
    public Sprite image;

    /// <summary>
    /// Likelihood for the employee to spawn, when all conditions are met.
    /// </summary>
    public float SpawnLikelihood = 0.3f;

    public bool StartEmployee = false;

    // Spawn Conditions

    /// <summary>
    /// Specifies if the special Employee should only be spawned when all specific conditions are met.
    /// If this is set to false the Employee will spawn randomly.
    /// </summary>
    public bool SpawnWhenAllConditionsAreMet;

    /// <summary>
    /// Specifies if this special Employee can recur, once fired.
    /// Only one instance of this Employee can be in Game.
    /// </summary>
    public bool Recurring;

    /// <summary>
    /// Specifies how many days need to pass, till this Employee can Spawn.
    /// </summary>
    public int NumberOfDaysTillEmpCanSpawn = 0;

    /// <summary>s
    /// Specifies that these Mission has to be succeeded in order for this employee to spawn.
    /// </summary>
    public MissionDefinition[] MissionSucceeded;

    /// <summary>
    /// Specifies a set of Items that need to be bought, unit this Employee can spawn.
    /// </summary>
    public ItemDefinition[] ItemsBought;

    /// <summary>
    /// Game Progress that has to be reached until this employee can spawn.
    /// </summary>
    public int GameProgress;
}
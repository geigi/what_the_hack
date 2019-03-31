using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class represents a hook which can occur at any time in the mission progress.
/// It can contain a GUI for the player to take extra actions.
/// The hook has to fire HookSuccessful or HookFailed to continue the mission progress.
/// </summary>
[CreateAssetMenu(fileName = "MissionHook", menuName = "What_The_Hack ModApi/Missions/Mission Hook", order = -399)]
public sealed class MissionHook : ScriptableObject
{
    public class MissionHookCompletedEvent : UnityEvent<bool> {}
    
    /// <summary>
    /// This GameObject contains the complete GUI.
    /// Be sure that the first element of the GUI is visible.
    /// Also all code that is required by the hook should be included as scripts
    /// and attached somewhere in the Prefab.
    /// </summary>
    public GameObject GUIPrefab;

    [NonSerialized]
    private UnityEvent<bool> completed;
    /// <summary>
    /// Fire this event when the hook has completed.
    /// The mission will not continue until one of the hooks is fired.
    /// Fire true for success, false for fail.
    /// </summary>
    public UnityEvent<bool> Completed
    {
        get
        {
            if (completed == null)
            {
                completed = new MissionHookCompletedEvent();
            }

            return completed;
        }
        private set => completed = value;
    }

    /// <summary>
    /// This text will be displayed in the failed notification
    /// when this hook was not completed successfully.
    /// </summary>
    public string FailText;

    /// <summary>
    /// Defines when this hook should appear.
    /// Must be between 0 and 1.
    /// </summary>
    [Range(0f, 1f)]
    public float Appear;

    public MissionHook()
    {
        Completed = new MissionHookCompletedEvent();
    }
    
    /// <summary>
    /// Successfully end this hook.
    /// </summary>
    public void RaiseSuccess()
    {
        Completed.Invoke(true);
    }

    /// <summary>
    /// Fail this hook.
    /// </summary>
    public void RaiseFailed()
    {
        Completed.Invoke(false);
    }
}
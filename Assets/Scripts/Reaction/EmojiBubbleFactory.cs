using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Reaction;
using Base;
using UE.Events;
using UnityEngine;

/// <summary>
/// Factory to show reactions 
/// </summary>
public class EmojiBubbleFactory : Singleton<EmojiBubbleFactory>
{ 
    public static readonly Vector3 EMPLYOEE_OFFSET = new Vector3(0, 1.8f);
    
    /// <summary>
    /// Contains all types of different reactions.
    /// </summary>
    public enum EmojiType
    {
        SUCCESS,
        ANGRY,
        OK,
        NO,
        BUBBLE,
        LEVELUP,
        TWITTER,
    }

    /// <summary>
    /// Standard time to display the reaction.
    /// </summary>
    public static readonly float StandardDisplayTime = 3f;

    /// <summary>
    /// Contains all Sprites for the different Reactions.
    /// </summary>
    public Sprite[] emojiBubbleSprites;

    /// <summary>
    /// The time it should take for the sprite to fadeIn / out.
    /// If 2*fadeTime > than the specified displayTime, the sprite will show for 2*fadeTime. 
    /// </summary>
    public float fadeTime = 1f;

    [Header("Emoji Sound Events")] 
    public GameEvent SuccessEvent;
    public GameEvent AngryEvent, OkEvent, NoEvent, LevelUpEvent;
    
    /// <summary>
    /// Gets the Sprite for the corresponding reaction.
    /// </summary>
    /// <param name="type">The EmojiType to get the Sprite for.</param>
    /// <returns>Sprite for the EmojiType</returns>
    protected internal Sprite GetSpriteFromEmojiType(EmojiType type) => 
        emojiBubbleSprites.FirstOrDefault(sprite => sprite.name ==
                                                      $"character-Emojis-{type.ToString().ToLowerInvariant()}");

    private Dictionary<Employee, Coroutine> EmployeeCoroutineMap;

    private void Awake()
    {
        EmployeeCoroutineMap = new Dictionary<Employee, Coroutine>();
    }

    /// <summary>
    /// Reaction to show above an Employee
    /// </summary>
    /// <param name="type">The type of reaction to display</param>
    /// <param name="emp">The employee to show the reaction above</param>
    /// <param name="offset">Defines an offset for the reaction, so it is not shown right on top of the employee sprite</param>
    /// <param name="displayTime">Defines for how long the reaction appears</param>
    public void EmpReaction(EmojiType type, Employee emp, Vector3 offset, float displayTime)
    {
        // Remove a possibly already existing emoji
        if (EmployeeCoroutineMap.ContainsKey(emp) && EmployeeCoroutineMap[emp] != null)
        {
            StopCoroutine(EmployeeCoroutineMap[emp]);
            EmployeeCoroutineMap[emp] = null;
            Destroy(emp.reaction.gameObject);
            emp.reaction = null;
        }
        
        if(2*fadeTime > displayTime) Debug.Log("Your specified displayTime is smaller than the time for fade in and out together!");
        var reaction = InitReaction(type, emp.transform.position, offset);
        emp.reaction = reaction.GetComponent<EmployeeReaction>();
        PlayEmojiSound(type);
        
        var coroutine = StartCoroutine(FadeAndCountdown(reaction, displayTime, emp));
        if (EmployeeCoroutineMap.ContainsKey(emp))
            EmployeeCoroutineMap[emp] = coroutine;
        else
            EmployeeCoroutineMap.Add(emp, coroutine);
    }

    /// <summary>
    /// Emoji to show at any arbitrary position
    /// </summary>
    /// <param name="type">The type of reaction</param>
    /// <param name="position">Position of the emoji</param>
    /// <param name="displayTime">Defines for how long the reaction appears</param>
    public void NonEmpReaction(EmojiType type, Vector3 position, float displayTime)
    {
        if (2 * fadeTime > displayTime) Debug.Log("Your specified displayTime is smaller than the time for fade in and out together!");
        var reaction = InitReaction(type, position, Vector3.zero);
        StartCoroutine(FadeAndCountdown(reaction, displayTime));
    }

    /// <summary>
    /// Initializes any type of reaction
    /// </summary>
    /// <param name="type">The type of reaction</param>
    /// <param name="position">Position of the reaction</param>
    /// <param name="offset">Offset for the reaction</param>
    /// <returns>GameObject which holds the reaction</returns>
    protected internal GameObject InitReaction(EmojiType type, Vector3 position, Vector3 offset)
    {
        Sprite reactionSprite = GetSpriteFromEmojiType(type);
        GameObject reactionObject = new GameObject("Reaction");
        var reaction = reactionObject.AddComponent<EmployeeReaction>();
        reaction.offset = offset;
        reaction.Position = position;
        reaction.ShowReaction(reactionSprite);
        return reactionObject;
    }

    /// <summary>
    /// Shows the Sprite with fade in / out for the specified time. Destroys the object at the end.
    /// </summary>
    /// <param name="reaction">The reaction gameObject</param>
    /// <param name="time">Time to show the reaction</param>
    /// <param name="emp">[Optional] Employee for when the reaction was to show above an employee</param>
    /// <returns></returns>
    private IEnumerator FadeAndCountdown(GameObject reaction, float time, Employee emp = null)
    {
        var spriteRenderer = reaction.GetComponent<SpriteRenderer>();
        var col = spriteRenderer.color;
        col.a = 0;
        spriteRenderer.color = col;
        while (col.a < 1)
        {
            var stepChange = Time.deltaTime / fadeTime;
            if (col.a + stepChange > 1f)
                col.a = 1f;
            else
                col.a += stepChange;
            
            spriteRenderer.color = col;
            yield return 0;
        }
        yield return new WaitForSeconds(time - 2*fadeTime);
        while (col.a > 0)
        {
            var stepChange = Time.deltaTime / fadeTime;
            if (col.a - stepChange < 0f)
                col.a = 0f;
            else
                col.a -= stepChange;
            
            spriteRenderer.color = col;
            yield return 0;
        }

        if (emp != null)
        {
            emp.reaction = null;
            EmployeeCoroutineMap[emp] = null;
        }
        Destroy(reaction.gameObject);
    }

    /// <summary>
    /// Play the matching sound to a given emoji type.
    /// </summary>
    /// <param name="type">The emoji type to play the sound to.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void PlayEmojiSound(EmojiType type)
    {
        switch (type)
        {
            case EmojiType.SUCCESS:
                if (SuccessEvent != null) SuccessEvent.Raise();
                break;
            case EmojiType.ANGRY:
                if (AngryEvent != null) AngryEvent.Raise();
                break;
            case EmojiType.OK:
                if (OkEvent != null) OkEvent.Raise();
                break;
            case EmojiType.NO:
                if (NoEvent != null) NoEvent.Raise();
                break;
            case EmojiType.BUBBLE:
                break;
            case EmojiType.LEVELUP:
                if (LevelUpEvent != null) LevelUpEvent.Raise();
                break;
            case EmojiType.TWITTER:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}

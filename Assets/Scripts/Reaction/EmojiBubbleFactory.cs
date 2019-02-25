using System.Collections;
using System.Linq;
using Assets.Scripts.Reaction;
using Base;
using NSubstitute;
using UE.Variables;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// Factory to show reactions 
/// </summary>
public class EmojiBubbleFactory : Singleton<EmojiBubbleFactory>
{ 
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
    public float StandardDisplayTime = 3f;

    /// <summary>
    /// Contains all Sprites for the different Reactions.
    /// </summary>
    public Sprite[] emojiBubbleSprites;

    /// <summary>
    /// The time it should take for the sprite to fadeIn / out.
    /// If 2*fadeTime > than the specified displayTime, the sprite will show for 2*fadeTime. 
    /// </summary>
    public float fadeTime = 1f;

    /// <summary>
    /// step Variable for the FadeAnimation.
    /// How much to advance the alpha each iteration
    /// </summary>
    [UnityEngine.Range(0, 1)]
    public float step = 0.1f;

    /// <summary>
    /// Gets the Sprite for the corresponding reaction.
    /// </summary>
    /// <param name="type">The EmojiType to get the Sprite for.</param>
    /// <returns>Sprite for the EmojiType</returns>
    protected internal Sprite GetSpriteFromEmojiType(EmojiType type) => 
        emojiBubbleSprites.FirstOrDefault(sprite => sprite.name ==
                                                      $"character-Emojis-{type.ToString().ToLowerInvariant()}");

    /// <summary>
    /// Reaction to show above an Employee
    /// </summary>
    /// <param name="type">The type of reaction to display</param>
    /// <param name="emp">The employee to show the reaction above</param>
    /// <param name="offset">Defines an offset for the reaction, so it is not shown right on top of the employee sprite</param>
    /// <param name="displayTime">Defines for how long the reaction appears</param>
    public void EmpReaction(EmojiType type, Employee emp, Vector3 offset, float displayTime)
    {
        if(2*fadeTime > displayTime) Debug.Log("Your specified displayTime is smaller than the time for fade in and out together!");
        var reaction = InitReaction(type, emp.transform.position, offset);
        emp.reaction = reaction.GetComponent<EmployeeReaction>();
        StartCoroutine(FadeAndCountdown(reaction, displayTime, emp));
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
        var timeStep = fadeTime * step;
        var spriteRenderer = reaction.GetComponent<SpriteRenderer>();
        var col = spriteRenderer.color;
        col.a = 0;
        spriteRenderer.color = col;
        while (col.a < 1)
        {
            col.a += step;
            spriteRenderer.color = col;
            yield return new WaitForSeconds(timeStep);
        }
        yield return new WaitForSeconds(time - 2*fadeTime);
        while (col.a > 0)
        {
            col.a -= step;
            spriteRenderer.color = col;
            yield return new WaitForSeconds(timeStep);
        }
        if (emp != null) emp.reaction = null;
        Destroy(reaction);
    }
}

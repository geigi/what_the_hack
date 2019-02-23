using System.Collections;
using System.Linq;
using Assets.Scripts.Reaction;
using Base;
using UnityEngine;

public class EmojiBubbleFactory : Singleton<EmojiBubbleFactory>
{

    public const float StandardDisplayTime = 7f; 

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

    protected internal ContentHub contentHub;

    protected internal GameObject reactionGameObject;

    internal void Awake()
    {
        contentHub = ContentHub.Instance;
        reactionGameObject = GameObject.FindGameObjectWithTag("EmployeeReactions");
    }

    public Sprite GetSpriteFromEmojiType(EmojiType type) => 
        contentHub.emojiBubbleSprites.FirstOrDefault(sprite => sprite.name ==
                                                      $"character-Emojis-{type.ToString().ToLowerInvariant()}");

    public void EmpReaction(EmojiType type, Employee emp, Vector3 offset, float displayTime)
    {
        var reaction = InitReaction(type, emp.transform.position, offset);
        emp.reaction = reaction.GetComponent<EmployeeReaction>();
        StartCoroutine(Countdown(reaction, displayTime, emp));
    }

    public void NonEmpReaction(EmojiType type, Vector3 position, float displayTime)
    {
        var reaction = InitReaction(type, position, Vector3.zero);
        StartCoroutine(Countdown(reaction, displayTime));
    }

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

    public IEnumerator Countdown(GameObject reaction, float time, Employee emp = null)
    {
        yield return new WaitForSeconds(time);
        if (emp != null) emp.reaction = null;
        Destroy(reaction);
    }
}

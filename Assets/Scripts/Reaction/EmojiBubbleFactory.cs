using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Reaction;
using Base;
using JetBrains.Annotations;
using NSubstitute;
using UE.Variables;
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

    public void EmpReaction(Employee emp, EmojiType type, float displayTime)
    {
        var reaction = InitReaction(type, emp.transform.position, new Vector3(0, 2, 0));
        emp.reaction = reaction.GetComponent<EmployeeReaction>();
        StartCoroutine(Countdown(reaction, displayTime, emp));
    }

    public void NonEmpReaction(Vector3 position, EmojiType type, float displayTime)
    {
        var reaction = InitReaction(type, position, Vector3.zero);
        StartCoroutine(Countdown(reaction, displayTime));
    }

    private GameObject InitReaction(EmojiType type, Vector3 position, Vector3 offset)
    {
        Sprite reactionSprite = GetSpriteFromEmojiType(type);
        GameObject reactionObject = new GameObject("Reaction");
        var reaction = reactionObject.AddComponent<EmployeeReaction>();
        reaction.offset = offset;
        reaction.ShowReaction(reactionSprite, position);
        return reactionObject;
    }

    public IEnumerator Countdown(GameObject reaction, float time, Employee emp = null)
    {
        yield return new WaitForSeconds(time);
        if (emp != null) emp.reaction = null;
        Destroy(reaction);
    }
}

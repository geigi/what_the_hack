using System.Collections;
using System.Collections.Generic;
using NSubstitute.Core;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Implementation of an Shadow for a specific Employee
/// </summary>
public class EmployeeShadow : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// The current Sprite for the Shadow.
    /// Ideally the sprite should be set through AnimationEvents
    /// </summary>
    public Sprite currentSprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }

    private Vector2 position;
    /// <summary>
    /// The position of this GameObject and therefore the Sprite.
    /// </summary>
    public Vector2 Position
    {
        get => position;
        set { position = value;
            gameObject.transform.position = value;
        }
    }

    public void Awake()
    {
        this.spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        this.spriteRenderer.sortingOrder = 0;
        this.transform.parent = GameObject.FindWithTag("EmployeeShadows").transform;
    }

    public void SetSpriteThroughObject(Object obj) => currentSprite = (Sprite) obj;
}

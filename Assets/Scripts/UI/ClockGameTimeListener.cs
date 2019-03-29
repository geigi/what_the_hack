using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// This class listens to an event and updates a clock with
/// a given set of sprites.
/// </summary>
public class ClockGameTimeListener : MonoBehaviour
{
    /// <summary>
    /// Game time event to listen to.
    /// </summary>
    public IntEvent GameTimeTickEvent;
    /// <summary>
    /// List of sprites.
    /// </summary>
    public Sprite[] ClockSprites;

    private UnityAction<int> tickAction;
    private Image clockImage;
    
    // Start is called before the first frame update
    void Awake()
    {
        clockImage = GetComponent<Image>();
        tickAction += TickListener;
        GameTimeTickEvent.AddListener(tickAction);
        //TickListener(GameTime.GameTime.Instance.Gettim);
    }

    /// <summary>
    /// Update UI Image with sprite of current game tick.
    /// </summary>
    /// <param name="tick"></param>
    private void TickListener(int tick)
    {
        clockImage.sprite = ClockSprites[tick];
    }
}

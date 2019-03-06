using System;
using System.Collections;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BalanceChangedListener : MonoBehaviour
{
    /// <summary>
    /// The (very approximated) time to reach the end of the Animation.
    /// The bigger the value the longer it will take.
    /// A Value of 1 yields an animation time of approximately 4.5 seconds.
    /// </summary>
    public float smoothTime = 1f;
    /// <summary>
    /// Reference to the Event that is fired when the player-balance changes.
    /// </summary>
    public IntEvent moneyEvent;
    internal UnityAction<int> evtAction;
    /// <summary>
    /// Reference to the text, which shows the balance.
    /// </summary>
    protected internal Text text;
    /// <summary>
    /// True iff the animation is currently running.
    /// </summary>
    private bool animatingMoney = false;

    internal void Awake()
    {
        evtAction += ChangeBalance;
        moneyEvent.AddListener(evtAction);
        text = gameObject.GetComponent<Text>();
        text.text = ContentHub.Instance.bank.GetData().ToString();
    }

    /// <summary>
    /// Starts the coroutine responsible for handling the animation. 
    /// </summary>
    /// <param name="newBalance">The new balance of the player</param>
    protected internal void ChangeBalance(int newBalance) => StartCoroutine(PreAnimation(newBalance));

    /// <summary>
    /// Handles the animation.
    /// If another money animation is currently playing, this function will wait, until the other animation has finished.
    /// </summary>
    /// <param name="newBalance">The new balance of the player.</param>
    /// <returns></returns>
    private IEnumerator PreAnimation(int newBalance)
    {
        while(animatingMoney){ yield return new WaitForSeconds(0.1f);}

        int oldValue = Convert.ToInt32(text.text);
        StartCoroutine(BalanceChangeAnimation(oldValue, newBalance));
    }

    /// <summary>
    /// Function responsible for animating the change in balance.
    /// Ony a single instance of the coroutine should run at any time, to avoid errors.
    /// </summary>
    /// <param name="oldValue">The old balance</param>
    /// <param name="newValue">The new balance</param>
    /// <returns></returns>
    private IEnumerator BalanceChangeAnimation(int oldValue, int newValue)
    {
        animatingMoney = true;
        float currentBalance = oldValue;
        float percent = 0.1f;
        while (Mathf.RoundToInt(currentBalance) != Mathf.RoundToInt(newValue))
        {
            currentBalance = Mathf.SmoothDamp(currentBalance, newValue,  ref percent, smoothTime);
            text.text = $"{Mathf.RoundToInt(currentBalance)}";
            yield return 0;
        }
        animatingMoney = false;
    }
}

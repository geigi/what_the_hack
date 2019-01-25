using System.Collections;
using System.Collections.Generic;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BalanceChangedListener : MonoBehaviour
{
    public IntEvent moneyEvent;
    internal UnityAction<int> evtAction;

    internal void Awake()
    {
        evtAction += ChangeBalance;
        moneyEvent.AddListener(evtAction);
    }

    private void ChangeBalance(int newBalance)
    {
        GetComponent<Text>().text = $"{newBalance}";
    }
}

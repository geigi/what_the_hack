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
    private Text text;

    internal void Awake()
    {
        evtAction += ChangeBalance;
        moneyEvent.AddListener(evtAction);
        text = gameObject.GetComponent<Text>();
        text.text = ContentHub.Instance.bank.GetData().ToString();
    }

    private void ChangeBalance(int newBalance)
    {
        text.text = $"{newBalance}";
    }
}

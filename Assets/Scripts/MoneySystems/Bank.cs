using System;
using System.Collections;
using System.Collections.Generic;
using Employees;
using GameSystem;
using Interfaces;
using SaveGame;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using Wth.ModApi.Employees;

public class Bank : MonoBehaviour, ISaveable<int>
{

    /// <summary>
    /// Amount of money at the start of a new Game.
    /// </summary>
    public int StartFund = 8000;

    /// <summary>
    /// Event which is raised, when the balance is set.
    /// </summary>
    public IntEvent balanceChanged;

    protected internal int balance;

    /// <summary>
    /// Amount of money the player has.
    /// </summary>
    public int Balance
    {
        get => balance;
        private set
        {
            balance = value;
            balanceChanged.Raise(balance);
        }
    }

    public void Awake()
    {
        if (! GameSettings.NewGame)
            LoadState();
        else Balance = StartFund;
    }

    private void Start()
    {
        // Fire the event to update gui etc with starting balance
        balanceChanged.Raise(balance);
    }

    /// <summary>
    /// Subtracts the prize from the current balance iff the player has enough money.
    /// </summary>
    /// <param name="prize">The prize to be payed.</param>
    /// <returns>True iff the player has enough money, false otherwise</returns>
    public virtual bool Pay(int prize)
    {
        if (Balance < prize) return false;
        Balance -= prize;
        return true;
    }

    /// <summary>
    /// Income is added to the current balance.
    /// </summary>
    /// <param name="income">The income to be added</param>
    public void Income(int income) => Balance += income;

    /// <summary>
    /// Load state from a given savegame.
    /// </summary>
    private void LoadState()
    {
        var mainSaveGame = gameObject.GetComponent<SaveGameSystem>().GetCurrentSaveGame();
        balance = mainSaveGame.balance;
    }

    /// <inheritdoc />
    public int GetData()
    {
        return Balance;
    }
}

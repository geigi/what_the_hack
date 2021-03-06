﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using Base;
using GameSystem;
using Interfaces;
using SaveGame;
using UE.Events;
using UnityEngine;
using UnityEngine.UI;

namespace GameTime
{
    /// <summary>
    /// This class manages the GameTime.
    /// </summary>
    public class GameTime : Singleton<GameTime>, ISaveable<GameTimeData>
    {
        /// <summary>
        /// Number of real time seconds that elapse per in-game time tick.
        /// Only used in classic game mode.
        /// </summary>
        public float ClassicSecondsPerTick = 4.5f;
        /// <summary>
        /// Number of real time minutes that elapse per in-game time tick.
        /// Only used in realtime game mode.
        /// </summary>
        public float RealtimeMinutesPerTick = 0.05f;

        /// <summary>
        /// Number of time ticks per day.
        /// Used in classic and realtime game mode.
        /// </summary>
        public int ClockSteps = 9;

        public IntEvent GameTickEvent;
        public NetObjectEvent GameDayEvent;
        
        private GameTimeData data;
        private Coroutine tickRoutine;

        private bool running = false;
        private bool gameLoaded = false;

        /// <summary>
        /// Initialize the game time system.
        /// Restores the game time if a savegame is present.
        /// </summary>
        public void Awake()
        {
            if (GameSettings.NewGame)
                Initialize();
            else
            {
                LoadState();
            }
        }

        private void Start()
        {
            gameLoaded = true;
            if (!GameSettings.NewGame && SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Realtime)
                FirePastTicks();
        }

        void OnApplicationPause(bool isPaused)
        {
            if (gameLoaded && !isPaused && SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Realtime)
            {
                FirePastTicks();
            }
        }

        public void StartGame()
        {
            if (!running)
            {
                running = true;
                tickRoutine = StartCoroutine(Tick());
            }
        }

        /// <summary>
        /// Pause the game.
        /// </summary>
        /// <param name="pause"></param>
        public void Pause(bool pause)
        {
            if (pause && running)
                stop();
            else if (!running && !pause)
                StartGame();
        }

        public int GetCurrentStep()
        {
            return data.Step;
        }
        
        private void stop()
        {
            running = false;
            StopCoroutine(tickRoutine);
        }

        /// <summary>
        /// Initialize the default state.
        /// </summary>
        private void Initialize()
        {
            data = new GameTimeData();
        }

        /// <summary>
        /// Restore the state of this object from the current savegame.
        /// </summary>
        private void LoadState()
        {
            var saveGame = SaveGameSystem.Instance.GetCurrentSaveGame();
            data = saveGame.gameTime;
        }

        /// <summary>
        /// Return the serializable object which stores the game time data.
        /// </summary>
        /// <returns>Serializable game time data</returns>
        public virtual GameTimeData GetData()
        {
            return data;
        }

        /// <summary>
        /// Get the number of game days that have been passed since the start of the game.
        /// </summary>
        /// <returns></returns>
        public int GetTotalGameDays()
        {
            return (data.Date.GetDateTime() - new DateTime(1, 1, 1)).Days + 1;
        }

        public int RemainingTicksTillPayday()
        {
            int dayTillPayDay = (int) (7 - data.Date.DayOfWeek);
            int ticks = ClockSteps - data.Step;
            ticks += dayTillPayDay * ClockSteps;
            
            return ticks;
        }

        /// <summary>
        /// Process a game tick.
        /// First wait depending on the game mode.
        /// Then increment the date objects and notifiy listeners.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Tick()
        {
            while (running)
            {
                // Sleep
                if (SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Classic)
                {
                    yield return new WaitForSeconds(ClassicSecondsPerTick);
                }
                else
                {
                    yield return new WaitForSeconds(RealtimeMinutesPerTick * 60);
                }

                doTick();
            }
        }

        private void doTick()
        {
            data.Step++;

            // Update data
            if (data.Step < ClockSteps)
            {
                GameTickEvent.Raise(data.Step);
            }
            else
            {
                data.Step = 0;
                data.Date.IncrementDay();
                GameTickEvent.Raise(data.Step);
                GameDayEvent.Raise(data.Date);
            }
        }

        private void FirePastTicks()
        {
            var now = DateTime.Now;
            var span = now.Subtract(SaveGameSystem.Instance.GetCurrentSaveGame().saveDate);

            int ticks = (int) (span.TotalMinutes / RealtimeMinutesPerTick);
            Debug.Log("Starting progress recalculation now with " + ticks + " ticks.");
            for (int i = 0; i < ticks; i++)
            {
                doTick();
            }
        }
    }
}
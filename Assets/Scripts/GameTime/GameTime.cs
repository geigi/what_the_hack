using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Interfaces;
using SaveGame;
using UnityEngine;

namespace GameTime
{
    /// <summary>
    /// This class manages the GameTime.
    /// </summary>
    public class GameTime : MonoBehaviour, Saveable<GameTimeData>
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
        public float RealtimeMinutesPerTick = 20f;
        /// <summary>
        /// Number of time ticks per day.
        /// Used in classic and realtime game mode.
        /// </summary>
        public int ClockSteps = 9;
        
        private GameTimeData data;
        private Coroutine classicTimeRoutine;
        private Coroutine realTimeRoutine;

        /// <summary>
        /// Initialize the game time system.
        /// Restores the game time if a savegame is present.
        /// </summary>
        public void Awake()
        {
            if (GameSettings.NewGame)
                Initialize();
            else
                LoadState();
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
            var saveGame = gameObject.GetComponent<SaveGameSystem>().GetCurrentSaveGame();
            data = saveGame.gameTime;
        }

        /// <summary>
        /// Return the serializable object which stores the game time data.
        /// </summary>
        /// <returns>Serializable game time data</returns>
        public GameTimeData GetData()
        {
            return data;
        }
    }
}
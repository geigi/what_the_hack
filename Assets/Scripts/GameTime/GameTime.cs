using System;
using System.Collections;
using System.Collections.Generic;
using SaveGame;
using UnityEngine;

namespace GameTime
{
    /// <summary>
    /// This class manages the GameTime.
    /// </summary>
    public class GameTime : MonoBehaviour
    {
        private GameTimeData data;
        
        public float ClassicSecondsPerTick = 4.5f;
        public float RealtimeMinutesPerTick = 20f;
        public int ClockSteps = 9;
        
        public void InitReferences()
        {
            throw new System.NotImplementedException();
        }

        public void InitDefaultState()
        {
            data = new GameTimeData();
        }

        public void LoadState(MainSaveGame mainSaveGame)
        {
            data = mainSaveGame.gameTime;
        }

        public void Cleanup()
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}

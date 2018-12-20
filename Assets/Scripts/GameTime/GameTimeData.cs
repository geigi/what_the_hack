using System;
using GameSystem;

namespace GameTime
{
    /// <summary>
    /// This class contains all gametime data needed to save/load a game.
    /// </summary>
    [Serializable]
    public class GameTimeData
    {
        /// <summary>
        /// GameTimeMode of this savegame.
        /// </summary>
        public SettingsManager.GameTimeMode GameTimeMode;
        
        /// <summary>
        /// In-game date in classic game mode.
        /// </summary>
        public DateTime ClassicDate;
        
        /// <summary>
        /// Date when the last step occured in realtime game mode.
        /// </summary>
        public DateTime RealtimeLastStep;
        
        /// <summary>
        /// Current step of day in either game mode.
        /// </summary>
        public int Step;
    }
}
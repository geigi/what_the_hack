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
        /// In-game date in classic game mode.
        /// </summary>
        public GameDate Date;
        
        /// <summary>
        /// Date when the last step occured in realtime game mode.
        /// </summary>
        public DateTime RealtimeLastStep;
        
        /// <summary>
        /// Current step of day in either game mode.
        /// </summary>
        public int Step;

        public GameTimeData()
        {
            Date = new GameDate();
            Step = 0;
        }
    }
}
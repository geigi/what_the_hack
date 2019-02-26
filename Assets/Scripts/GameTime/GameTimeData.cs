using System;
using GameSystem;

namespace GameTime
{
    /// <summary>
    /// This class contains all gametime data needed to save/load a game.
    /// </summary>
    [Serializable]
    public class GameTimeData: ICloneable
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

        /// <summary>
        /// Get the difference between this date and another given date in timesteps.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int TimeStepDifference(GameTimeData other)
        {
            int difference = other.Step - Step;
            var dayDifference = (int)(other.Date.DateTime - Date.DateTime).TotalDays;
            difference += dayDifference * GameTime.Instance.ClockSteps;
            return difference;
        }
        
        /// <summary>
        /// Clone this object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var clone = new GameTimeData();
            clone.Date = (GameDate) Date.Clone();
            clone.Step = Step;
            return clone;
        }
    }
}
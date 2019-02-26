using System;

namespace GameTime
{
    /// <summary>
    /// Enum representing a day of the week.
    /// </summary>
    public enum DayOfWeek
    {
        Monday = 0,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }
    
    /// <summary>
    /// This class represents a specific date in the game.
    /// </summary>
    [Serializable]
    public class GameDate: ICloneable
    {
        /// <summary>
        /// Date. Only Day, Month and Year are used in the game.
        /// WARNING: Don't modify the date manually!
        /// </summary>
        public DateTime DateTime;
        /// <summary>
        /// Current day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek;
        
        
        public GameDate()
        {
            DateTime = new DateTime(1, 1, 1);
            DayOfWeek = DayOfWeek.Monday;
        }

        /// <summary>
        /// Increment the GameDate by one day.
        /// </summary>
        public void IncrementDay()
        {
            DateTime = DateTime.AddDays(1);

            if (DayOfWeek == DayOfWeek.Sunday)
            {
                DayOfWeek = DayOfWeek.Monday;
            }
            else
            {
                DayOfWeek++;
            }
        }

        /// <summary>
        /// Get the current <see cref="DateTime"/> object.
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            return DateTime;
        }
        
        /// <summary>
        /// Create a clone of this object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var date = new GameDate();
            date.DateTime = date.DateTime.AddYears(DateTime.Year - 1);
            date.DateTime = date.DateTime.AddMonths(DateTime.Month - 1);
            date.DateTime = date.DateTime.AddDays(DateTime.Day - 1);
            date.DayOfWeek = DayOfWeek;
            return date;
        }
    }
}
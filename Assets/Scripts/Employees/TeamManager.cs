using System;
using UnityEngine;

namespace Employees
{
    /// <summary>
    /// This class manages team specific stuff like calculating the game progress.
    /// </summary>
    public class TeamManager: MonoBehaviour
    {
        #region Singleton
        private static readonly Lazy<TeamManager> lazy = 
            new Lazy<TeamManager>(() => GameObject.FindWithTag("Managers").GetComponent<TeamManager>());

        /// <summary>
        /// The single Instance of this class
        /// </summary>
        public static TeamManager Instance => lazy.Value;

        private TeamManager() { }
        #endregion


        /// <summary>
        /// Calculate the current game progress.
        /// </summary>
        /// <returns></returns>
        public int calcGameProgress()
        {
            return 0;
        }
    }
}
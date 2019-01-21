using System;
using UnityEngine;

namespace Employees
{
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


        public int calcGameProgress()
        {
            return 0;
        }
    }
}
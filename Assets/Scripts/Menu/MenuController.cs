﻿using SaveGame;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    /// <summary>
    /// This class handles main menu specific logic.
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        /// <summary>
        /// Button that resumes a game.
        /// </summary>
        public Button ResumeButton;
    
        void Start()
        {
            // Show Resume button only when a savegame exists
            if (SaveGameSystem.DoesSaveGameExist(SaveGameSystem.DEFAULT_SAVE_GAME_NAME))
                ResumeButton.gameObject.SetActive(true);
        }
    }
}

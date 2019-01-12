using System.Collections;
using System.Collections.Generic;
using GameSystem;
using SaveGame;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

    public void NewGame()
    {
        GameSettings.NewGame = true;
        GameSettings.ModID = "";
        GotoMainScene();
    }

    public void LoadGame()
    {
        GameSettings.NewGame = false;
        GameSettings.SaveGameName = SaveGameSystem.DEFAULT_SAVE_GAME_NAME;
        GotoMainScene();
    }
    
    /// <summary>
    /// Load the Menu Scene.
    /// </summary>
    public void GotoMenuScene()
    {
        ContentHub.Instance.SaveGameSystem.SaveGame(SaveGameSystem.DEFAULT_SAVE_GAME_NAME);
        SceneManager.LoadScene("Menu");
    }
    
    /// <summary>
    /// Load the Main Game Scene. 
    /// </summary>
    private void GotoMainScene()
    {
        SceneManager.LoadScene("MainGame");
    }
}

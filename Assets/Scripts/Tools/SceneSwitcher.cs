using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameSystem;
using ModTool;
using SaveGame;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private Mod mod;
    public void NewGame()
    {
        GameSettings.NewGame = true;
        GameSettings.ModID = "";
        GotoMainScene();
    }

    public void LoadGame()
    {
        if (File.Exists(SaveGameSystem.GetSavePath("modinfo")))
        {
            var modId = File.ReadAllText(SaveGameSystem.GetSavePath("modinfo"));
            mod = ModManager.instance.mods.First(m => m.name == modId);
            ModManager.instance.ModLoaded += ModLoaded;
            mod.Load();
        }
        else
        {
            doLoadGame();
        }
    }

    private void doLoadGame()
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
        SceneLoaderAsync.Instance.LoadScene();
    }

    private void ModLoaded(Mod mod)
    {
        ModManager.instance.ModLoaded -= ModLoaded;
        doLoadGame();
    }
}

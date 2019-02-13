using System;
using GameSystem;
using SaveGame;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This hub offers some helpful options to set when developing the game.
/// </summary>
public class DebugHub : EditorWindow
{
    public static readonly string LOAD_GAME = "DEBUG_LOAD_GAME";
    
    [MenuItem("Tools/Debug Hub")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(DebugHub), false, "Debug Hub");
    }
    
    private void OnGUI()
    {
        if (ContentHub.Instance.bank == null) return;
        
        // Header
        GUILayout.BeginHorizontal();
        GUILayout.Label("Debug Hub", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Savegame", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();
        
        #region Load game at startup
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        var loadGame = EditorPrefs.GetBool(LOAD_GAME);
        loadGame = EditorGUILayout.Toggle ("Load game on startup", loadGame, GUILayout.Width(200));
        EditorPrefs.SetBool(LOAD_GAME, loadGame);
        #endregion
        
        GUILayout.Space(10f);

        #region Save Game
        
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        
        if (GUILayout.Button("Save Game", GUILayout.Width(85)))
        {
            SaveGameSystem.Instance.SaveGame(SaveGameSystem.DEFAULT_SAVE_GAME_NAME);
        }
        EditorGUI.EndDisabledGroup();
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion
        
        GUILayout.Space(10f);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Game Options", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();

        #region GameTime Mode
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        var selected = (SettingsManager.GameTimeMode)EditorGUILayout.EnumPopup("GameTime Mode", SettingsManager.GetGameTime(), GUILayout.Width(300));
        SettingsManager.SetGameTime((int) selected);
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion
        
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("In-Game Variables", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();
        
        #region Balance
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        var bank = ContentHub.Instance.bank;
        var newBalance = EditorGUILayout.IntField("Balance:", bank.Balance, GUILayout.Width(300));
        if (newBalance != bank.Balance)
            bank.Income(newBalance - bank.Balance);
        EditorGUI.EndDisabledGroup();
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion
        
        #region Game Speed
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        var gameTime = GameTime.GameTime.Instance;

        float oldSpeed;
        if (SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Classic)
            oldSpeed = gameTime.ClassicSecondsPerTick;
        else
            oldSpeed = gameTime.RealtimeMinutesPerTick;
        
        var newSpeed = EditorGUILayout.Slider("Step Duration:", oldSpeed, 0.1f, 20f, GUILayout.Width(300));
        if (Math.Abs(newSpeed - oldSpeed) > 0.001f)
            if (SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Classic)
                gameTime.ClassicSecondsPerTick = newSpeed;
            else
                gameTime.RealtimeMinutesPerTick = newSpeed;
            
        EditorGUI.EndDisabledGroup();
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion
    }
}
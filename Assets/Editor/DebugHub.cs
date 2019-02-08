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
        // Header
        GUILayout.BeginHorizontal();
        GUILayout.Label("Debug Hub", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("SaveGame", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        // Save Game
        if(GUILayout.Button("Save Game"))
        {
            SaveGameSystem.Instance.SaveGame(SaveGameSystem.DEFAULT_SAVE_GAME_NAME);
        }
        EditorGUI.EndDisabledGroup();
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10f);
        
        // Load Game at Startup
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        var loadGame = EditorPrefs.GetBool(LOAD_GAME);
        loadGame = EditorGUILayout.Toggle ("Load game on startup", loadGame);
        EditorPrefs.SetBool(LOAD_GAME, loadGame);
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10f);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("In-Game Variables", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        var bank = ContentHub.Instance.bank;
        var newBalance = EditorGUILayout.IntField("Balance:", bank.Balance);
        if (newBalance != bank.Balance)
            bank.Income(newBalance - bank.Balance);
        EditorGUI.EndDisabledGroup();
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
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
        GUILayout.BeginHorizontal();
        GUILayout.Label("Debug Hub", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        // load the game on scene startup
        var loadGame = EditorPrefs.GetBool(LOAD_GAME);
        loadGame = EditorGUILayout.Toggle ("Load Game", loadGame);
        EditorPrefs.SetBool(LOAD_GAME, loadGame);
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
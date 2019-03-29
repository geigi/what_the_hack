using System;
using System.Globalization;
using Assets.Scripts.NotificationSystem;
using Employees;
using GameSystem;
using SaveGame;
using Team;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This hub offers some helpful options to set when developing the game.
/// </summary>
public class DebugHub : EditorWindow
{
    public static readonly string LOAD_GAME = "DEBUG_LOAD_GAME";
    private Vector2 scrollPos;
    private string notification = "";
    
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
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth));
        
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
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Game Options", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();

        #region GameTime Mode
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        
        var selected = (SettingsManager.GameTimeMode)EditorGUILayout.EnumPopup("GameTime Mode", SettingsManager.GetGameTime(), GUILayout.ExpandWidth(true));
        SettingsManager.SetGameTime((int) selected);
        
        GUILayout.EndHorizontal();
        #endregion
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("In-Game Variables", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();
        
        #region Balance
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        var bank = ContentHub.Instance.bank;
        
        var newBalance = EditorGUILayout.IntField("Balance:", bank.Balance, GUILayout.ExpandWidth(true));
        if (newBalance != bank.Balance)
            bank.Income(newBalance - bank.Balance);
        EditorGUI.EndDisabledGroup();
        
        GUILayout.EndHorizontal();
        #endregion
        
        #region Game Speed
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        var gameTime = GameTime.GameTime.Instance;

        float oldSpeed;
        if (SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Classic)
            oldSpeed = gameTime.ClassicSecondsPerTick;
        else
            oldSpeed = gameTime.RealtimeMinutesPerTick;
        
        var newSpeed = EditorGUILayout.Slider("Step Duration:", oldSpeed, 0.1f, 20f, GUILayout.ExpandWidth(true));
        if (Math.Abs(newSpeed - oldSpeed) > 0.001f)
            if (SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Classic)
                gameTime.ClassicSecondsPerTick = newSpeed;
            else
                gameTime.RealtimeMinutesPerTick = newSpeed;
            
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
        #endregion
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Game Monitor", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();
        
        #region Game Monitor
        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.FloatField("Game Progress:", TeamManager.Instance.calcGameProgress(), GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);

            notification = EditorGUILayout.TextField("Notification: ", notification, GUILayout.ExpandWidth(true));
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Post Notification", GUILayout.Width(85)))
            {
                NotificationManager.Instance.Info(notification);
            }
        }
        #endregion
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Employee Monitor", EditorStyles.miniBoldLabel);
        GUILayout.EndHorizontal();
        
        #region Employee Monitor

        if (Application.isPlaying)
        {
            foreach (var employee in EmployeeManager.Instance.GetData().hiredEmployees)
            {
                var emp = EmployeeManager.Instance.GetEmployee(employee);
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(emp.Name, EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                GUILayout.Space(10);
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Spawn Emoji", GUILayout.Width(85)))
                {
                    EmojiBubbleFactory.Instance.EmpReaction(EmojiBubbleFactory.EmojiType.OK, emp, EmojiBubbleFactory.EMPLYOEE_OFFSET, EmojiBubbleFactory.StandardDisplayTime);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                GUILayout.Space(10);
                
                EditorGUILayout.IntField("Level:", employee.Level, GUILayout.ExpandWidth(true));
                employee.SkillPoints = EditorGUILayout.IntField("Skill Points: ", employee.SkillPoints, GUILayout.ExpandWidth(true));
                var freeScore = EditorGUILayout.FloatField("Free Score:", employee.FreeScore, GUILayout.ExpandWidth(true));
                if (Math.Abs(freeScore - employee.FreeScore) > 0.01)
                    employee.IncrementFreeScore(freeScore - employee.FreeScore);
                EditorGUILayout.FloatField("Used Score:", employee.UsedScore, GUILayout.ExpandWidth(true));
                EditorGUILayout.FloatField("Next Level Score:", employee.LevelUpScoreNeeded, GUILayout.ExpandWidth(true));

                GUILayout.Space(10);
                
                foreach (var skill in employee.Skills)
                {
                    GUILayout.Label(skill.GetName(), GUILayout.Width(300));
                    GUILayout.Space(2);
                    EditorGUILayout.IntField("Level:", skill.Level, GUILayout.ExpandWidth(true));
                    GUILayout.Space(10);
                }
            }
        }
        
        #endregion
        
        EditorGUILayout.EndScrollView();
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.NotificationSystem;
using Base;
using Employees;
using GameSystem;
using Missions;
using Pathfinding;
using Team;
using UnityEngine;
using Wth.ModApi.Items;

namespace SaveGame
{
    /// <summary>
    /// This class is responsible for loading and saving a game.
    /// </summary>
    public class SaveGameSystem : Singleton<SaveGameSystem>
    {
        public const string DEFAULT_SAVE_GAME_NAME = "savegame";

        public EmployeeManager EmployeeManager;
        public MissionManager MissionManager;
        public Bank bank;
        public NotificationCenter NotificationCenter;

        private MainSaveGame currentSaveGame;
        private int TutorialStage = 0;

        private void Awake()
        {
            if (!GameSettings.NewGame && DoesSaveGameExist(DEFAULT_SAVE_GAME_NAME))
            {
                currentSaveGame = LoadGame(DEFAULT_SAVE_GAME_NAME);

                if (currentSaveGame == null)
                {
                    GameSettings.NewGame = true;
                }
            }
        }

        /// <summary>
        /// Returns the savegame object that was loaded from disk.
        /// May be null.
        /// </summary>
        /// <returns>Current deserialized savegame</returns>
        public MainSaveGame GetCurrentSaveGame()
        {
            return currentSaveGame;
        }

        /// <summary>
        /// Create a new savegame object.
        /// This method gathers all data and prepares the object
        /// for storage.
        /// </summary>
        /// <param name="saveGameName"></param>
        /// <returns></returns>
        private MainSaveGame CreateNewSaveGame(string saveGameName)
        {
            MainSaveGame saveGame = new MainSaveGame();
            saveGame.name = saveGameName;
            saveGame.saveDate = DateTime.Now;
            saveGame.employeeManagerData = EmployeeManager.GetData();
            saveGame.missionManagerData = MissionManager.GetData();
            saveGame.teamManagerData = TeamManager.Instance.GetData();
            saveGame.gameTime = GameTime.GameTime.Instance.GetData();
            saveGame.NotificationCenterData = NotificationCenter.Instance.GetData();
            FillTileMapData(saveGame);
            saveGame.balance = bank.Balance;
            saveGame.Difficulty = SettingsManager.GetDifficulty();
            saveGame.TutorialStage = TutorialStage;
            return saveGame;
        }

        /// <summary>
        /// Save the current game to disk.
        /// </summary>
        /// <param name="saveGameName"></param>
        /// <returns></returns>
        public bool SaveGame(string saveGameName)
        {
            var saveGame = CreateNewSaveGame(saveGameName);
            return SaveGameToFile(saveGame);
        }

        /// <summary>
        /// Save a saveGame to disk.
        /// </summary>
        /// <param name="saveGame">SaveGame Object to save.</param>
        /// <returns>Successfully saved?</returns>
        public static bool SaveGameToFile(MainSaveGame saveGame)
        {
            BinaryFormatter formatter = CreateBinaryFormatter();
            using (var stream = new FileStream(GetSavePath(saveGame.name), FileMode.Create))
            {
                try
                {
                    formatter.Serialize(stream, saveGame);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Load a savegame from disk.
        /// </summary>
        /// <param name="saveGameName">Name of the savegame.</param>
        /// <returns>Successfully loaded?</returns>
        public MainSaveGame LoadGame(string saveGameName)
        {
            if (!DoesSaveGameExist(saveGameName))
            {
                return null;
            }

            var formatter = CreateBinaryFormatter();
            MainSaveGame saveGame;

            using (var stream = new FileStream(GetSavePath(saveGameName), FileMode.Open))
            {
                try
                {
                    saveGame = formatter.Deserialize(stream) as MainSaveGame;
                    if (saveGame != null) TutorialStage = saveGame.TutorialStage;
                }
                catch (Exception e)
                {
                    Debug.LogError("Error while loading savegame:");
                    Debug.LogError(e);
                    return null;
                }
            }

            //RestoreTileMapData(saveGame);
            return saveGame;
        }

        /// <summary>
        /// Delete a savegame from disk.
        /// </summary>
        /// <param name="name">Name of the savegame.</param>
        /// <returns>Successfully deleted?</returns>
        public bool DeleteSaveGame(string name)
        {
            try
            {
                File.Delete(GetSavePath(name));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Does a savegame with the given name exist?
        /// </summary>
        /// <param name="saveGameName">Savegame name to check for.</param>
        /// <returns>Exists?</returns>
        public static bool DoesSaveGameExist(string saveGameName)
        {
            return File.Exists(GetSavePath(saveGameName));
        }

        /// <summary>
        /// Set the current tutorial stage for the savegame.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetTutorialLevel(int value)
        {
            TutorialStage = value;
        }

        /// <summary>
        /// Create a <see cref="BinaryFormatter"/> instance with necessary binder and surrogates.
        /// </summary>
        /// <returns></returns>
        private static BinaryFormatter CreateBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new VersionDeserializationBinder();
            
            List<Type> types = new List<Type>()
            {
                typeof(EmployeeDefinition),
                typeof(SkillDefinition),
                typeof(ScriptableObject),
                typeof(NameLists),
                typeof(ItemDefinition),
                typeof(MissionDefinition)
            };
        
            // Create a SurrogateSelector
            SurrogateSelector ss = new SurrogateSelector();
            ss.AddSurrogate(typeof(Vector2Int), new StreamingContext(StreamingContextStates.All), new Vector2IntSerializationSurrogate());
            ss.AddSurrogate(typeof(Color32), new StreamingContext(StreamingContextStates.All), new ColorSerializationSurrogate());
            foreach (var type in types)
            {
                ss.AddSurrogate(type, new StreamingContext(StreamingContextStates.All), new ScriptableObjectSerializationSurrogate());
            }
        
            formatter.SurrogateSelector = ss;

            return formatter;
        }
    
        /// <summary>
        /// Create the full path for a given savegame.
        /// </summary>
        /// <param name="saveGameName">Name of the savegame.</param>
        /// <returns>Path of the savegame.</returns>
        private static string GetSavePath(string saveGameName)
        {
            return Path.Combine(Application.persistentDataPath, saveGameName + ".sav");
        }

        /// <summary>
        /// Fill a SaveGameObject with NodeMap data.
        /// </summary>
        /// <param name="saveGame"></param>
        private static void FillTileMapData(MainSaveGame saveGame)
        {
            var go = GameObject.FindWithTag("Pathfinding");
            var grid = go.GetComponent<AGrid>();
        
            saveGame.Tilemap = new NodeData[AGrid.GridSizeX, AGrid.GridSizeY];
        
            for (int x = 0; x < AGrid.GridSizeX; x++)
            {
                for (int y = 0; y < AGrid.GridSizeY; y++)
                {
                    var node = grid.getNode(new Vector2Int(x, y));
                    saveGame.Tilemap[x, y] = node.GetData();
                }
            }
        }

        /// <summary>
        /// Restore tilemap data from a savegame to the gameobjects.
        /// </summary>
        /// <param name="saveGame"></param>
        private static void RestoreTileMapData(MainSaveGame saveGame)
        {
            var go = GameObject.FindWithTag("Pathfinding");
            var grid = go.GetComponent<AGrid>();
        
            for (int x = 0; x < AGrid.GridSizeX; x++)
            {
                for (int y = 0; y < AGrid.GridSizeY; y++)
                {
                    var node = grid.getNode(new Vector2Int(x, y));
                    node.SetData(saveGame.Tilemap[x, y]);
                }
            }
        }
    }
}

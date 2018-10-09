using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Pathfinding;
using UnityEngine;
using Wth.ModApi.Tools;

/// <summary>
/// This class is responsible for loading and saving a game.
/// </summary>
public static class SaveGameSystem
{
    public static MainSaveGame CreateNewSaveGame(string name)
    {
        MainSaveGame saveGame = new MainSaveGame();
        saveGame.name = name;
        saveGame.saveDate = DateTime.Now;
        FillTileMapData(saveGame);

        return saveGame;
    }
    
    /// <summary>
    /// Save a saveGame to disk.
    /// </summary>
    /// <param name="saveGame">SaveGame Object to save.</param>
    /// <returns>Successfully saved?</returns>
    public static bool SaveGame(MainSaveGame saveGame)
    {
        BinaryFormatter formatter = new BinaryFormatter();
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
    /// <param name="name">Name of the savegame.</param>
    /// <returns>Successfully loaded?</returns>
    public static MainSaveGame LoadGame(string name)
    {
        if (!DoesSaveGameExist(name))
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        MainSaveGame saveGame;
   
        using (var stream = new FileStream(GetSavePath(name), FileMode.Open))
        {
            try
            {
                saveGame = formatter.Deserialize(stream) as MainSaveGame;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        RestoreTileMapData(saveGame);
        return saveGame;
    }

    /// <summary>
    /// Delete a savegame from disk.
    /// </summary>
    /// <param name="name">Name of the savegame.</param>
    /// <returns>Successfully deleted?</returns>
    public static bool DeleteSaveGame(string name)
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
    /// <param name="name">Savegame name to check for.</param>
    /// <returns>Exists?</returns>
    public static bool DoesSaveGameExist(string name)
    {
        return File.Exists(GetSavePath(name));
    }

    /// <summary>
    /// Create the full path for a given savegame.
    /// </summary>
    /// <param name="name">Name of the savegame.</param>
    /// <returns>Path of the savegame.</returns>
    private static string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name + ".sav");
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

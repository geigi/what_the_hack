using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// This class is responsible for loading and saving a game.
/// </summary>
public static class SaveGameSystem {
    /// <summary>
    /// Save a saveGame to disk.
    /// </summary>
    /// <param name="saveGame">SaveGame Object to save.</param>
    /// <param name="name">Name of the SaveGame.</param>
    /// <returns>Successfully saved?</returns>
	public static bool SaveGame(SaveGame saveGame, string name)
    {
        try
        {
            using (StreamWriter file = File.CreateText(GetSavePath(name)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.Serialize(file, saveGame);
                return true;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }

    /// <summary>
    /// Load a savegame from disk.
    /// </summary>
    /// <param name="name">Name of the savegame.</param>
    /// <returns>Successfully loaded?</returns>
    public static SaveGame LoadGame(string name)
    {
        if (!DoesSaveGameExist(name))
        {
            return null;
        }

        using (var stream = File.OpenText(GetSavePath(name)))
        {
            try
            {
                var serializer = new JsonSerializer();
                SaveGame saveGame = (SaveGame)serializer.Deserialize(stream, typeof(SaveGame));
                return saveGame;
            }
            catch (Exception)
            {
                return null;
            }
        }
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
}

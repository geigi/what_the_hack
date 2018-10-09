using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wth.ModApi.Tools
{
    /// <summary>
    /// This Scriptable Object contains key value pairs for each Scriptable Object used in the game.
    /// This is necessary to save references to scriptable objects in the savegame.
    /// Use the key in your ISerializable implementation to save/load the reference.
    /// The main game and each mod needs an instance of this class.
    /// </summary>
    [CreateAssetMenu(fileName = "SODictionary", menuName = "What_The_Hack ModApi/ScriptableObject Dictionary", order = 1)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScriptableObjectDictionary: ScriptableObject
    {
        /// <summary>
        /// This class is a simple data container for an scriptable object dictionary.
        /// </summary>
        [Serializable]
        public class ScriptableObjectEntry
        {
            /// <summary>
            /// Key for a given object.
            /// </summary>
            public string Key;
            
            public ScriptableObject ScriptableObject;

            public ScriptableObjectEntry(string key, ScriptableObject obj)
            {
                Key = key;
                ScriptableObject = obj;
            }
        }
        
        /// <summary>
        /// Dictionary which contains the key value pairs.
        /// </summary>
        [Header("Scriptable Object Dictionary")]
        [Tooltip("Each Scriptable Object that needs to be referenced in a save game needs to be included here.")]
        public List<ScriptableObjectEntry> Dictionary;
        
        /// <summary>
        /// Add or update a key object pair.
        /// This method first tries to find the object by key then by the object itself. It then updates the key/object
        /// depending on how the entry was found. Otherwise it creates a new entry.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddUpdate(string key, ScriptableObject obj)
        {
            var entry = Dictionary.FirstOrDefault(i => i.Key == key);
            if (entry != null)
            {
                if (obj == entry.ScriptableObject)
                {
                    return;
                }
                else if (obj != null)
                {
                    entry.ScriptableObject = obj;
                }
                else
                {
                    Debug.LogWarning("SODictionary: ScriptableObject object with key " + key + " is null. Not saving.");
                }

                return;
            }
            
            entry = Dictionary.FirstOrDefault(i => i.ScriptableObject == obj);
            if (entry != null)
            {
                entry.Key = key;
            }
            else
            {
                Dictionary.Add(new ScriptableObjectEntry(key, obj));
            }
        }
        
        /// <summary>
        /// Delete an entry by key.
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            Dictionary.RemoveAll(i => i.Key == key);
        }

        /// <summary>
        /// Delete an entry by object.
        /// </summary>
        /// <param name="obj"></param>
        public void Delete(ScriptableObject obj)
        {
            Dictionary.RemoveAll(i => i.ScriptableObject == obj);
        }

        /// <summary>
        /// Find the key to a given object in this dictionary.
        /// Returns null if not found.
        /// </summary>
        /// <param name="obj">Object to find</param>
        /// <returns></returns>
        public string GetKey(ScriptableObject obj)
        {
            return Dictionary.First(x => x.ScriptableObject == obj).Key;
        }

        /// <summary>
        /// Find the object to a given key in this dictionary.
        /// Returns null if not found.
        /// </summary>
        /// <param name="key">Key to find</param>
        /// <returns></returns>
        public ScriptableObject GetObject(string key)
        {
            return Dictionary.First(x => x.Key == key).ScriptableObject;
        }
    }
}
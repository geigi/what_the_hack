using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using ModTool;
using UnityEngine;

namespace Wth.ModApi.Tools
{
    /// <summary>
    /// This class contains methods needed to serialize and deserialize scriptable object references.
    /// Note: This is a singleton.
    /// </summary>
    public sealed class ScriptableObjectManager: Singleton<ScriptableObjectManager>
    {
        private ScriptableObjectDictionary MainDictionary;
        private ScriptableObjectDictionary ModDictionary;
        
        void Awake()
        {
            MainDictionary = Resources.LoadAll<ScriptableObjectDictionary>("").First();
            RefreshModDictionary();
            ModManager.instance.ModLoaded += OnModLoaded;
            ModManager.instance.ModUnloaded += OnModUnloaded;
        }

        /// <summary>
        /// Refreshes the <see cref="ScriptableObjectDictionary"/> reference of the currently loaded mod.
        /// </summary>
        public void RefreshModDictionary()
        {
            foreach (var mod in ModManager.instance.mods)
            {
                if (mod.loadState == ResourceLoadState.Loaded)
                {
                    ModDictionary = mod.GetAsset<ModInfo>("modinfo").ScriptableObjectDictionary;
                    break;
                }
            }
            Debug.Log("No Mod SO dictionary found.");
        }
        
        /// <summary>
        /// Get the key to a given ScriptableObject.
        /// First tries to find the object in <see cref="MainDictionary"/> then in <see cref="ModDictionary"/> if loaded.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetKey(ScriptableObject obj)
        {
            if (obj == null)
                return "";
            
            string key = MainDictionary.GetKey(obj);
            if (String.IsNullOrEmpty(key) && ModDictionary != null)
                key = ModDictionary.GetKey(obj);

            return key;
        }

        /// <summary>
        /// Get the ScriptableObject to a given key.
        /// First tries to find the key in <see cref="MainDictionary"/> then in <see cref="ModDictionary"/> if loaded.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ScriptableObject GetObject(string key)
        {
            if (String.IsNullOrEmpty(key))
                return null;
            
            ScriptableObject scriptableObject = MainDictionary.GetObject(key);
            if (scriptableObject == null && ModDictionary != null)
                scriptableObject = ModDictionary.GetObject(key);

            return scriptableObject;
        }

        /// <summary>
        /// Convert a list of ScriptableObjects to it's reference keys.
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public List<string> GetKeys(IEnumerable<ScriptableObject> objs)
        {
            List<string> keys = new List<string>();
            foreach (var obj in objs)
            {
                var key = GetKey(obj);
                if (obj != null) { keys.Add(key); }
            }

            return keys;
        }
        
        /// <summary>
        /// Converts a list of keys to the corresponding ScriptableObjects.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<ScriptableObject> GetObjects(List<string> keys)
        {
            List<ScriptableObject> objs = new List<ScriptableObject>();
            foreach (var key in keys)
            {
                var obj = GetObject(key);
                if (obj != null) { objs.Add(obj); }
            }

            return objs;
        }
        
        private void OnModLoaded(Mod mod) {
            ModDictionary = mod.GetAsset<ModInfo>("modinfo").ScriptableObjectDictionary;
        }

        private void OnModUnloaded(Mod mod)
        {
            ModDictionary = null;
        }
    }
}
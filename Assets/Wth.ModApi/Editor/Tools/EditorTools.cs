using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Wth.ModApi.Tools;

namespace Wth.ModApi.Editor.Tools
{
    /// <summary>
    /// This class contains some useful generic editor tools.
    /// </summary>
    public static class EditorTools
    {
        /// <summary>
        /// Finds or creates the scriptable object dictionary for this mod.
        /// </summary>
        /// <returns></returns>
        public static ScriptableObjectDictionary GetScriptableObjectDictionary()
        {
            var assetPath = "Assets/Resources/SODictionary.asset";
            
            var dir = Path.GetDirectoryName(assetPath);
            if (dir != null)
            {
                Directory.CreateDirectory(dir);
            }
            
            ScriptableObjectDictionary asset = AssetDatabase.LoadAssetAtPath<ScriptableObjectDictionary>(assetPath);
            if (asset == null)
            {
                asset = Create<ScriptableObjectDictionary>(assetPath);
            }

            return asset;
        }

        /// <summary>
        /// Creates an Asset of type T and returns it.
        /// </summary>
        /// <typeparam name="T">The Type of Asset that should be created.</typeparam>
        /// <param name="path">The path where the Asset should be created.</param>
        /// <returns>The created asset.</returns>
        public static T Create<T>(string path) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}
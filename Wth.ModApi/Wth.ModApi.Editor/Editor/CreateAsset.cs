using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Wth.ModApi.Editor.Editor
{
    /// <summary>
    /// Utility Class for creating an Asset of a specific type T.
    /// </summary>
    class CreateAsset : MonoBehaviour
    {
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

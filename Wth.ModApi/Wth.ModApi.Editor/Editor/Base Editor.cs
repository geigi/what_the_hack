using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Wth.ModApi.Editor.Tools;
using Wth.ModApi.Tools;
using Object = System.Object;

namespace Wth.ModApi.Editor.Editor
{
    /// <summary>
    /// A Base Editor, for Editing a Scriptable Object.
    /// </summary>
    /// <typeparam name="T">The Type of ScriptableObject that is to be edited.</typeparam>
    public abstract class BaseEditor<T> : EditorWindow where T : ScriptableObject
    {

        public abstract void OnGUI();

        /// <summary>
        /// The Asset which is being shown by the Editor.
        /// </summary>
        public T asset;

        /// <summary>
        /// The name of this Asset. This will be used for GUI and path saving.
        /// </summary>
        public string assetName;

        /// <summary>
        /// If this editor handles scriptable objects that need references in savegames, it needs to keep track of those
        /// in a <see cref="ScriptableObjectDictionary"/>.
        /// Set this in the constructor of the inheritor if needed.
        /// </summary>
        public bool NeedsDictionary = false;

        /// <summary>
        /// The current Item of the SkillSet.
        /// </summary>
        protected int viewIndex = 1;
        
        protected void OnEnable()
        {
            if (EditorPrefs.HasKey("AssetPath" + assetName))
            {
                string objectPath = EditorPrefs.GetString("AssetPath" + assetName);
                asset = AssetDatabase.LoadAssetAtPath(objectPath, typeof(T)) as T;
            }
        }
        
        /// <summary>
        /// Creates a new Asset of type T.
        /// </summary>
        /// <param name="assetPath">The Path the new Asset should be created.</param>
        protected virtual void CreateNewAsset(string assetPath)
        {
            Debug.Log("Creating New Asset " + assetPath);
            // There is no overwrite protection here!
            viewIndex = 1;
            
            // Create directories if necessary
            var dir = Path.GetDirectoryName(assetPath);
            if (dir != null)
            {
                Directory.CreateDirectory(dir);
            }
            else
            {
                Debug.LogError("Invalid assetPath: " + assetPath);
                return;
            }
            
            asset = EditorTools.Create<T>(assetPath);
            if (asset)
            {
                string relPath = AssetDatabase.GetAssetPath(asset);
                EditorPrefs.SetString("AssetPath" + assetName, relPath);
            }
        }

        /// <summary>
        /// Opens an Asset at a user defined path.
        /// </summary>
        /// <param name="objectName">The Name of this edited Scriptable Object.</param>
        protected virtual void OpenAsset(string objectName)
        {
            string absPath = EditorUtility.OpenFilePanel("Select " + objectName, "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                T asset = AssetDatabase.LoadAssetAtPath(relPath, typeof(T)) as T;
                Debug.Log("Loaded Asset " + asset);
                if (asset)
                {
                    this.asset = asset;
                    EditorPrefs.SetString("AssetPath" + assetName, relPath);
                }
            }
        }

        /// <summary>
        /// Creates a Button to show, open and create a new Scriptable Object of type T.
        /// </summary>
        /// <param name="assetPath">The Path a new Asset should be created </param>
        /// <param name="objectName">The name of this edited Scriptable Object (Usually the name of the Class)</param>
        protected virtual void CreateListButtons(string assetPath, string objectName)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(assetName + " Creator", EditorStyles.boldLabel);
            if (this.asset != null)
            {
                if (GUILayout.Button("Show " + objectName))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = this.asset;
                }
            }
            if (GUILayout.Button("Open " + objectName))
            {
                this.OpenAsset(objectName);
            }
            if (GUILayout.Button("New " + objectName))
            {
                this.CreateNewAsset(assetPath);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = this.asset;
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Creates the navigation, to navigate the set of Scriptable Objects.
        /// Only for Scriptable Objects, which holds a List of other Objects.
        /// </summary>
        /// <param name="numItems">The current number of items in the List.</param>
        protected virtual void CreateAssetNavigation(int numItems)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }

            GUILayout.Space(20);

            viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current " + assetName, viewIndex, GUILayout.ExpandWidth(false)), 1, numItems);
            GUILayout.Space(5);
            EditorGUILayout.LabelField("of   " + numItems.ToString() + "   " + assetName + "s", "", GUILayout.ExpandWidth(false));

            GUILayout.Space(20);

            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < numItems)
                {
                    viewIndex++;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Save all dirty assets to disk and update the scriptable object dictionary.
        /// </summary>
        protected void SaveAssets()
        {
            AssetDatabase.SaveAssets();
            UpdateScriptableObjectDictionary();
        }

        /// <summary>
        /// Update the scriptable object dictionary.
        /// Uses the asset path as key.
        /// </summary>
        private void UpdateScriptableObjectDictionary()
        {
            if (asset == null)
                return;

            var dictionary = EditorTools.GetScriptableObjectDictionary();
            
            foreach (var item in GetList())
            {
                dictionary.AddUpdate(AssetDatabase.GetAssetPath(item), item);
            }
            
            EditorUtility.SetDirty(dictionary);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Return the list of this asset.
        /// Needs to be overwritten by the derived class.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual List<ScriptableObject> GetList()
        {
            throw new NotImplementedException("This method needs to be overwritten by its inheritor.");
        }
    }
}

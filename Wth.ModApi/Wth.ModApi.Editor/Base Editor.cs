using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Wth.ModApi.Editor.Tools;
using Wth.ModApi.Tools;

namespace Wth.ModApi.Editor
{
    /// <summary>
    /// A Base Editor, for Editing a Unity Object.
    /// </summary>
    /// <typeparam name="T">The Type of ScriptableObject that is to be edited.</typeparam>
    public abstract class BaseEditor<T> : EditorWindow where T : UnityEngine.Object
    {
	    /// <summary>
	    /// This method constructs the editor GUI.
	    /// </summary>
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
								/// If this Editor referes to a Scriptable Object, which should be able create new assets and save these assets
								/// via Json, this variable should be set to true.
								/// </summary>
								public bool JsonSerializable = false;

        /// <summary>
        /// The current Item of the SkillSet.
        /// </summary>
        protected int viewIndex = 1;
        
	    /// <summary>
	    /// This method gets called when the GUI is enabled.
	    /// </summary>
        protected virtual void OnEnable()
        {
            if (EditorPrefs.HasKey("AssetPath" + assetName))
            {
                string objectPath = EditorPrefs.GetString("AssetPath" + assetName);
                asset = AssetDatabase.LoadAssetAtPath(objectPath, typeof(T)) as T;
            }
        }

	    /// <summary>
	    /// Creates a new asset at the given path.
	    /// </summary>
	    /// <param name="assetPath">Path where asset will be saved</param>
		public abstract void CreateNewAsset(string assetPath);

        /// <summary>
        /// Creates a new Asset of type T.
        /// </summary>
        /// <param name="assetPath">The Path the new Asset should be created.</param>
        protected virtual void CreateDirectories(string assetPath)
        {
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
        }

        /// <summary>
        /// Opens an Asset at a user defined path.
        /// </summary>
        /// <param name="objectName">The Name of this edited Scriptable Object.</param>
        protected virtual string OpenAsset(string objectName)
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
																return relPath;
            }
												return absPath;
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
												if(JsonSerializable && GUILayout.Button("New " + objectName + " from JSON"))
												{
																string path = EditorUtility.OpenFilePanel("Choose JSON file", "", "json");
																if (File.Exists(path))
																{
																				try
																				{
																								string json = File.ReadAllText(path);
																								JsonUtility.FromJsonOverwrite(json, asset);
																				} catch (Exception e)
																				{
																					Debug.Log(e);
																					ShowNotification(new GUIContent("Could not parse JSON correctly"));
																				}
																}
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
								/// If JsonSerializable is set to true, this method will display a button to save the Scriptable Object to a JSON file.
								/// If JsonSerializable is set to false, this method will do nothing.
								/// </summary>
								protected void SaveToJSON()
								{
												GUILayout.BeginHorizontal();
												GUILayout.FlexibleSpace();
												if (JsonSerializable && GUILayout.Button("Save To JSON", GUILayout.ExpandWidth(false)))
												{
																string path = EditorUtility.SaveFilePanel("Save Names as JSON", "", assetName + ".json", "json");
																if (path.Length != 0)
																{
																				try
																				{
																								string json = JsonUtility.ToJson(asset);
																								File.WriteAllText(path, json);
																				} catch (Exception e)
																				{
																					Debug.Log(e);
																					ShowNotification(new GUIContent("Could not Save Object to JSON"));
																				}
																}
												}
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

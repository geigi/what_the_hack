using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Wth.ModApi.Editor.Tools;
using Wth.ModApi.Names;

namespace Wth.ModApi.Editor.Names
{
    /// <summary>
    /// Editor Window for editing names.
    /// </summary>
    class NameEditor : BaseEditor<NameLists>
    {
        /// <summary>
        /// The current list, which is being modified.
        /// </summary>
        List<string> currentList;

        /// <summary>
        /// The name of the current list.
        /// </summary>
        PersonNames currentPersonNamePersonName = PersonNames.MaleFirstName;

        private int dropdownSelected = 0;

        private static readonly string[] dropdownList = new string[] 
        {
            "Male First Names",
            "Female First Names",
            "Last Names",
            "Companies",
            "Password Applications",
            "Universities",
            "Web Services",
            "Software",
            "Towns",
            "Countries",
            "Institutions"
        };

        private List<string>[] dropdownStorage;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NameEditor()
        {
            assetName = "Name";
            NeedsDictionary = false;
            JsonSerializable = true;
        }

        /// <summary>
        /// Initializes the window.
        /// </summary>
        [MenuItem("Tools/What_The_Hack ModApi/Name Editor", priority = 60)]
        static void Init()
        {
            GetWindow(typeof(NameEditor), false, "Name Editor");
        }

        /// <summary>
        /// Draws the GUI.
        /// </summary>
        public override void OnGUI()
        {
            base.CreateListButtons("Assets/Data/Names/Names.asset", "Name List");
            GUILayout.Space(20);

            if (asset)
            {
                dropdownStorage = new []
                {
                    asset.firstNamesMale,
                    asset.firstNamesFemale,
                    asset.lastNames,
                    asset.companyNames,
                    asset.passwordApplications,
                    asset.universities,
                    asset.webServices,
                    asset.software,
                    asset.towns,
                    asset.countries,
                    asset.institutions
                };
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                asset.UseExclusively = EditorGUILayout.Toggle("Use list exclusively", asset.UseExclusively);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                GUILayout.Space(10);
                
                // Make dropdown menu.
                dropdownSelected = EditorGUILayout.Popup("Current List:", dropdownSelected, dropdownList);
                currentList = dropdownStorage[dropdownSelected];

                base.CreateAssetNavigation(currentList.Count);
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Name", GUILayout.ExpandWidth(false)))
                    AddItem();
                if (GUILayout.Button("Delete Name", GUILayout.ExpandWidth(false)))
                    DeleteItem();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                if (currentList.Count > 0)
                {
                    GUILayout.Space(5);
                    currentList[viewIndex - 1] = EditorGUILayout.TextField("Name: ", currentList[viewIndex - 1]);

                    GUILayout.Space(5);
                    if (GUILayout.Button("Save"))
                        AssetDatabase.SaveAssets();

                    GUILayout.Space(10);
                    SaveToJSON();
                }
                else
                {
                    viewIndex = 0;
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(asset);
            }
        }

        /// <summary>
        /// Creates a new asset at the specified path.
        /// </summary>
        /// <param name="assetPath">Path the asset should be created.</param>
        public override void CreateNewAsset(string assetPath)
        {
            base.CreateDirectories(assetPath);
            asset = EditorTools.Create<NameLists>(assetPath);

            if (asset)
            {
                string relPath = AssetDatabase.GetAssetPath(asset);
                EditorPrefs.SetString("AssetPath" + assetName, relPath);
            }
        }

        /// <summary>
        /// Adds a new name to the current list.
        /// </summary>
        void AddItem()
        {
            currentList.Add("new name");
            viewIndex = currentList.Count();
        }

        /// <summary>
        /// Deletes the name from the list.
        /// </summary>
        void DeleteItem()
        {
            viewIndex -= 1;
            currentList.RemoveAt(viewIndex);
            viewIndex = currentList.Count;
        }
    }
}
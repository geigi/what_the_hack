using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using Wth.ModApi.Editor.Tools;
using Wth.ModApi.Editor;
using Object = System.Object;

namespace Wth.ModApi.Editor.Employees
{
    /// <summary>
    /// This class represents an editor for <see cref="EmployeeDefinition"/>.
    /// </summary>
    public class EmployeeEditor : Editor.BaseEditor<EmployeeList>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EmployeeEditor()
        {
            assetName = "Employee";
            NeedsDictionary = true;
        }

        private Vector2[] scrollPositions = new Vector2[2];

        [MenuItem("Tools/What_The_Hack ModApi/Employee Creator", priority = 42)]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(EmployeeEditor), false, "Employee Creator");
        }

        /// <summary>
        /// Create the UI.
        /// </summary>
        public override void OnGUI()
        {
            CreateListButtons("Assets/Data/Employees/EmployeeList.asset", "Employee List");
            GUILayout.Space(20);

            if (asset != null)
            {
                if (asset.employeeList == null)
                    asset.employeeList = new List<EmployeeDefinition>();
                CreateAssetNavigation(asset.employeeList.Count);
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Employee", GUILayout.ExpandWidth(false)))
                {
                    AddItem();
                }

                if (GUILayout.Button("Delete Employee", GUILayout.ExpandWidth(false)))
                {
                    viewIndex -= 1;
                    DeleteItem(viewIndex);
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                CreateEmployeeGui();

                if (GUILayout.Button("Save Employees"))
                {
                    SaveAssets();
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(asset);

                if (asset.employeeList != null)
                {
                    foreach (var employee in base.asset.employeeList)
                    {
                        EditorUtility.SetDirty(employee);
                    }
                }
            }
        }

        #region GUI

        private void CreateEmployeeGui()
        {
            if (asset.employeeList.Count > 0)
            {
                asset.employeeList[viewIndex - 1].EmployeeName = EditorGUILayout.TextField("Employee Name",
                    asset.employeeList[viewIndex - 1].EmployeeName as string);

                GUILayout.BeginHorizontal();
                asset.employeeList[viewIndex - 1].IdleAnimation = EditorGUILayout.ObjectField("Idle Animation",
                    asset.employeeList[viewIndex - 1].IdleAnimation, typeof(AnimationClip), false) as AnimationClip;
                if (GUILayout.Button("Create Idle Animation", GUILayout.MaxWidth(200)))
                {
                    OpenAnimationEditor("Basic Idle Animation");
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                asset.employeeList[viewIndex - 1].WalkingAnimation = EditorGUILayout.ObjectField("Walking Animation",
                    asset.employeeList[viewIndex - 1].WalkingAnimation, typeof(AnimationClip), false) as AnimationClip;
                if (GUILayout.Button("Create Walking Animation", GUILayout.MaxWidth(200)))
                {
                    OpenAnimationEditor("Basic Walking Animation");
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                asset.employeeList[viewIndex - 1].WorkingAnimation = EditorGUILayout.ObjectField("Working Animation",
                    asset.employeeList[viewIndex - 1].WorkingAnimation, typeof(AnimationClip), false) as AnimationClip;
                if (GUILayout.Button("Create Working Animation", GUILayout.MaxWidth(200)))
                {
                    OpenAnimationEditor("Basic Working Animation");
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                asset.employeeList[viewIndex - 1].image = EditorGUILayout.ObjectField("Image",
                    asset.employeeList[viewIndex - 1].image, typeof(Sprite), false) as Sprite;

                GUILayout.Space(10);
                    
                GUILayout.BeginHorizontal();
                asset.employeeList[viewIndex - 1].EmployeeSpecials[0] = EditorGUILayout.TextField("Special 1",
                    asset.employeeList[viewIndex - 1].EmployeeSpecials[0]);
                GUILayout.Space(10);
                asset.employeeList[viewIndex - 1].EmployeeSpecials[1] = EditorGUILayout.TextField("Special 2",
                    asset.employeeList[viewIndex - 1].EmployeeSpecials[1]);
                GUILayout.EndHorizontal();
                
                GUILayout.Space(10);

                asset.employeeList[viewIndex - 1].SpawnLikelihood = EditorGUILayout.Slider("Spawn Likelihood",
                    asset.employeeList[viewIndex - 1].SpawnLikelihood, 0f, 1f);

                GUILayout.Space(10);

                asset.employeeList[viewIndex - 1].Recurring =
                    GUILayout.Toggle(asset.employeeList[viewIndex - 1].Recurring, "Recurring Employee");

                GUILayout.Space(10);

                asset.employeeList[viewIndex - 1].StartEmployee = EditorGUILayout.Toggle("Start Employee",
                    asset.employeeList[viewIndex - 1].StartEmployee);
                
                GUILayout.Space(10);

                asset.employeeList[viewIndex - 1].SpawnWhenAllConditionsAreMet = GUILayout.Toggle(
                    asset.employeeList[viewIndex - 1].SpawnWhenAllConditionsAreMet, "Specific spawn conditions");

                GUILayout.Space(10);

                if (asset.employeeList[viewIndex - 1].SpawnWhenAllConditionsAreMet && !asset.employeeList[viewIndex - 1].StartEmployee)
                {
                    asset.employeeList[viewIndex - 1].NumberOfDaysTillEmpCanSpawn = EditorGUILayout.IntField(
                        "Passed Days",
                        asset.employeeList[viewIndex - 1].NumberOfDaysTillEmpCanSpawn);

                    asset.employeeList[viewIndex - 1].GameProgress =
                        EditorGUILayout.IntField("GameProgress reached",
                            asset.employeeList[viewIndex - 1].GameProgress);

                    GUILayout.Space(10);

                    GUILayout.Label("Missions to be successfully completed:");
                    GUILayout.Space(3);
                    scrollPositions[0] = ArrayField(ref asset.employeeList[viewIndex - 1].MissionSucceeded, "Mission", scrollPositions[0], false, 5);

                    GUILayout.Space(10);
                    
                    GUILayout.Label("Items to be bought:");
                    GUILayout.Space(3);
                    scrollPositions[1] = ArrayField(ref asset.employeeList[viewIndex - 1].ItemsBought, "Item", scrollPositions[1], false, 5);
                }
                GUILayout.Space(10);
            }
            else
            {
                GUILayout.Label("This Employee List is Empty.");
            }
        }

        #endregion

        /// <summary>
        /// Create a new <see cref="EmployeeList"/> at a given path.
        /// </summary>
        /// <param name="assetPath">Path where <see cref="EmployeeList"/> will be saved</param>
        public override void CreateNewAsset(string assetPath)
        {
            base.CreateDirectories(assetPath);
            asset = EditorTools.Create<EmployeeList>(assetPath);
            if (asset)
            {
                string relPath = AssetDatabase.GetAssetPath(asset);
                EditorPrefs.SetString("AssetPath" + assetName, relPath);
            }
        }

        void AddItem()
        {
            var asset = ScriptableObject.CreateInstance<EmployeeDefinition>();

            AssetDatabase.CreateAsset(asset,
                "Assets/Data/Employees/" + Guid.NewGuid() + ".asset");
            asset.EmployeeName = "Max Mustermann";
            this.asset.employeeList.Add(asset);
            viewIndex = this.asset.employeeList.Count;
            SaveAssets();
        }

        void DeleteItem(int index)
        {
            var item = asset.employeeList[index];
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
            asset.employeeList.RemoveAt(index);
            viewIndex = asset.employeeList.Count;

            var dictionary = EditorTools.GetScriptableObjectDictionary();
            dictionary.Delete(item);
            EditorUtility.SetDirty(dictionary);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Get a list of all custom <see cref="EmployeeDefinition"/> in this list.
        /// This is used for the ScriptableObject Key Value dictionary used for serialization.
        /// </summary>
        /// <returns>List of <see cref="ScriptableObject"/> which are <see cref="EmployeeDefinition"/></returns>
        protected override List<ScriptableObject> GetList()
        {
            return asset.employeeList.Cast<ScriptableObject>().ToList();
        }

        void OpenAnimationEditor(string name)
        {
            AnimationEditor window = (AnimationEditor) EditorWindow.GetWindow(typeof(AnimationEditor));
            window.Show();
            window.CreateNewAsset("Asset/Animations");
            window.emp = asset.employeeList[viewIndex - 1];
            window.SetAnimationName(name);
            window.animationString = name;
        }
    }
}
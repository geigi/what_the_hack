using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Wth.ModApi.Editor.Tools;
using Wth.ModApi.Employees;

namespace Wth.ModApi.Editor.Employees {
public class EmployeeEditor : Editor.BaseEditor<EmployeeList>
{
    public EmployeeEditor()
    {
        assetName = "Employee";
        NeedsDictionary = true;
    }
    
    [MenuItem("Tools/What_The_Hack ModApi/Employee Creator")]
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
            asset.employeeList[viewIndex - 1].EmployeeName = EditorGUILayout.TextField("Employee Name", asset.employeeList[viewIndex - 1].EmployeeName as string);
			
            GUILayout.BeginHorizontal();
			asset.employeeList[viewIndex - 1].IdleAnimation = EditorGUILayout.ObjectField("Idle Animation", asset.employeeList[viewIndex - 1].IdleAnimation, typeof(AnimationClip), false) as AnimationClip;
			if (GUILayout.Button("Create Idle Animation", GUILayout.MaxWidth(200))) {
											OpenAnimationEditor("Basic Idle Animation");
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			asset.employeeList[viewIndex - 1].WalkingAnimation = EditorGUILayout.ObjectField("Walking Animation", asset.employeeList[viewIndex - 1].WalkingAnimation, typeof(AnimationClip), false) as AnimationClip;
			if (GUILayout.Button("Create Walking Animation", GUILayout.MaxWidth(200)))
			{
							OpenAnimationEditor("Basic Walking Animation");
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			asset.employeeList[viewIndex - 1].WorkingAnimation = EditorGUILayout.ObjectField("Working Animation", asset.employeeList[viewIndex - 1].WorkingAnimation, typeof(AnimationClip), false) as AnimationClip;
			if (GUILayout.Button("Create Working Animation", GUILayout.MaxWidth(200)))
			{
			    OpenAnimationEditor("Basic Working Animation");
			}
			GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            asset.employeeList[viewIndex - 1].Level = EditorGUILayout.IntField("Level", asset.employeeList[viewIndex - 1].Level, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

        }
        else
        {
            GUILayout.Label("This Employee List is Empty.");
        }
    }

    #endregion

    void AddItem()
    {
        var asset = ScriptableObject.CreateInstance<EmployeeDefinition>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Employees/Employee " + this.asset.employeeList.Count + ".asset");
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

    protected override List<ScriptableObject> GetList()
    {
        return asset.employeeList.Cast<ScriptableObject>().ToList();
    }

				void OpenAnimationEditor(string name)
				{
								AnimationEditor window = (AnimationEditor)EditorWindow.GetWindow(typeof(AnimationEditor));
								window.animationName = name;
								window.Show();
								window.CreateNewAnimation();
				}
}
}

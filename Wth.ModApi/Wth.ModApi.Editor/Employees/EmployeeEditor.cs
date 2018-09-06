using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Wth.ModApi.Editor {
public class EmployeeEditor : EditorWindow
{

    public EmployeeList employeeList;
    private int viewIndex = 1;

    [MenuItem("Window/Employee Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(EmployeeEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            employeeList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(EmployeeList)) as EmployeeList;
        }

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Employee Editor", EditorStyles.boldLabel);
        if (employeeList != null)
        {
            if (GUILayout.Button("Show Employee List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = employeeList;
            }
        }
        if (GUILayout.Button("Open Employee List"))
        {
            OpenItemList();
        }
        if (GUILayout.Button("New Employee List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = employeeList;
        }
        GUILayout.EndHorizontal();

        if (employeeList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Employee List", GUILayout.ExpandWidth(false)))
            {
                CreateNewItemList();
            }
            if (GUILayout.Button("Open Existing Employee List", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (employeeList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < employeeList.employeeList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Employee", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Employee", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (employeeList.employeeList == null)
                Debug.Log("wtf");
            if (employeeList.employeeList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Employee", viewIndex, GUILayout.ExpandWidth(false)), 1, employeeList.employeeList.Count);
                //Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
                EditorGUILayout.LabelField("of   " + employeeList.employeeList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                employeeList.employeeList[viewIndex - 1].employeeName = EditorGUILayout.TextField("Employee Name", employeeList.employeeList[viewIndex - 1].employeeName as string);
                employeeList.employeeList[viewIndex - 1].idleAnimation = EditorGUILayout.ObjectField("Idle Animation", employeeList.employeeList[viewIndex - 1].idleAnimation, typeof(AnimationClip), false) as AnimationClip;
                employeeList.employeeList[viewIndex - 1].walkingAnimation = EditorGUILayout.ObjectField("Walking Animation", employeeList.employeeList[viewIndex - 1].walkingAnimation, typeof(AnimationClip), false) as AnimationClip;
                employeeList.employeeList[viewIndex - 1].workingAnimation = EditorGUILayout.ObjectField("Working Animation", employeeList.employeeList[viewIndex - 1].workingAnimation, typeof(AnimationClip), false) as AnimationClip;

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                employeeList.employeeList[viewIndex - 1].level = EditorGUILayout.IntField("Level", employeeList.employeeList[viewIndex - 1].level, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

            }
            else
            {
                GUILayout.Label("This Employee List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(employeeList);
        }
    }

    void CreateNewItemList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        employeeList = CreateEmployeeList.Create();
        if (employeeList)
        {
            employeeList.employeeList = new List<EmployeeData>();
            string relPath = AssetDatabase.GetAssetPath(employeeList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenItemList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Employee List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            employeeList = AssetDatabase.LoadAssetAtPath(relPath, typeof(EmployeeList)) as EmployeeList;
            if (employeeList.employeeList == null)
                employeeList.employeeList = new List<EmployeeData>();
            if (employeeList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        var asset = ScriptableObject.CreateInstance<EmployeeData>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Employees/Employee " + employeeList.employeeList.Count + ".asset");
        AssetDatabase.SaveAssets();
        asset.employeeName = "Max Mustermann";
        employeeList.employeeList.Add(asset);
        viewIndex = employeeList.employeeList.Count;
    }

    void DeleteItem(int index)
    {
        var item = employeeList.employeeList[index];
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
        employeeList.employeeList.RemoveAt(index);
    }
}
}
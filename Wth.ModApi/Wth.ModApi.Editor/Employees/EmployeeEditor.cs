using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Wth.ModApi.Editor {
public class EmployeeEditor : EditorWindow
{

    public EmployeeList employeeList;
    private int viewIndex = 1;

    [MenuItem("Tools/What_The_Hack ModApi/Employee Creator")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(EmployeeEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("EmployeeListPath"))
        {
            string objectPath = EditorPrefs.GetString("EmployeeListPath");
            employeeList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(EmployeeList)) as EmployeeList;
        }

    }

    void OnGUI()
    {
        CreateListButtons();

        GUILayout.Space(20);

        if (employeeList != null)
        {
            CreateEmployeeNavigation();
            
            GUILayout.Space(10);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Employee", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Employee", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(viewIndex - 1);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            
            CreateEmployeeGui();
            
            if (GUILayout.Button("Save Employees"))
            {
                AssetDatabase.SaveAssets();
            }
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(employeeList);
            foreach (var employee in employeeList.employeeList)
            {
                EditorUtility.SetDirty(employee);
            }
        }
    }

    #region GUI

    private void CreateListButtons()
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
            CreateNewItemList();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = employeeList;
        }
        GUILayout.EndHorizontal();
    }

    private void CreateEmployeeNavigation()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex > 1)
                viewIndex--;
        }
        
        GUILayout.Space(20);
            
        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Employee", viewIndex, GUILayout.ExpandWidth(false)), 1, employeeList.employeeList.Count);
        GUILayout.Space(5);
        EditorGUILayout.LabelField("of   " + employeeList.employeeList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));

        GUILayout.Space(20);
        
        if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex < employeeList.employeeList.Count)
            {
                viewIndex++;
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void CreateEmployeeGui()
    {
        if (employeeList.employeeList.Count > 0)
        {
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

    #endregion

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
            EditorPrefs.SetString("EmployeeListPath", relPath);
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
                EditorPrefs.SetString("EmployeeListPath", relPath);
            }
        }
    }

    void AddItem()
    {
        var asset = ScriptableObject.CreateInstance<EmployeeData>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Employees/Employee " + employeeList.employeeList.Count + ".asset");
        asset.employeeName = "Max Mustermann";
        employeeList.employeeList.Add(asset);
        viewIndex = employeeList.employeeList.Count;
        AssetDatabase.SaveAssets();
    }

    void DeleteItem(int index)
    {
        var item = employeeList.employeeList[index];
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
        employeeList.employeeList.RemoveAt(index);
    }
}
}
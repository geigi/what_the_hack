#if (UNITY_EDITOR)
using UnityEngine;                                                                    
using System.Collections;
using UnityEditor;

namespace Wth.ModApi {
public class CreateEmployeeList
{
    [MenuItem("Assets/Create/Employee List")]
    public static EmployeeList Create()
    {
        EmployeeList asset = ScriptableObject.CreateInstance<EmployeeList>();

        AssetDatabase.CreateAsset(asset, "Assets/EmployeeList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
}
#endif
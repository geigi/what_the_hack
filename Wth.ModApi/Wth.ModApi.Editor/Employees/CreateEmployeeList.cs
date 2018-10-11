using UnityEditor;
using UnityEngine;
using Wth.ModApi.Employees;

namespace Wth.ModApi.Editor.Employees {
public class CreateEmployeeList
{
    public static EmployeeList Create()
    {
        EmployeeList asset = ScriptableObject.CreateInstance<EmployeeList>();

        AssetDatabase.CreateAsset(asset, "Assets/EmployeeList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
}
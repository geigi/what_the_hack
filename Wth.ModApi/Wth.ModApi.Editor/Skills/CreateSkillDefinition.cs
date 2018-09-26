using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Wth.ModApi.Editor
{
    public class CreateSkillDefinition : MonoBehaviour
    {
        public static SkillDefinition Create(string path)
        {
            SkillDefinition asset = ScriptableObject.CreateInstance<SkillDefinition>();

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Wth.ModApi.Editor
{
    public class CreateSkillDefinition : MonoBehaviour
    {

        [MenuItem("Assets/Create/Skill Definition")]
        public static SkillDefinition Create()
        {
            SkillDefinition asset = ScriptableObject.CreateInstance<SkillDefinition>();

            AssetDatabase.CreateAsset(asset, "Assets/SkillDefinition.asset");
            AssetDatabase.SaveAssets();
            return asset;
        }

        public static SkillDefinition Create(string path)
        {
            SkillDefinition asset = ScriptableObject.CreateInstance<SkillDefinition>();

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
        }

    }
}
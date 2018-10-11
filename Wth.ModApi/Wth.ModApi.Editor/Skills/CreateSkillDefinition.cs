using UnityEditor;
using UnityEngine;
using Wth.ModApi.Skills;

namespace Wth.ModApi.Editor.Skills
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
using UnityEditor;
using UnityEngine;

namespace Wth.ModApi.Editor.Skills
{
    /// <summary>
    /// Helper class to create a new <see cref="SkillDefinition"/> asset.
    /// </summary>
    public class CreateSkillDefinition : MonoBehaviour
    {
        /// <summary>
        /// Create a new <see cref="SkillDefinition"/> at the given path.
        /// </summary>
        /// <param name="path">Path where the <see cref="SkillDefinition"/> will be saved</param>
        /// <returns></returns>
        public static SkillDefinition Create(string path)
        {
            SkillDefinition asset = ScriptableObject.CreateInstance<SkillDefinition>();

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}
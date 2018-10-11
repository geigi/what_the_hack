using UnityEditor;
using UnityEngine;
using Wth.ModApi.Skills;

namespace Wth.ModApi.Editor.Skills
{
    public class CreateSkillSet : MonoBehaviour
    {
        public static SkillSet Create()
        {
            SkillSet asset = ScriptableObject.CreateInstance<SkillSet>();

            AssetDatabase.CreateAsset(asset, "Assets/SkillSet.asset");
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}
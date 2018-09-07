
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Wth.ModApi.Editor
{
    public class CreateSkillSet : MonoBehaviour
    {

        [MenuItem("Assets/Create/Skill Set")]
        public static SkillSet Create()
        {
            SkillSet asset = ScriptableObject.CreateInstance<SkillSet>();

            AssetDatabase.CreateAsset(asset, "Assets/SkillSet.asset");
            AssetDatabase.SaveAssets();
            return asset;
        }

    }
}
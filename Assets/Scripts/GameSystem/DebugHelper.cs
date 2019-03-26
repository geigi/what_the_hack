using UnityEditor;
using UnityEngine;

namespace GameSystem
{
    public class DebugHelper: MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR
            if (EditorPrefs.GetBool("DEBUG_LOAD_GAME"))
                GameSettings.NewGame = false;
#endif
        }
    }
}
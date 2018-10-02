using UnityEngine;
using UnityEditor;

namespace Wth.ModApi.Editor.Editor
{
    /// <summary>
    /// A Base Editor, for Editing a Scriptable Object.
    /// </summary>
    /// <typeparam name="T">The Type of ScriptableObject that is to be edited.</typeparam>
    public abstract class BaseEditor<T> : EditorWindow where T : ScriptableObject
    {

        public abstract void OnGUI();

        /// <summary>
        /// The Asset which is being shown by the Editor.
        /// </summary>
        public T asset;

        /// <summary>
        /// The current Item of the SkillSet.
        /// </summary>
        protected int viewIndex = 1;

        /// <summary>
        /// Creates a new Asset of type T.
        /// </summary>
        /// <param name="assetPath">The Path the new Asset should be created.</param>
        protected virtual void CreateNewAsset(string assetPath)
        {
            Debug.Log("Creating New Set");
            // There is no overwrite protection here!
            viewIndex = 1;
            asset = CreateAsset.Create<T>(assetPath);
            if (asset)
            {
                string relPath = AssetDatabase.GetAssetPath(asset);
                EditorPrefs.SetString("AssetPath", relPath);
            }
        }

        /// <summary>
        /// Opens an Asset at a user defined path.
        /// </summary>
        /// <param name="objectName">The Name of this edited Scriptable Object.</param>
        protected virtual void OpenAsset(string objectName)
        {
            string absPath = EditorUtility.OpenFilePanel("Select " + objectName, "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                T asset = AssetDatabase.LoadAssetAtPath(relPath, typeof(T)) as T;
                if (asset)
                {
                    EditorPrefs.SetString("AssetPath", relPath);
                }
            }
        }

        /// <summary>
        /// Creates a Button to show, open anbd create a new Scriptable Object of type T.
        /// </summary>
        /// <param name="assetPath">The Path a new Asset should be created </param>
        /// <param name="windowName">The Name of this editor window.</param>
        /// <param name="objectName">The name of this edited Scriptable Object (Usually the name of the Class)</param>
        protected virtual void CreateListButtons(string assetPath, string windowName, string objectName)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(windowName, EditorStyles.boldLabel);
            if (this.asset != null)
            {
                if (GUILayout.Button("Show " + objectName))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = this.asset;
                }
            }
            if (GUILayout.Button("Open " + objectName))
            {
                this.OpenAsset(objectName);
            }
            if (GUILayout.Button("New " + objectName))
            {
                this.CreateNewAsset(assetPath);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = this.asset;
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Creates the navigation, to navigate the set of Scriptable Objects.
        /// Only for Scriptable Objects, which holds a List of other Objects.
        /// </summary>
        /// <param name="numItems">The current number of items in the List.</param>
        /// <param name="objectName">The name of this edited Scriptable Object (Usually the name of the Class)</param>
        /// <param name="subassetName">The Name of the Subassets which this Scriptable Object is a list of.</param>
        protected virtual void CreateAssetNavigation(int numItems, string objectName, string subassetName = "Asset")
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }

            GUILayout.Space(20);

            viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current " + objectName, viewIndex, GUILayout.ExpandWidth(false)), 1, numItems);
            GUILayout.Space(5);
            EditorGUILayout.LabelField("of   " + numItems.ToString() + subassetName, "", GUILayout.ExpandWidth(false));

            GUILayout.Space(20);

            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < numItems)
                {
                    viewIndex++;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}

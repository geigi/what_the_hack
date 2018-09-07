using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Wth.ModApi.Editor
{
    public class SkillEditor : EditorWindow
    {

        public SkillSet skillSet;
        private int viewIndex = 1;

        [MenuItem("Window/SkillEditor")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(SkillEditor));

        }

        void OnEnable()
        {
            if (EditorPrefs.HasKey("ObjectPath"))
            {
                string objectPath = EditorPrefs.GetString("ObjectPath");
                skillSet = AssetDatabase.LoadAssetAtPath(objectPath, typeof(SkillSet)) as SkillSet;
            }
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("SkillSet Editor", EditorStyles.boldLabel);

            if (skillSet != null)
            {
                if (GUILayout.Button("Show Skill Set"))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = skillSet;
                }
            }

            if (GUILayout.Button("Open Skill Set"))
            {
                OpenSkillSet();
            }

            if (GUILayout.Button("New Skill Set"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = skillSet;
            }

            GUILayout.EndHorizontal();

            if (skillSet == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                if (GUILayout.Button("Create New Skill Set", GUILayout.ExpandWidth(false)))
                {
                    CreateNewSkillSet();
                }

                if (GUILayout.Button("Open Existing Skill Set", GUILayout.ExpandWidth(false)))
                {
                    OpenSkillSet();
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(20);

            if (skillSet != null)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Space(10);

                if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
                {
                    if (viewIndex > 1)
                        viewIndex--;
                }

                GUILayout.Space(5);
                if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
                {
                    if (viewIndex < skillSet.skills.Count)
                    {
                        viewIndex++;
                    }
                }

                GUILayout.Space(60);

                if (GUILayout.Button("Add Skill", GUILayout.ExpandWidth(false)))
                {
                    AddSkill();
                }

                if (GUILayout.Button("Delete Skill", GUILayout.ExpandWidth(false)))
                {
                    DeleteSkill(viewIndex - 1);
                }

                GUILayout.EndHorizontal();
                if (skillSet.skills == null)
                    Debug.Log("wtf");
                if (skillSet.skills.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    viewIndex = Mathf.Clamp(
                        EditorGUILayout.IntField("Current Skill", viewIndex, GUILayout.ExpandWidth(false)), 1,
                        skillSet.skills.Count);
                    EditorGUILayout.LabelField("of   " + skillSet.skills.Count.ToString() + "  skills", "",
                        GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();

                    Dictionary<SkillDefinition, float>.KeyCollection keyCollection = skillSet.skills.Keys;
                    SkillDefinition[] skillsArray = new SkillDefinition[skillSet.skills.Count];
                    keyCollection.CopyTo(skillsArray, 0);
                    skillsArray[viewIndex - 1].skillName = EditorGUILayout.TextField("Skill Name",
                        skillsArray[viewIndex - 1].skillName as string);
                    skillsArray[viewIndex - 1].skillSprite = EditorGUILayout.ObjectField("Skill Icon",
                        skillsArray[viewIndex - 1].skillSprite, typeof(Sprite), false) as Sprite;

                    GUILayout.Space(10);
                }
                else
                {
                    GUILayout.Label("This SkillSet is Empty.");
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(skillSet);
            }
        }

        void CreateNewSkillSet()
        {
            Debug.Log("Creating New Set");
            // There is no overwrite protection here!
            viewIndex = 1;
            skillSet = CreateSkillSet.Create();
            if (skillSet)
            {
                skillSet.skills = new Dictionary<SkillDefinition, float>();
                string relPath = AssetDatabase.GetAssetPath(skillSet);
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }

        void OpenSkillSet()
        {
            string absPath = EditorUtility.OpenFilePanel("Select Skill Set", "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                skillSet = AssetDatabase.LoadAssetAtPath(relPath, typeof(SkillSet)) as SkillSet;
                if (skillSet.skills == null)
                    skillSet.skills = new Dictionary<SkillDefinition, float>();
                if (skillSet)
                {
                    EditorPrefs.SetString("ObjectPath", relPath);
                }
            }
        }

        void AddSkill()
        {
            SkillDefinition newSkill = new SkillDefinition();
            newSkill.skillName = "New Skill";
            skillSet.skills.Add(newSkill, 0);
            viewIndex = skillSet.skills.Count;
        }

        void DeleteSkill(int index)
        {
            Dictionary<SkillDefinition, float>.KeyCollection keyCollection = skillSet.skills.Keys;
            SkillDefinition[] skillsArray = new SkillDefinition[skillSet.skills.Count];
            keyCollection.CopyTo(skillsArray, 0);
            SkillDefinition key = skillsArray[index];
            skillSet.skills.Remove(key);
        }
    }
}
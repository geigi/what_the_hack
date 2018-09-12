using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Wth.ModApi.Editor
{
    public class SkillEditor : EditorWindow
    {

        public SkillSet skillSet;
        private int viewIndex = 1;

        private List<float> lastValues;
        private int skillIndexToDecrease;
        private float valueToDecreaseBy;
        private float percentagesAdded = 0;
        
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
                    // Focuses the asset in the File Menu
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = skillSet;
                }
            }

            if (GUILayout.Button("Open Skill Set"))
            {
                //Opens an existing SkillSet
                OpenSkillSet();
            }

            if (GUILayout.Button("New Skill Set"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = skillSet;
            }

            if (GUILayout.Button("Save Skills"))
            {
                int count = skillSet.keys.Count;
                for(int i = 0; i < count; i++)
                {
                    var asset = skillSet.keys[i];
                    SkillDefinition saved = ScriptableObject.CreateInstance<SkillDefinition>();
                    saved.skillName = asset.skillName;
                    saved.skillSprite = asset.skillSprite;
                    string assetPath = AssetDatabase.GetAssetPath(asset);
                    AssetDatabase.DeleteAsset(assetPath);
                    AssetDatabase.CreateAsset(saved, assetPath);
                    skillSet.keys[i] = saved;
                    AssetDatabase.SaveAssets();
                }
            }

            GUILayout.EndHorizontal();

            if (skillSet == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                if (GUILayout.Button("Create New Skill Set", GUILayout.ExpandWidth(false)))
                {
                    //Creates a new SkillSet and saves the Asset created
                    CreateNewSkillSet();
                }

                if (GUILayout.Button("Open Existing Skill Set", GUILayout.ExpandWidth(false)))
                {
                    //Opens an existing Skill Set
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
                    //Gets the previous item in the Dictionary
                    if (viewIndex > 1)
                        viewIndex--;
                }

                GUILayout.Space(5);
                if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
                {
                    if (viewIndex < skillSet.keys.Count)
                    {
                        //Gets the next item in the Dictionary
                        viewIndex++;
                    }
                }

                GUILayout.Space(60);

                if (GUILayout.Button("Add Skill", GUILayout.ExpandWidth(false)))
                {
                    //Adds a new Skill to the Dictionary
                    AddSkill();
                }

                if (GUILayout.Button("Delete Skill", GUILayout.ExpandWidth(false)))
                {
                    //Deletes a Skill from the Dictionary
                    DeleteSkill(viewIndex - 1);
                }
                GUILayout.EndHorizontal();
                if (skillSet.keys == null || skillSet.values == null)
                    Debug.Log("wtf");
                if (skillSet.keys.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    viewIndex = Mathf.Clamp(
                        EditorGUILayout.IntField("Current Skill", viewIndex, GUILayout.ExpandWidth(false)), 1,
                        skillSet.keys.Count);
                    EditorGUILayout.LabelField("of   " + skillSet.keys.Count.ToString() + "  skills", "",
                        GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();

                    skillSet.keys[viewIndex - 1].skillName = EditorGUILayout.TextField("Skill Name",
                        skillSet.keys[viewIndex - 1].skillName as string);
                    skillSet.keys[viewIndex - 1].skillSprite = EditorGUILayout.ObjectField("Skill Icon",
                        skillSet.keys[viewIndex - 1].skillSprite, typeof(Sprite), false) as Sprite;
                    
                    GUILayout.Space(10);

                    EditorGUILayout.LabelField("The Skills occurence probability");

                    GUILayout.Space(5);

                    //Adjust the Probability, if there are over 100.
                    if(percentagesAdded > 100)
                    {
                        skillSet.values[skillIndexToDecrease] -= valueToDecreaseBy;
                    }

                    this.percentagesAdded = 0;
                    //Set the skill to be decreased, to the skill with the biggest Percentage
                    this.skillIndexToDecrease = skillSet.values.IndexOf(skillSet.values.Max()); 
                    //Calculate the second Biggest Probability, in case the biggest skillProbability is being Modified.
                    int secondBiggestPercentageSkillIndex = skillSet.values.IndexOf(skillSet.values.Min());
                    SkillDefinition lastModified = skillSet.keys[0];

                    for (int i = 0; i < skillSet.keys.Count; i++) {
                        //Create a new Slider for the percentage
                        skillSet.values[i] = EditorGUILayout.Slider(skillSet.keys[i].skillName, skillSet.values[i], 0, 100);
                        
                        //Get the Skill with the second biggest Percentage
                        if (skillSet.values[i] > skillSet.values[secondBiggestPercentageSkillIndex] 
                            && i != skillIndexToDecrease)
                        {
                            secondBiggestPercentageSkillIndex = i;
                        }

                        //Get the skill which was last modified
                        if (lastValues != null && lastValues.Count > i && skillSet.values[i] != lastValues[i])
                        {
                            lastModified = skillSet.keys[i];
                        } 
                          
                        // Add all Percentages together
                        percentagesAdded += skillSet.values[i];
                    }
                    if (percentagesAdded > 100)
                    {
                        //Make sure, that the Slider which is currently being modified, does not decrease.
                        if (lastModified == skillSet.keys[skillIndexToDecrease])
                        {
                            skillIndexToDecrease = secondBiggestPercentageSkillIndex;
                        }
                        //Set the Amount to Decrease;
                        this.valueToDecreaseBy = percentagesAdded - 100;
                    }
                    this.lastValues = new List<float>(skillSet.values);
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
                skillSet.keys = new List<SkillDefinition>();
                skillSet.values = new List<float>();
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
                if (skillSet.keys == null)
                    skillSet.keys = new List<SkillDefinition>();
                if (skillSet.values == null)
                    skillSet.values = new List<float>();
                if (skillSet)
                {
                    EditorPrefs.SetString("ObjectPath", relPath);
                }
            }
        }

        void AddSkill()
        {
            var asset = ScriptableObject.CreateInstance<SkillDefinition>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/Skills/Skill " + skillSet.keys.Count + ".asset");
            AssetDatabase.SaveAssets();
            asset.skillName = "New Skill";
            skillSet.keys.Add(asset);
            skillSet.values.Add(0);
            viewIndex = skillSet.keys.Count;
        }

        void DeleteSkill(int index)
        {
            var item = skillSet.keys[index];
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
            skillSet.keys.Remove(item);
            skillSet.values.RemoveAt(index);
        }
    }
}
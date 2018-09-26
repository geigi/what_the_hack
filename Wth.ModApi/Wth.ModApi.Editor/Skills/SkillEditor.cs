using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Wth.ModApi.Editor
{
    /// <summary>
    /// An editor window, to modify an existing or create a new set of skills.
    /// </summary>
    public class SkillEditor : Editor.BaseEditor<SkillSet>
    {

        /// <summary>
        /// The percentage values, of the previous OnGui Iteration 
        /// </summary>
        private List<float> lastValues;

        /// <summary>
        /// The Index of the skill, where the percentage should be decreased the next time OnGui is called.
        /// </summary>
        private int skillIndexToDecrease;
        
        /// <summary>
        /// The amount the percentage value of the skill should be decreased. 
        /// </summary>
        private float valueToDecreaseBy;
        
        /// <summary>
        /// All probabilities added together.
        /// </summary>
        private float percentagesAdded = 0;

        /// <summary>
        /// Initializes the editor window.
        /// </summary>
        [MenuItem("Tools/What_The_Hack ModApi/Skill Creator")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(SkillEditor));
        }

        /// <summary>
        /// Procedure which is called when a SkillSet is loaded into the editor.
        /// </summary>
        void OnEnable()
        {
            if (EditorPrefs.HasKey("SkillSetPath"))
            {
                string objectPath = EditorPrefs.GetString("SkillSetPath");
                asset = AssetDatabase.LoadAssetAtPath(objectPath, typeof(SkillSet)) as SkillSet;
            }
        }

        /// <summary>
        /// Draws the GUI.
        /// </summary>
        public override void OnGUI()
        {
            CreateListButtons("Skill Editor", "Skill Set");
            GUILayout.Space(20);

            if (asset != null)
            {
                CreateAssetNavigation(asset.keys.Count, "Skill Set");
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Skill", GUILayout.ExpandWidth(false)))
                {
                    AddSkill();
                }
                if (GUILayout.Button("Delete Skill", GUILayout.ExpandWidth(false)))
                {
                    DeleteSkill(viewIndex - 1);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                this.CreateSkillSetGUI();

                if (GUILayout.Button("Save Skills"))
                {
                    AssetDatabase.SaveAssets();
                }
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(asset);
                foreach (var skillDefinition in asset.keys)
                {
                    EditorUtility.SetDirty(skillDefinition);
                }
            }
        }

        #region GUI

        /// <summary>
        /// Creates the buttons to open, show and create a new SkillSet.
        /// </summary>
        /*
        void CreateListButtons()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Skill Editor", EditorStyles.boldLabel);
            if (this.asset != null)
            {
                if (GUILayout.Button("Show Skill Set"))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = this.asset;
                }
            }
            if (GUILayout.Button("Open Skill Set"))
            {
                this.OpenSkillSet();
            }
            if (GUILayout.Button("New Skill Set"))
            {
                this.CreateNewSkillSet();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = this.asset;
            }
            GUILayout.EndHorizontal();
        }
        */
        /// <summary>
        /// Implements Buttons to navigate the set of skills.
        /// </summary>
        /*
        private void CreateSkillSetNavigation()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }

            GUILayout.Space(20);

            viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Skill Set", viewIndex, GUILayout.ExpandWidth(false)), 1, this.asset.keys.Count);
            GUILayout.Space(5);
            EditorGUILayout.LabelField("of   " + this.asset.keys.Count.ToString() + "  skills", "", GUILayout.ExpandWidth(false));

            GUILayout.Space(20);

            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < this.asset.keys.Count)
                {
                    viewIndex++;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        */

        /// <summary>
        /// Creates the GUI elements to change the occurence probability of a particular skill.
        /// </summary>
        private void CreateSkillSetGUI()
        {
            asset.keys[viewIndex - 1].skillName = EditorGUILayout.TextField("Skill Name",
                   asset.keys[viewIndex - 1].skillName as string);
            asset.keys[viewIndex - 1].skillSprite = EditorGUILayout.ObjectField("Skill Icon",
                asset.keys[viewIndex - 1].skillSprite, typeof(Sprite), false) as Sprite;

            GUILayout.Space(10);

            EditorGUILayout.LabelField("The Skills occurence probability");

            GUILayout.Space(5);

            //Adjust the probability, if all probabilies together exceeds 100%.
            if (percentagesAdded > 100)
            {
                asset.values[skillIndexToDecrease] -= valueToDecreaseBy;
            }

            this.percentagesAdded = 0;
            //Set the skill to be decreased, to the skill with the biggest percentage
            this.skillIndexToDecrease = asset.values.IndexOf(asset.values.Max());
            //Calculate the second biggest probability, in case the biggest skillProbability is being modified.
            int secondBiggestPercentageSkillIndex = asset.values.IndexOf(asset.values.Min());
            SkillDefinition lastModified = asset.keys[0];

            for (int i = 0; i < asset.keys.Count; i++)
            {
                //Create a new slider for the percentage
                asset.values[i] = EditorGUILayout.Slider(asset.keys[i].skillName, asset.values[i], 0, 100);

                //Get the skill with the second biggest percentage
                if (base.asset.values[i] > base.asset.values[secondBiggestPercentageSkillIndex]
                    && i != skillIndexToDecrease)
                {
                    secondBiggestPercentageSkillIndex = i;
                }

                //Get the skill which was last modified
                if (lastValues != null && lastValues.Count > i && base.asset.values[i] != lastValues[i])
                {
                    lastModified = base.asset.keys[i];
                }

                // Add all percentages together
                percentagesAdded += base.asset.values[i];
            }
            if (percentagesAdded > 100)
            {
                //Make sure, that the Slider which is currently being modified, does not decrease.
                if (lastModified == base.asset.keys[skillIndexToDecrease])
                {
                    skillIndexToDecrease = secondBiggestPercentageSkillIndex;
                }
                //Set the amount to decrease;
                this.valueToDecreaseBy = percentagesAdded - 100;
            }
            else
            {
                GUILayout.Label("This SkillSet is Empty.");
            }
            this.lastValues = new List<float>(asset.values);
        }

        #endregion

        /// <summary>
        /// Creates a new set of skills and saves it in the main folder.
        /// </summary>
        protected override void CreateNewAsset()
        {
            Debug.Log("Creating New Set");
            // There is no overwrite protection here!
            viewIndex = 1;
            base.CreateNewAsset();
            if (base.asset)
            {
                base.asset.keys = new List<SkillDefinition>();
                base.asset.values = new List<float>();
            }
        }

        /// <summary>
        /// Opens a SkillSet at a specific user defined location.
        /// </summary>
        protected override void OpenAsset()
        {
            base.OpenAsset();
            if (base.asset.keys == null)
                base.asset.keys = new List<SkillDefinition>();
            if (base.asset.values == null)
                base.asset.values = new List<float>();
        }

        /// <summary>
        /// Adds a new skill to the current SkillSet.
        /// </summary>
        void AddSkill()
        {
            var newSkill = CreateSkillDefinition.Create("Assets/Data/Skills/Skill " + base.asset.keys.Count + ".asset");

            newSkill.skillName = "New Skill";
            base.asset.keys.Add(newSkill);
            base.asset.values.Add(0);
            viewIndex = base.asset.keys.Count;
        }

        /// <summary>
        /// Deletes a skill from the current SkillSet.
        /// </summary>
        /// <param name="index">The index of the skill that should be removed.</param>
        void DeleteSkill(int index)
        {
            var item = base.asset.keys[index];
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
            base.asset.keys.Remove(item);
            base.asset.values.RemoveAt(index);
            base.viewIndex = base.asset.keys.Count;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Wth.ModApi.Editor.Tools;

namespace Wth.ModApi.Editor.Missions
{
    /// <summary>
    /// This class represents an editor for <see cref="MissionDefinition"/>.
    /// </summary>
    public class MissionEditor : BaseEditor<MissionList>
    {
        private bool skillFold = true;
        private bool requirementsFold = true;
        private MissionRequirements requiredMissions;
        private SerializedObject soRequiredMissions;
        private ReorderableList listRequiredMissions;
        private Vector2 scrollPos;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public MissionEditor()
        {
            assetName = "Mission";
            NeedsDictionary = true;
        }
        
        [MenuItem("Tools/What_The_Hack ModApi/Mission Creator", priority = 41)]
        static void Init()
        {
            GetWindow(typeof(MissionEditor), false, "Mission Creator");
            EditorStyles.textArea.wordWrap = true;
        }

        /// <summary>
        /// Create the UI.
        /// </summary>
        public override void OnGUI()
        {
            CreateListButtons("Assets/Data/Missions/MissionList.asset", "Mission List");
            GUILayout.Space(20);

            if (asset != null)
            {
                if (asset.missionList == null)
                    asset.missionList = new List<MissionDefinition>();
                CreateAssetNavigation(asset.missionList.Count);
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Mission", GUILayout.ExpandWidth(false)))
                {
                    AddItem();
                }

                if (GUILayout.Button("Delete Mission", GUILayout.ExpandWidth(false)))
                {
                    viewIndex -= 1;
                    DeleteItem(viewIndex);
                }
                
                if (GUILayout.Button("Import from JSON", GUILayout.ExpandWidth(false)))
                {
                    ImportFromJson();
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                
                CreateMissionGui();
                
                GUILayout.Space(10);
                
                if (GUILayout.Button("Save Missions"))
                {
                    SaveAssets();
                }
            }
                
            if (GUI.changed)
            {
                EditorUtility.SetDirty(asset);

                if (asset.missionList != null)
                {
                    foreach (var mission in base.asset.missionList)
                    {
                        EditorUtility.SetDirty(mission);
                    }
                }
            }
        }

        void CreateMissionGui()
        {
            if (asset.missionList.Count < 1)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("This mission list is empty.");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                return;
            }

            MissionDefinition mission = asset.missionList[viewIndex - 1];
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Filename", GUILayout.Width(146));
            EditorGUILayout.LabelField(mission.name);
            GUILayout.EndHorizontal();
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth));
            mission.Title = EditorGUILayout.TextField("Title", mission.Title);
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Description", GUILayout.Width(146));
            mission.Description = EditorGUILayout.TextArea(mission.Description, EditorStyles.textArea, GUILayout.Height(80));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Success Message", GUILayout.Width(146));
            mission.MissionSucceeded = EditorGUILayout.TextArea(mission.MissionSucceeded, EditorStyles.textArea, GUILayout.Height(80));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Failure Message", GUILayout.Width(146));
            mission.MissionFailed = EditorGUILayout.TextArea(mission.MissionFailed, EditorStyles.textArea, GUILayout.Height(80));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Difficulty", GUILayout.Width(146));
            mission.Difficulty = EditorGUILayout.IntSlider(mission.Difficulty, 0, 5);
            GUILayout.Label("Hardness", GUILayout.Width(146));
            mission.Hardness = EditorGUILayout.Slider(mission.Hardness, 0.5f, 10f);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Deadline", GUILayout.Width(146));
            mission.Deadline = EditorGUILayout.IntSlider(mission.Deadline, 1, 14);
            GUILayout.EndHorizontal();
            
            CreateSkillSelector(mission);
            CreateRequirements(mission);
            
            EditorGUILayout.EndScrollView();
        }

        void CreateSkillSelector(MissionDefinition mission)
        {
            skillFold = EditorGUILayout.Foldout(skillFold, "Required Skills");
            if (skillFold)
            {
                if (!EditorPrefs.HasKey("AssetPath" + "Skill"))
                {
                    GUILayout.Label("Please create a SkillSet first. This is required.");
                    return;
                }

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25f);
                EditorGUILayout.BeginVertical();
                string objectPath = EditorPrefs.GetString("AssetPath" + "Skill");
                var skillSet = AssetDatabase.LoadAssetAtPath(objectPath, typeof(SkillSet)) as SkillSet;
                CleanSkills(skillSet);
                if (skillSet != null)
                {
                    foreach (var skillDefinition in skillSet.keys)
                    {
                        bool isUsed = mission.SkillsRequired.Contains(skillDefinition);
                        isUsed = EditorGUILayout.ToggleLeft(skillDefinition.skillName, isUsed);
                        if (isUsed && !mission.SkillsRequired.Contains(skillDefinition))
                        {
                            mission.SkillsRequired.Add(skillDefinition);
                        }
                        else if (!isUsed && mission.SkillsRequired.Contains(skillDefinition))
                        {
                            mission.SkillsRequired.Remove(skillDefinition);
                        }
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        void CreateRequirements(MissionDefinition mission)
        {
            requirementsFold = EditorGUILayout.Foldout(requirementsFold, "Requirements");
            if (requirementsFold)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25f);
                EditorGUILayout.BeginVertical();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Required Level", GUILayout.Width(146));
                mission.RequiredLevel = EditorGUILayout.IntSlider(mission.RequiredLevel, 0, 100);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Required Employees", GUILayout.Width(146));
                mission.RequiredEmployees = EditorGUILayout.IntSlider(mission.RequiredEmployees, 0, 4);
                EditorGUILayout.EndHorizontal();
                
                mission.ForceAppear = EditorGUILayout.ToggleLeft("Force Appear", mission.ForceAppear);

                if (soRequiredMissions == null || requiredMissions != mission.RequiredMissions)
                {
                    requiredMissions = mission.RequiredMissions;
                    soRequiredMissions = new SerializedObject(requiredMissions);
                    listRequiredMissions = new ReorderableList(soRequiredMissions, soRequiredMissions.FindProperty("RequiredMissions"), false, true, true, true);
                    listRequiredMissions = new ReorderableList(soRequiredMissions, soRequiredMissions.FindProperty("RequiredMissions"), false, true, true, true);
                    listRequiredMissions.drawHeaderCallback = rect =>
                    {
                        EditorGUI.LabelField(rect, "Required completion of missions", EditorStyles.boldLabel);
                    };
                    listRequiredMissions.drawElementCallback = 
                        (Rect rect, int index, bool isActive, bool isFocused) => {
                            EditorGUI.PropertyField(rect, listRequiredMissions.serializedProperty.GetArrayElementAtIndex(index));
                        };
                }
                
                soRequiredMissions.Update();
                listRequiredMissions.DoLayoutList();
                soRequiredMissions.ApplyModifiedProperties();
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        void CleanSkills(SkillSet skillSet)
        {
            foreach (var skill in this.asset.missionList[viewIndex - 1].SkillsRequired.ToList())
            {
                if (!skillSet.keys.Contains(skill))
                {
                    this.asset.missionList[viewIndex - 1].SkillsRequired.Remove(skill);
                }
            }
        }
        
        void AddItem()
        {
            var asset = CreateInstance<MissionDefinition>();

            AssetDatabase.CreateAsset(asset,
                "Assets/Data/Missions/" + Guid.NewGuid() + ".asset");
            asset.Title = "Think about new missions";
            this.asset.missionList.Add(asset);
            viewIndex = this.asset.missionList.Count;

            var requirements = CreateInstance<MissionRequirements>();
            CreateDirectories("Assets/Data/Missions/Requirements/");
            AssetDatabase.CreateAsset(requirements, "Assets/Data/Missions/Requirements/" + Guid.NewGuid() + ".asset");
            asset.RequiredMissions = requirements;
            
            SaveAssets();
        }

        void DeleteItem(int index)
        {
            var item = asset.missionList[index];
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item.RequiredMissions));
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
            asset.missionList.RemoveAt(index);
            viewIndex = asset.missionList.Count;

            var dictionary = EditorTools.GetScriptableObjectDictionary();
            dictionary.Delete(item);
            EditorUtility.SetDirty(dictionary);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Get a list of all custom <see cref="MissionDefinition"/> in this list.
        /// This is used for the ScriptableObject Key Value dictionary used for serialization.
        /// </summary>
        /// <returns>List of <see cref="ScriptableObject"/> which are <see cref="MissionDefinition"/></returns>
        protected override List<ScriptableObject> GetList()
        {
            return asset.missionList.Cast<ScriptableObject>().ToList();
        }
        
        /// <summary>
        /// Create a new <see cref="MissionList"/> at a given path.
        /// </summary>
        /// <param name="assetPath">Path where <see cref="MissionList"/> will be saved</param>
        public override void CreateNewAsset(string assetPath)
        {
            base.CreateDirectories(assetPath);
            asset = EditorTools.Create<MissionList>(assetPath);
            if (asset)
            {
                string relPath = AssetDatabase.GetAssetPath(asset);
                EditorPrefs.SetString("AssetPath" + assetName, relPath);
            }
        }

        private void ImportFromJson()
        {
            string path = EditorUtility.OpenFilePanel("Choose JSON file", "", "json");
            if (File.Exists(path))
            {
                CreateDirectories("Assets/Data/Missions/Requirements/");
                try
                {
                    string json = File.ReadAllText(path);
                    var missions = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                    foreach (var mission in missions)
                    {
                        Debug.Log("Importing: " + mission["name"]);
                        var missionDefinition = CreateInstance<MissionDefinition>();
                        AssetDatabase.CreateAsset(missionDefinition, "Assets/Data/Missions/" + Guid.NewGuid() + ".asset");
                        
                        missionDefinition.Title = mission["name"].ToString();
                        missionDefinition.Description = mission["description"].ToString();
                        missionDefinition.MissionSucceeded = mission["onSuccess"].ToString();
                        missionDefinition.MissionFailed = mission["onFail"].ToString();
                        Debug.Log(mission["duration"]);
                        Debug.Log(mission["duration"].GetType());
                        missionDefinition.Hardness = float.Parse(mission["hardness"].ToString());
                        Debug.Log(missionDefinition.Hardness);
                        missionDefinition.Deadline = int.Parse(mission["duration"].ToString());
                        Debug.Log(missionDefinition.Deadline);
                        missionDefinition.RequiredLevel = int.Parse(mission["minLevel"].ToString());
                        
                        Debug.Log(mission["skill"].GetType());
                        
                        var skills = ((JArray)mission["skill"]).ToObject<List<Dictionary<string, string>>>();
                        string objectPath = EditorPrefs.GetString("AssetPath" + "Skill");
                        var skillSet = AssetDatabase.LoadAssetAtPath(objectPath, typeof(SkillSet)) as SkillSet;
                        foreach (var skill in skills)
                        {
                            foreach (var skillDefinition in skillSet.keys)
                            {
                                if (skillDefinition.skillName.ToLower() == skill["type"].ToLower())
                                {
                                    missionDefinition.SkillsRequired.Add(skillDefinition);
                                }
                            }
                        }
                        
                        var requirements = CreateInstance<MissionRequirements>();
                        AssetDatabase.CreateAsset(requirements, "Assets/Data/Missions/Requirements/" + Guid.NewGuid() + ".asset");
                        missionDefinition.RequiredMissions = requirements;
                        
                        this.asset.missionList.Add(missionDefinition);
                        EditorUtility.SetDirty(missionDefinition);
                        EditorUtility.SetDirty(requirements);
                    }
                    viewIndex = this.asset.missionList.Count;
                    EditorUtility.SetDirty(asset);
                    SaveAssets();
                } catch (Exception e)
                {
                    Debug.Log(e);
                    ShowNotification(new GUIContent("Could not parse JSON correctly"));
                }
            }
        }
    }
}
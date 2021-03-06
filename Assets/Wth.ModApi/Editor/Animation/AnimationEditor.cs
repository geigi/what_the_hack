﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Wth.ModApi.Editor
{
    /// <summary>
    /// An editor window to modify and create animations.
    /// </summary>
    class AnimationEditor : BaseEditor<AnimationClip>
    {

        /// <summary>
        /// The default amount of time, a sprite should be displayed in seconds.
        /// </summary>
        private static float defaultSpriteTime = 0.01f;

        /// <summary>
        /// The Employee this animation belongs to (can be null).
        /// </summary>
        public EmployeeDefinition emp { get; set; }

        /// <summary>
        /// The type of animation of the employee.
        /// </summary>
        public string animationString { get; set; }

        /// <summary>
        /// Name of the animation.
        /// </summary>
        string animationName = "Base Animation.anim";

        /// <summary>
        /// Relative path to the folder, this animation is located.
        /// </summary>
        string folderPath = "Assets/Animations/";

        /// <summary>
        /// Relative path to this animation.
        /// </summary>
        string filePath = "Assets/Animations/Base Animation.anim";

        /// <summary>
        /// Binding for the animation.
        /// </summary>
        EditorCurveBinding spriteBinding;

        /// <summary>
        /// List of all keyframes of this animation.
        /// </summary>
        List<KeyFrame> spriteKeyFrames;

        /// <summary>
        /// The animation settings.
        /// </summary>
        AnimationClipSettings animClipSettings;

        public AnimationEditor()
        {
            this.assetName = "frame";
        }

        [MenuItem("Tools/What_The_Hack ModApi/Animation Editor", priority = 80)]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(AnimationEditor), false, "Animation Editor");
        }

        protected override void OnEnable()
        {
            if (EditorPrefs.HasKey("AssetPath" + assetName))
            {
                SetFilePath(EditorPrefs.GetString("AssetPath" + assetName));
                asset = AssetDatabase.LoadAssetAtPath(filePath, typeof(AnimationClip)) as AnimationClip;
                if (asset)
                    GetSubAssets();
                else
                {
                    asset = null;
                    ResetPaths();
                }
            }
        }

        void ResetPaths()
        {
            animationName = "Base Animation.anim";
            filePath = "Assets/Animations/Base Animation.anim";
            folderPath = "Assets/Animations/";
        }

        public void Awake()
        {
            animationName = "Base Animation.anim";
            folderPath = "Assets/Animations/";
        }

        public override void OnGUI()
        {
            base.CreateListButtons(filePath, "Animation");
            CreateFileNameNav();
            if (asset != null)
            {
                GUILayout.Space(20);

                animClipSettings.loopTime = EditorGUILayout.Toggle("Loop Time", animClipSettings.loopTime);

                GUILayout.Space(20);
                base.CreateAssetNavigation(spriteKeyFrames.Count);
                CreateFrameEditing();
                GUILayout.Space(10);
                if (GUILayout.Button("Save Animation"))
                {
                    SaveAnimation();
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(asset);
            }
        }

        /// <summary>
        /// Creates the navigation to change the name and path of this animation.
        /// </summary>
        void CreateFileNameNav()
        {
            String notification = "";
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            SetAnimationName(EditorGUILayout.TextField("Animation Name", animationName));
            if (GUILayout.Button("Change File Name", GUILayout.MaxWidth(200)) && asset)
            {
                notification = ChangeFileName();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            filePath = EditorGUILayout.TextField("Path", filePath);
            if (GUILayout.Button("Change Path", GUILayout.MaxWidth(200)) && asset)
            {
                notification = ChangePath();
            }

            if (notification != "")
            {
                ShowNotification(new GUIContent(notification));
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the GUI to modify the different keyFrames.
        /// </summary>
        void CreateFrameEditing()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Frame", GUILayout.ExpandWidth(false)))
            {
                AddFrame();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Delete Frame", GUILayout.ExpandWidth(false)))
            {
                DeleteFrame();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (spriteKeyFrames.Count > 0)
            {
                SetSprite();
                //How long to show the Sprite
                if (viewIndex < spriteKeyFrames.Count)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    spriteKeyFrames[viewIndex - 1].showingTime = EditorGUILayout.FloatField("Show Sprite for: ",
                        spriteKeyFrames[viewIndex - 1].showingTime, GUILayout.ExpandWidth(false));
                    EditorGUILayout.LabelField("seconds", GUILayout.ExpandWidth(false));

                    if (GUILayout.Button("Adjust Showing time", GUILayout.ExpandWidth(false)))
                    {
                        String notification = SetSpriteDisplayTime();
                        ShowNotification(new GUIContent(notification));
                    }

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.BeginDisabledGroup(spriteKeyFrames[viewIndex - 1].evt != null);
                if (GUILayout.Button("Set Shadow Event"))
                {
                    spriteKeyFrames[viewIndex - 1].getEventOrNew();
                }
                EditorGUI.EndDisabledGroup();
                if (spriteKeyFrames[viewIndex - 1].evt != null)
                {
                    AnimationEvent _evt = spriteKeyFrames[viewIndex - 1].getEventOrNew();
                    _evt.objectReferenceParameter = EditorGUILayout.ObjectField("Shadow Sprite", _evt.objectReferenceParameter, typeof(Sprite), false) as
                        Sprite;
                    _evt.time = spriteKeyFrames[viewIndex - 1].frame.time;
                    _evt.functionName = "ShadowEvent";
                    spriteKeyFrames[viewIndex - 1].evt = _evt;
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Delete Event", GUILayout.ExpandWidth(false)))
                    {
                        spriteKeyFrames[viewIndex - 1].evt = null;
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }

        /// <summary>
        /// Creates a new asset at a specific path.
        /// </summary>
        /// <param name="path">The path, where the new animation should be created.</param>
        public override void CreateNewAsset(String path)
        {
            if (AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip != null)
            {
                SetAnimationName("Base Animation.anim");
                SetFilePath("Assets/Animations/Base Animation.anim");
                path = "Assets/Animations/Base Animation.anim";
            }

            emp = null;
            asset = new AnimationClip();
            asset.frameRate = 12;
            spriteKeyFrames = new List<KeyFrame>();
            spriteBinding = new EditorCurveBinding();
            spriteBinding.type = typeof(SpriteRenderer);
            spriteBinding.propertyName = "m_Sprite";
            animClipSettings = new AnimationClipSettings();
            AnimationUtility.SetAnimationClipSettings(asset, animClipSettings);
            animClipSettings.loopTime = true;
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Opens an Asset at an specific path and return the path, if the animation could be openend succesfully.
        /// </summary>
        /// <param name="objectName">The name of the Object.</param>
        /// <returns>The file path if opening the animation was succesfull, otherwise null</returns>
        protected override string OpenAsset(String objectName)
        {
            SetFilePath(base.OpenAsset(objectName));
            Debug.Log(filePath);
            if (asset != null)
            {
                GetSubAssets();
                return filePath;
            }

            return null;
        }

        #region UtilFunctions

        /// <summary>
        /// Gets the binding keyframes and displaying times into their variables.
        /// </summary>
        void GetSubAssets()
        {
            List<AnimationEvent> events = new List<AnimationEvent>(asset.events);
            animClipSettings = AnimationUtility.GetAnimationClipSettings(asset);
            if (animClipSettings == null)
                animClipSettings = new AnimationClipSettings();
            EditorCurveBinding[] bind = AnimationUtility.GetObjectReferenceCurveBindings(asset);
            if (bind.Length > 0)
            {
                spriteBinding = bind[0];
                spriteBinding.propertyName = "m_Sprite";
                ObjectReferenceKeyframe[] frames = AnimationUtility.GetObjectReferenceCurve(asset, spriteBinding);
                if (frames == null)
                    frames = new ObjectReferenceKeyframe[0];
                spriteKeyFrames = new List<KeyFrame>();
                for (var i = 0; i < frames.Length; i++)
                {
                    var keyFrame = frames[i];
                    // Does this keyFrame have a corresponding event?
                    AnimationEvent currEvent = events.Find((evt) => keyFrame.time == evt.time);
                    float spriteTime = defaultSpriteTime;
                    if (i < frames.Length - 1)
                    {
                        spriteTime = (frames[i + 1].time) - frames[i].time *
                                     asset.frameRate / 100;
                    }
                    spriteKeyFrames.Add(new KeyFrame(keyFrame, spriteTime, currEvent));
                }
            }
            else
            {
                spriteBinding = new EditorCurveBinding();
                spriteBinding.propertyName = "m_Sprite";
                spriteKeyFrames = new List<KeyFrame>();
            }
        }

        /// <summary>
        /// Builds and saves this animation.
        /// </summary>
        void SaveAnimation()
        {
            AnimationUtility.SetObjectReferenceCurve(asset, spriteBinding, KeyFrame.KeyFramesToArray(spriteKeyFrames));
            AnimationUtility.SetAnimationClipSettings(asset, animClipSettings);
            var events = KeyFrame.EventsToArray(spriteKeyFrames);
            AnimationUtility.SetAnimationEvents(asset, events);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            if (emp != null)
            {
                if (animationString.Equals("Basic Idle Animation"))
                    emp.IdleAnimation = asset;
                else if (animationString.Equals("Basic Walking Animation"))
                    emp.WalkingAnimation = asset;
                else if (animationString.Equals("Basic Working Animation"))
                    emp.WorkingAnimation = asset;
            }
        }

        /// <summary>
        /// Changes the file name of this animation.
        /// </summary>
        /// <returns>An empty String if the renaming was succesfull, otherwise an error specific message.</returns>
        string ChangeFileName()
        {
            string message = "";
            if (AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(asset), animationName) != "")
            {
                message = "Asset can't be renamed. Does a file with the " +
                          "same name already exist?";
                animationName = filePath.Split('/')[filePath.Split('/').Length - 1];
            }

            if (!filePath.EndsWith(animationName))
            {
                filePath = filePath.Replace(filePath.Split('/')[filePath.Split('/').Length - 1], animationName);
            }

            return message;
        }

        /// <summary>
        /// Changes the apth to this animation.
        /// </summary>
        /// <returns>An empty String if the renaming was succesfull, otherwise an error specific message.</returns>
        string ChangePath()
        {
            string returnString = "";
            //Discard unsafed animationName changes.
            if (!AssetDatabase.GetAssetPath(asset).EndsWith(animationName))
            {
                animationName = filePath.Split('/')[filePath.Split('/').Length - 1];
            }

            string oldFilePath = filePath;
            string oldFolderPath = folderPath;
            SetFolderPath(EditorUtility.OpenFolderPanel("Select new Path", folderPath, ""));
            String answer = AssetDatabase.MoveAsset(oldFilePath, filePath);
            if (answer != "" &&
                !answer.StartsWith("Trying to move asset to location it came from"))
            {
                returnString = (answer.EndsWith("is not a valid path."))
                    ? "Asset can only be moved to the Asset Folder" +
                      " or a Subfolder of the Asset Folder"
                    : "File with the same name already exists in that folder";
                SetFilePath(oldFilePath);
            }

            return returnString;
        }

        /// <summary>
        /// Set the sprite for this keyframe.
        /// </summary>
        void SetSprite()
        {
            ObjectReferenceKeyframe frame = spriteKeyFrames[viewIndex - 1].frame;
            frame.value =
                EditorGUILayout.ObjectField("Sprite", frame.value, typeof(Sprite), false) as
                    Sprite;
            spriteKeyFrames[viewIndex - 1].frame = frame;
        }

        /// <summary>
        /// Adjust the time a specific sprite should be displayed.
        /// </summary>
        /// <returns>An empty String if the renaming was succesfull, otherwise an error specific message.</returns>
        string SetSpriteDisplayTime()
        {
            string notification;
            if (spriteKeyFrames[viewIndex - 1].showingTime >= 0.01f)
            {
                float oldTime = (spriteKeyFrames[viewIndex].frame.time - spriteKeyFrames[viewIndex - 1].frame.time) *
                                asset.frameRate;
                oldTime /= 100;
                float diff = spriteKeyFrames[viewIndex - 1].showingTime - oldTime;
                for (int i = viewIndex; i < spriteKeyFrames.Count; i++)
                {
                    ObjectReferenceKeyframe currentFrame = spriteKeyFrames[i].frame;
                    currentFrame.time = (currentFrame.time * asset.frameRate) + (diff * 100);
                    currentFrame.time /= asset.frameRate;
                    spriteKeyFrames[i].frame = currentFrame;
                }

                notification = "Adjustet Times succesfuly, dont forget to save your Animation!";
            }
            else
            {
                notification = "Value must be bigger or equal than 0,01!";
            }

            return notification;
        }

        /// <summary>
        /// Adds a new Frame to this animation.
        /// </summary>
        void AddFrame()
        {
            ObjectReferenceKeyframe keyFrame = new ObjectReferenceKeyframe();
            keyFrame.time = spriteKeyFrames.Count / asset.frameRate;
            spriteKeyFrames.Add(new KeyFrame(keyFrame, defaultSpriteTime));
            viewIndex = spriteKeyFrames.Count;
        }

        /// <summary>
        /// Deletes a Frame from this Animation.
        /// </summary>
        void DeleteFrame()
        {
            float moveby = spriteKeyFrames[viewIndex - 1].showingTime;
            spriteKeyFrames.RemoveAt(viewIndex - 1);
            //Move all Frames after that
            if (viewIndex <= spriteKeyFrames.Count)
            {
                for (int i = viewIndex - 1; i < spriteKeyFrames.Count; i++)
                {
                    ObjectReferenceKeyframe current = spriteKeyFrames[i].frame;
                    current.time -= moveby / asset.frameRate;
                    spriteKeyFrames[i].frame = current;
                }
            }

            viewIndex = spriteKeyFrames.Count;
        }

        /// <summary>
        /// Pseudo Bindings for the folder, file Path and the Animation Name.
        /// </summary>
        /// <param name="newName">The new animation name.</param>
        public void SetAnimationName(string newName)
        {
            if (!newName.EndsWith(".anim"))
                newName += ".anim";
            animationName = newName;
            SetFilePath(folderPath + animationName);
        }

        /// <summary>
        /// Pseudo Bindings for the folder, file Path and the Animation Name.
        /// </summary>
        /// <param name="newFolderPath">The new Folder path</param>
        public void SetFolderPath(string newFolderPath)
        {
            if (newFolderPath != "")
            {
                //Make Relative Path if necassary.
                if (newFolderPath.StartsWith(Application.dataPath))
                {
                    newFolderPath = newFolderPath.Substring(Application.dataPath.Length - "Assets".Length);
                }

                if (!newFolderPath.EndsWith("/"))
                    newFolderPath += ("/");
                folderPath = newFolderPath;
                //Adjust filePath
                filePath = folderPath + animationName;
            }
        }

        /// <summary>
        /// Pseudo Bindings for the folder, file Path and the Animation Name.
        /// </summary>
        /// <param name="newFilePath">The ne path of this animation.</param>
        public void SetFilePath(string newFilePath)
        {
            if (newFilePath != "")
            {
                if (newFilePath.StartsWith(Application.dataPath))
                {
                    newFilePath = newFilePath.Substring(Application.dataPath.Length - "Assets".Length);
                }

                filePath = newFilePath;
                string[] s = filePath.Split('/');
                animationName = s[s.Length - 1];
                folderPath = filePath.Remove(filePath.Length - animationName.Length);
            }
        }

        /// <summary>
        /// Sets the Employee, if this animation is specific to an employee.
        /// </summary>
        /// <param name="_emp">The Employee.</param>
        public void SetEmp(EmployeeDefinition _emp)
        {
            Debug.Log("Hello World!" + emp.EmployeeName);
            emp = _emp;
        }

        #endregion
    }
}
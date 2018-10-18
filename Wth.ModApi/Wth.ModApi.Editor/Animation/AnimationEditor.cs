using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Wth.ModApi.Editor
{
				class AnimationEditor : EditorWindow
				{
								string animationName = "Base Animation.anim";
								string folderPath = "Assets/Animations/";
								string filePath = "Assets/Animations/Base Animation.anim";

								AnimationClip anim;
								EditorCurveBinding spriteBinding;
								List<ObjectReferenceKeyframe> spriteKeyFrames;
								AnimationClipSettings animClipSettings;
								int frameIndex = 1;

								[MenuItem("Tools/What_The_Hack ModApi/Animation Editor")]
								static void Init()
								{
												EditorWindow.GetWindow(typeof(AnimationEditor), false, "Animation Editor");
								}

								public void Awake()
								{
												Debug.Log("In Awake");
												animationName = "Base Animation.anim";
												folderPath = "Assets/Animations/";
								}

								public void OnGUI()
								{
												CreateBaseButtons();
												if (anim != null)
												{			
																GUILayout.Space(20);

																animClipSettings.loopTime = EditorGUILayout.Toggle("Loop Time", animClipSettings.loopTime);
																setFrameRate(EditorGUILayout.FloatField("Frame Rate", anim.frameRate));

																GUILayout.Space(20);
																CreateFrameNavigation();
																CreateFrameEditing();

																if(GUILayout.Button("Save Animation"))
																{
																				SaveAnimation();
																}
												}
												if(GUI.changed)
												{
																EditorUtility.SetDirty(anim);
												}
								}

								void CreateBaseButtons()
								{
												string notification = "";
												GUILayout.BeginHorizontal();
												GUILayout.Label("Animation Editor", EditorStyles.boldLabel);
												if (anim != null)
												{
																if (GUILayout.Button("Show Animation" ))
																{
																				EditorUtility.FocusProjectWindow();
																				Selection.activeObject = this.anim;
																}
												}
												if (GUILayout.Button("Open Animation"))
												{
																OpenAnimation();
												}
												if (GUILayout.Button("New Animation"))
												{
																this.CreateNewAnimation();
																EditorUtility.FocusProjectWindow();
																Selection.activeObject = this.anim;
												}
												GUILayout.EndHorizontal();

												GUILayout.Space(5);

												GUILayout.BeginHorizontal();
												SetAnimationName(EditorGUILayout.TextField("Animation Name", animationName));
												if (GUILayout.Button("Change File Name", GUILayout.MaxWidth(200)) && anim)
												{
																if(AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(anim), animationName) != "")
																{
																				notification = "Asset can't be renamed. Does a file with the " +
																												"same name already exist?";
																				animationName = filePath.Split('/')[filePath.Split('/').Length - 1];
																}
																if (! filePath.EndsWith(animationName))
																{
																				filePath = filePath.Replace(filePath.Split('/')[filePath.Split('/').Length - 1], animationName);
																}
												}
												GUILayout.EndHorizontal();
												GUILayout.BeginHorizontal();
												filePath = EditorGUILayout.TextField("Path", filePath);
												if(GUILayout.Button("Change Path", GUILayout.MaxWidth(200)) && anim)
												{
																//Discard unsafed animationName changes.
																if (!AssetDatabase.GetAssetPath(anim).EndsWith(animationName))
																{
																				animationName = filePath.Split('/')[filePath.Split('/').Length - 1];
																}
																string oldFilePath = filePath;
																string oldFolderPath = folderPath;
																SetFolderPath(EditorUtility.OpenFolderPanel("Select new Path", folderPath, ""));
																String answer = AssetDatabase.MoveAsset(oldFilePath, filePath);
																if (answer != "" &&
																				! answer.StartsWith("Trying to move asset to location it came from"))
																{
																				Debug.Log("Answer: " + answer);
																				notification = (answer.EndsWith("is not a valid path.")) ? "Asset can only be moved to the Asset Folder" +
																								" or a Subfolder of the Asset Folder" : "File with the same name already exists in that folder";
																				SetFilePath(oldFilePath);
																}
												}
												if(notification != "")
												{
																ShowNotification(new GUIContent(notification));
												}
												GUILayout.EndHorizontal();
								}

								void CreateFrameNavigation()
								{
												GUILayout.BeginHorizontal();
												GUILayout.FlexibleSpace();
												if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
												{
																if (frameIndex > 1)
																{
																				frameIndex--;
																}
												}
												GUILayout.Space(20);

												frameIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Frame ", frameIndex, GUILayout.ExpandWidth(false)),
																1, spriteKeyFrames.Count);
												GUILayout.Space(5);
												EditorGUILayout.LabelField("of " + spriteKeyFrames.Count.ToString() + " frames", GUILayout.ExpandWidth(false));
												GUILayout.Space(20);

												if(GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
												{
																if(frameIndex < spriteKeyFrames.Count)
																{
																				frameIndex++;
																}
												}
												GUILayout.FlexibleSpace();
												GUILayout.EndHorizontal();
								}

								void CreateFrameEditing()
								{
												GUILayout.BeginHorizontal();
												GUILayout.FlexibleSpace();
												if (GUILayout.Button("Add Frame", GUILayout.ExpandWidth(false)))
												{
																ObjectReferenceKeyframe keyFrame = new ObjectReferenceKeyframe();
																keyFrame.time = spriteKeyFrames.Count / anim.frameRate;
																DebugFrames();
																spriteKeyFrames.Add(keyFrame);
																frameIndex = spriteKeyFrames.Count;
												}

												GUILayout.Space(5);
												
												if(GUILayout.Button("Delete Frame", GUILayout.ExpandWidth(false)))
												{
																spriteKeyFrames.RemoveAt(frameIndex - 1);
																//Move all Frames after that one (for now) behind
																if(frameIndex <= spriteKeyFrames.Count)
																{
																				for (int i = frameIndex - 1; i < spriteKeyFrames.Count; i++)
																				{
																								ObjectReferenceKeyframe current = spriteKeyFrames[i];
																								current.time -= 1;
																								spriteKeyFrames[i] = current;
																				}
																}
																frameIndex = spriteKeyFrames.Count;
												}

												GUILayout.FlexibleSpace();
												GUILayout.EndHorizontal();

												GUILayout.Space(10);

												if (spriteKeyFrames.Count > 0)
												{
																ObjectReferenceKeyframe frame = spriteKeyFrames[frameIndex - 1];
																frame.value = EditorGUILayout.ObjectField("Sprite", spriteKeyFrames[frameIndex - 1].value, typeof(Sprite), false) as Sprite;
																spriteKeyFrames[frameIndex - 1] = frame;
												}
								}

								void OpenAnimation()
								{
												SetFilePath(EditorUtility.OpenFilePanel("Select Animation", filePath, ""));
												string[] s = filePath.Split('/');
												animationName = s[s.Length - 1];
												anim = AssetDatabase.LoadAssetAtPath(filePath, typeof(AnimationClip)) as AnimationClip;
												if (anim)
																getSubAssets();
												else
																CreateNewAnimation();
								}

								public void CreateNewAnimation()
								{
												anim = new AnimationClip();
												anim.frameRate = 12;
												spriteKeyFrames = new List<ObjectReferenceKeyframe>();
												spriteBinding = new EditorCurveBinding();
												spriteBinding.type = typeof(SpriteRenderer);
												animClipSettings = new AnimationClipSettings();
												animClipSettings.loopTime = true;
												AssetDatabase.CreateAsset(anim, filePath);
												AssetDatabase.SaveAssets();
								}

								void getSubAssets()
								{
												animClipSettings = AnimationUtility.GetAnimationClipSettings(anim);
												if (animClipSettings == null)
																animClipSettings = new AnimationClipSettings();
												EditorCurveBinding[] bind = AnimationUtility.GetObjectReferenceCurveBindings(anim);
												Debug.Log(bind.Length);
												if (bind.Length > 0)
												{
																spriteBinding = bind[0];
																ObjectReferenceKeyframe[] frames = AnimationUtility.GetObjectReferenceCurve(anim, spriteBinding);
																if (frames == null)
																				frames = new ObjectReferenceKeyframe[0];
																spriteKeyFrames = new List<ObjectReferenceKeyframe>(frames);
												}
												else
												{
																spriteBinding = new EditorCurveBinding();
																spriteKeyFrames = new List<ObjectReferenceKeyframe>();
												}
												DebugFrames();
								}

								void SaveAnimation()
								{
												AnimationUtility.SetObjectReferenceCurve(anim, spriteBinding, spriteKeyFrames.ToArray());
												EditorUtility.SetDirty(anim);
												AssetDatabase.SaveAssets();
								}

								public void SetAnimationName(string newName)
								{
												if (!newName.EndsWith(".anim"))
																newName += ".anim";
												animationName = newName;
								}

								public void SetFolderPath(string newFolderPath)
								{
												if(newFolderPath != "")
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

								public void SetFilePath(string newFilePath)
								{
												Debug.Log("new File path" + newFilePath);
												if(newFilePath != "")
												{
																if (newFilePath.StartsWith(Application.dataPath))
																{
																				newFilePath = newFilePath.Substring(Application.dataPath.Length - "Assets".Length);
																}
																filePath = newFilePath;
																folderPath = filePath.Remove(filePath.Length - animationName.Length);
												}
								}

								void setFrameRate(float newFrameRate)
								{
												if(newFrameRate > 0)
												{
																for(int i = 0; i < spriteKeyFrames.Count; i++)
																{
																				ObjectReferenceKeyframe current = spriteKeyFrames[i];
																				current.time = i / newFrameRate;
																				spriteKeyFrames[i] = current;
																}
																anim.frameRate = newFrameRate;
												}
								}
								void DebugFrames()
								{
												for(int i = 0; i < spriteKeyFrames.Count; i++)
												{
																Debug.Log(i + " " + spriteKeyFrames[i].time);
												}
								}
				}
}

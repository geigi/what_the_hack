using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wth.ModApi.Employees;

namespace Wth.ModApi.Editor
{
				class AnimationEditor : EditorWindow
				{
								private static float defaultSpriteTime = 0.01f;

								public EmployeeDefinition emp { get; set; }
								public String animationString { get; set; }

								string animationName = "Base Animation.anim";
								string folderPath = "Assets/Animations/";
								string filePath = "Assets/Animations/Base Animation.anim";

								AnimationClip anim;
								EditorCurveBinding spriteBinding;
								List<ObjectReferenceKeyframe> spriteKeyFrames;
								List<float> spriteShowingTime;
								AnimationClipSettings animClipSettings;
								int frameIndex = 1;

								[MenuItem("Tools/What_The_Hack ModApi/Animation Editor")]
								static void Init()
								{
												EditorWindow.GetWindow(typeof(AnimationEditor), false, "Animation Editor");
								}

								public void Awake()
								{
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
																notification = ChangeFileName();
												}
												GUILayout.EndHorizontal();
												GUILayout.BeginHorizontal();
												filePath = EditorGUILayout.TextField("Path", filePath);
												if (GUILayout.Button("Change Path", GUILayout.MaxWidth(200)) && anim)
												{
																notification = ChangePath();
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
																AddFrame();			
												}

												GUILayout.Space(5);
												
												if(GUILayout.Button("Delete Frame", GUILayout.ExpandWidth(false)))
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
																if(frameIndex < spriteShowingTime.Count)
																{
																				EditorGUILayout.BeginHorizontal();
																				GUILayout.FlexibleSpace();
																			 spriteShowingTime[frameIndex - 1] = EditorGUILayout.FloatField("Show Sprite for: ", spriteShowingTime[frameIndex - 1], GUILayout.ExpandWidth(false));
																				EditorGUILayout.LabelField("seconds", GUILayout.ExpandWidth(false));

																				if (GUILayout.Button("Adjust Showing time", GUILayout.ExpandWidth(false)))
																				{
																								String notification = SetSpriteShowingTime();
																								ShowNotification(new GUIContent(notification));
																				}
																				GUILayout.FlexibleSpace();
																				EditorGUILayout.EndHorizontal();
																}
												}
								}
								
								void OpenAnimation()
								{
												emp = null;
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
												emp = null;
												anim = new AnimationClip();
												anim.frameRate = 12;
												spriteKeyFrames = new List<ObjectReferenceKeyframe>();
												spriteShowingTime = new List<float>();
												spriteBinding = new EditorCurveBinding();
												spriteBinding.type = typeof(SpriteRenderer);
												animClipSettings = new AnimationClipSettings();
												animClipSettings.loopTime = true;
												AssetDatabase.CreateAsset(anim, filePath);
												AssetDatabase.SaveAssets();
								}

#region UtilFunctions

								void getSubAssets()
								{
												spriteShowingTime = new List<float>();
												animClipSettings = AnimationUtility.GetAnimationClipSettings(anim);
												if (animClipSettings == null)
																animClipSettings = new AnimationClipSettings();
												EditorCurveBinding[] bind = AnimationUtility.GetObjectReferenceCurveBindings(anim);
												if (bind.Length > 0)
												{
																spriteBinding = bind[0];
																ObjectReferenceKeyframe[] frames = AnimationUtility.GetObjectReferenceCurve(anim, spriteBinding);
																if (frames == null)
																				frames = new ObjectReferenceKeyframe[0];
																spriteKeyFrames = new List<ObjectReferenceKeyframe>(frames);

																// Get the Sprite Time Showings
																for(int i = 1; i < spriteKeyFrames.Count; i++)
																{
																				spriteShowingTime.Add((spriteKeyFrames[i].time - spriteKeyFrames[i - 1].time) * anim.frameRate/100);
																}
																//For Convinience add a Time to the last Frame, even though it can not be changed, except if I
																// find a workaround.
																spriteShowingTime.Add(defaultSpriteTime);
												}
												else
												{
																spriteBinding = new EditorCurveBinding();
																spriteKeyFrames = new List<ObjectReferenceKeyframe>();
												}
								}

								void SaveAnimation()
								{
												AnimationUtility.SetObjectReferenceCurve(anim, spriteBinding, spriteKeyFrames.ToArray());
												EditorUtility.SetDirty(anim);
												AssetDatabase.SaveAssets();
												if (emp != null)
												{
																Debug.Log("Here");
																if (animationString.Equals("Basic Idle Animation"))
																				emp.IdleAnimation = anim;
																else if (animationString.Equals("Basic Walking Animation"))
																				emp.WalkingAnimation = anim;
																else if (animationString.Equals("Basic Working Animation"))
																				emp.WorkingAnimation = anim;
												}
								}

								string ChangeFileName()
								{
												string message = "";
												if (AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(anim), animationName) != "")
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

								string ChangePath()
								{
												string returnString = "";
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
																!answer.StartsWith("Trying to move asset to location it came from"))
												{
																returnString = (answer.EndsWith("is not a valid path.")) ? "Asset can only be moved to the Asset Folder" +
																				" or a Subfolder of the Asset Folder" : "File with the same name already exists in that folder";
																SetFilePath(oldFilePath);
												}
												return returnString;
								}
								
								void SetSprite()
								{
												ObjectReferenceKeyframe frame = spriteKeyFrames[frameIndex - 1];
												frame.value = EditorGUILayout.ObjectField("Sprite", spriteKeyFrames[frameIndex - 1].value, typeof(Sprite), false) as Sprite;
												spriteKeyFrames[frameIndex - 1] = frame;
								}

								string SetSpriteShowingTime()
								{
												string notification;
												//Needs to be 0.009 because of weird floating Point Comparisons
												if (spriteShowingTime[frameIndex - 1] > 0.009)
												{
																float oldTime = (spriteKeyFrames[frameIndex].time - spriteKeyFrames[frameIndex - 1].time) * anim.frameRate;
																oldTime /= 100;
																float diff = spriteShowingTime[frameIndex - 1] - oldTime;
																for (int i = frameIndex; i < spriteShowingTime.Count; i++)
																{
																				ObjectReferenceKeyframe currentFrame = spriteKeyFrames[i];
																				currentFrame.time = (currentFrame.time * anim.frameRate) + (diff * 100);
																				currentFrame.time /= anim.frameRate;
																				spriteKeyFrames[i] = currentFrame;
																}
																notification = "Adjustet Times succesfuly, dont forget to save your Animation!";
												}
												else
												{
																notification = "Value must be bigger or equal than 0,01!";
												}
												return notification;
								}

								void AddFrame()
								{
												ObjectReferenceKeyframe keyFrame = new ObjectReferenceKeyframe();
												keyFrame.time = spriteKeyFrames.Count / anim.frameRate;
												spriteKeyFrames.Add(keyFrame);
												spriteShowingTime.Add(defaultSpriteTime);
												frameIndex = spriteKeyFrames.Count;
								}

								void DeleteFrame()
								{
												spriteShowingTime.RemoveAt(frameIndex - 1);
												spriteKeyFrames.RemoveAt(frameIndex - 1);
												//Move all Frames after that
												if (frameIndex <= spriteKeyFrames.Count)
												{
																for (int i = frameIndex - 1; i < spriteKeyFrames.Count; i++)
																{
																				ObjectReferenceKeyframe current = spriteKeyFrames[i];
																				current.time -= spriteShowingTime[i] / anim.frameRate;
																				spriteKeyFrames[i] = current;
																}
												}
												frameIndex = spriteKeyFrames.Count;
								}

								public void SetAnimationName(string newName)
								{
												if (!newName.EndsWith(".anim"))
																newName += ".anim";
												animationName = newName;
												SetFilePath(folderPath + animationName);
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

								public void SetEmp(EmployeeDefinition _emp)
								{
												Debug.Log("Hello World!" + emp.EmployeeName);
												emp = _emp;
								}

								#endregion

				}
}

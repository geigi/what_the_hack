using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Wth.ModApi.Editor
{
				class AnimationEditor : EditorWindow
				{

								public string animationName { get; set; } = "Base Animation";

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

								public void OnGUI()
								{
												CreateBaseButtons();
												if (anim != null)
												{			
																GUILayout.Space(20);

																animClipSettings.loopTime = EditorGUILayout.Toggle("Loop Time", animClipSettings.loopTime);
																anim.frameRate = EditorGUILayout.FloatField("Frame Rate", anim.frameRate);

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
												GUILayout.BeginHorizontal();
												animationName = EditorGUILayout.TextField("Animation Name", animationName);
																if (GUILayout.Button("Change File Name", GUILayout.MaxWidth(200)) && anim)
																{
																				AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(anim), animationName);
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
																keyFrame.time = spriteKeyFrames.Count;
																Debug.Log(keyFrame.time);
																spriteKeyFrames.Add(keyFrame);
																frameIndex = spriteKeyFrames.Count;
												}

												GUILayout.Space(5);
												
												if(GUILayout.Button("Delete Frame", GUILayout.ExpandWidth(false)))
												{
																//TODO: Move Frames after that one behind because one got deleted.
																spriteKeyFrames.RemoveAt(frameIndex - 1);
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
												string abspath = EditorUtility.OpenFilePanel("Select Animation", "", "");
												if (abspath.StartsWith(Application.dataPath))
												{
																string[] s = abspath.Split('/');
																animationName = s[s.Length - 1];
																string relpath = abspath.Substring(Application.dataPath.Length - "Assets".Length);
																anim = AssetDatabase.LoadAssetAtPath(relpath, typeof(AnimationClip)) as AnimationClip;
												}
												if(anim)
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
												} else
												{
																CreateNewAnimation();
												}
								}
								public void CreateNewAnimation()
								{
												anim = new AnimationClip();
												spriteKeyFrames = new List<ObjectReferenceKeyframe>();
												spriteBinding = new EditorCurveBinding();
												spriteBinding.type = typeof(SpriteRenderer);
												animClipSettings = new AnimationClipSettings();
												AssetDatabase.CreateAsset(anim, "Assets/Animations/" + animationName + ".anim");
												AssetDatabase.SaveAssets();
								}

								void SaveAnimation()
								{
												AnimationUtility.SetObjectReferenceCurve(anim, spriteBinding, spriteKeyFrames.ToArray());
												EditorUtility.SetDirty(anim);
												AssetDatabase.SaveAssets();
								}
				}
}

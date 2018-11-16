using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Wth.ModApi.Names;
using Wth.ModApi.Editor.Tools;

namespace Wth.ModApi.Editor.Names
{
				/// <summary>
				/// Editor Window for editing names.
				/// </summary>
				class NameEditor : BaseEditor<NameLists>
				{

								/// <summary>
								/// The current list, which is being modified.
								/// </summary>
								List<string> currentList;

								/// <summary>
								/// The name of the current list.
								/// </summary>
								Lists currentListName = Lists.surNamesMale;

								/// <summary>
								/// Default constructor.
								/// </summary>
								public NameEditor()
								{
												assetName = "Name";
												NeedsDictionary = false;
												JsonSerializable = true;
								}

								/// <summary>
								/// Initializes the window.
								/// </summary>
								[MenuItem("Tools/What_The_Hack ModApi/Name Editor", priority = 60)]
								static void Init()
								{
												EditorWindow.GetWindow(typeof(NameEditor), false, "Name Editor");
								}

								/// <summary>
								/// Draws the GUI.
								/// </summary>
								public override void OnGUI()
								{
												base.CreateListButtons("Assets/Data/Names/Names.asset", "Name List");
												GUILayout.Space(20);

												if(asset)
												{
																// Make dropdown menu.
																currentListName = (Lists)EditorGUILayout.EnumPopup("Current List:", currentListName);
																currentList = (currentListName == Lists.surNamesMale) ? asset.surNamesMale :
																				(currentListName == Lists.surNamesFemale) ? asset.surNamesFemale : asset.lastNames;

																base.CreateAssetNavigation(currentList.Count);
																GUILayout.Space(5);

																GUILayout.BeginHorizontal();
																GUILayout.FlexibleSpace();
																if (GUILayout.Button("Add Name", GUILayout.ExpandWidth(false)))
																				AddItem();
																if (GUILayout.Button("Delete Name", GUILayout.ExpandWidth(false)))
																				DeleteItem();
																GUILayout.FlexibleSpace();
																GUILayout.EndHorizontal();

																if (currentList.Count > 0)
																{
																				GUILayout.Space(5);
																				currentList[viewIndex - 1] = EditorGUILayout.TextField("Name: ", currentList[viewIndex - 1]);

																				GUILayout.Space(5);
																				if (GUILayout.Button("Save Names"))
																								AssetDatabase.SaveAssets();

																				GUILayout.Space(10);
																				SaveToJSON();
																} else
																{
																				viewIndex = 0;
																}
												}

												if(GUI.changed)
												{
																EditorUtility.SetDirty(asset);
												}
								}

								/// <summary>
								/// Creates a new asset at the specified path.
								/// </summary>
								/// <param name="assetPath">Path the asset should be created.</param>
								public override void CreateNewAsset(string assetPath)
								{
												base.CreateDirectories(assetPath);
												asset = EditorTools.Create<NameLists>(assetPath);

												if(asset)
												{
																string relPath = AssetDatabase.GetAssetPath(asset);
																EditorPrefs.SetString("AssetPath" + assetName, relPath);
												}
								}

								/// <summary>
								/// Adds a new name to the current list.
								/// </summary>
								void AddItem()
								{
												currentList.Add("new name");
												viewIndex = this.currentList.Count();
								}

								/// <summary>
								/// Deletes the name from the list.
								/// </summary>
								void DeleteItem()
								{
												viewIndex -= 1;
												currentList.RemoveAt(viewIndex);
												viewIndex = currentList.Count;
								}
				}
}

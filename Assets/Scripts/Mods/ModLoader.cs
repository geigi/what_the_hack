using System;
using System.Collections;
using System.Collections.Generic;
#if !UNITY_WEBGL
using ModTool;
#endif
using UnityEngine;
using UnityEngine.UI;
using Wth.ModApi;

public class ModLoader : MonoBehaviour {
	public Text titleText, descriptionText;
	public Image banner;
	public GameObject loadingPanel;

#if !UNITY_WEBGL
	private Mod currentMod;
	private List<Mod> mods;
	private ModManager modManager;
#endif

	private string baseTitle, baseDescription;
	private Sprite baseBanner;

	// Use this for initialization
	void Start () {
		baseTitle = titleText.text;
		baseDescription = descriptionText.text;
		baseBanner = banner.sprite;

#if !UNITY_WEBGL
		mods = new List<Mod>();
		modManager = ModManager.instance;

		var path = Application.dataPath;
    	if (Application.platform == RuntimePlatform.OSXPlayer) {
        	path += "/../../Mods";
    	}
    	else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer) {
        	path += "/../Mods";
    	}

#if !UNITY_ANDROID
		modManager.AddSearchDirectory(path);
#endif

		modManager.refreshInterval = 15;

        foreach (Mod mod in modManager.mods)
            OnModFound(mod);

        modManager.ModFound += OnModFound;
        modManager.ModRemoved += OnModRemoved;
		modManager.ModLoaded += OnModLoaded;
#endif
	}

#if !UNITY_WEBGL
	public void NextMod() {
		if (currentMod != null) {
			var currentModIndex = mods.FindIndex(p => p == currentMod);
			if (currentModIndex < mods.Count - 1) {
				currentMod.Unload();
				currentMod = mods[currentModIndex + 1];
				LoadMod(currentMod);
			}
			else {
				return;
			}
		}
		else {
			if (mods.Count > 0) {
				currentMod = mods[0];
				LoadMod(currentMod);
			}
			else {
				// No mods found.
				return;
			}
		}
	}

	public void PrevMod() {
		if (currentMod == null) {
			// We are at the beginning of the List, no more going further.
			return;
		}
		else {
			var currentModIndex = mods.FindIndex(p => p == currentMod);
			if (currentModIndex < 1) {
				currentMod.Unload();
				currentMod = null;
				DisplayBaseGame();
			}
			else {
				currentMod.Unload();
				currentMod = mods[currentModIndex - 1];
				LoadMod(currentMod);
			}
		}
	}

	private bool ValidateMod(Mod mod) {
		try {
			mod.GetAsset("ModInfo");
			return true;
		}
		catch (Exception e) {
			Debug.LogError("Mod " + mod.name + " doesn't contain a ModInfo asset which is required for each mod.");
			Debug.LogError(e);
			return false;
		}
	}

	private void LoadMod(Mod mod) {
		loadingPanel.SetActive(true);
		mod.LoadAsync();
	}

	private void DisplayBaseGame() {
		titleText.text = baseTitle;
		descriptionText.text = baseDescription;
		banner.sprite = baseBanner;
	}

	private void DisplayMod(Mod mod) {
		titleText.text = mod.modInfo.name;
		descriptionText.text = mod.modInfo.description + Environment.NewLine + Environment.NewLine + "Created by " + mod.modInfo.author;
		banner.sprite = mod.GetAsset<ModInfo>("modinfo").banner;
	}

	private void OnModFound(Mod mod)
    {
		if (ValidateMod(mod)) {
			mods.Add(mod);
		}
		else {
			Debug.LogWarning("Could not load mod:");
			LogModInfo(mod);
		}
    }

    private void OnModRemoved(Mod mod)
    {
        mods.Remove(mod);
    }

	private void OnModLoaded(Mod mod) {
		loadingPanel.SetActive(false);
		DisplayMod(mod);
	}

	private void LogModInfo(Mod mod) {
		Debug.Log(mod.modInfo);
		Debug.Log("AssemblyNames:");
		Debug.Log(mod.assemblyNames);
		Debug.Log("AssetPaths:");
		Debug.Log(mod.assetPaths);
	}
#endif
}

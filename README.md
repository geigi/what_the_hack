![What\_the\_Hack][image-1]

![][image-2]

Have you ever wondered what "White Hat Hacking" is all about? Try it yourself as the head of a white hat hacking company in **What the Hack**!
Get started by hiring your first employee, buy your first computer and accept one of many generated missions. But it's not as easy as it sounds - each employee has it's strengths and weaknesses, so be sure to find the perfect mission for your team.
While playing the game you'll learn a lot about IT security, hacking and prevention of cyber attacks.

**What the Hack** is a modular game. Creating extensions is easy with the free [Unity Editor][1]. Have a look at section [Mod Development][2] for more information.

# Features
- What the Hack is a platform
	- Support for all kind of different extensions! (called *Mods*)
	- Multiple difficulties: game content for everyone
	- Interactive missions that test your knowledge
- Base Game
	- The base game ships with a special employee and basic missions about IT-security
- Two game modes: Classic and Realtime!
	- **Classic**: Just as every other game. Start it and enjoy! Automatic saving is included.
	- **Realtime**: Even when you close the game, the game time passes. Without draining your battery. You’ll get notifications when your employees need your assistance! 

# Download
## Main Game
Android:	[Download v1.0.3][3]

Linux:		[Download v1.0.3][4]

macOS:		[Download v1.0.3][5]

Windows:	[Download v1.0.3][6]

## Java Cryptography Extension Addon
Desktop: [Download v1.0][7]

Android: [Download v1.0][8]

Source: [GitHub Repository][9]

## Development
Mod Creator: [Download UnityPackage v1.0.2][10]

Mod Assets: [Download UnityPackage v1.0][11] 

Template for Addon-APK: [GitHub Repository][12]

# Installing a mod
## Addon-Apps on Android
On Android you can install special Addon-Apps, that contain extensions for **What the Hack**!

## Manual
Mods are served as a zip file and contain a folder which needs to be copied to specific locations depending on your operating system.
- **Android:** `Android/data/com.github.geigi.wth/files/Mods/` (on external storage or SD card)
- **Linux & macOS & Windows:** `Mods` directory side by side with the executable

# Development
## Requirements
Whether you want to contribute to the source code or you want to create a mod, the only thing you'll need is [**Unity v2018.3.7f1**][13]. 

**Important**: You must use this exact version of Unity because **What the Hack** can only load Mods that are compiled with the same version as the base game. 

## Components
There are three main components:
- The *Main Game* Unity project which is located in this directory.
- The *Wth.ModApi* and *Wth.ModApi.Editor* assemblies contain the addon API. They are located in the `Assets/Wth.ModApi` folder. 
- The *AndroidPlugin* Android Studio project which is located in the `AndroidPlugin` folder. It handles the installation and deinstallation of addon apps.

## Getting the project started
1. Clone the repository
2. Open Unity and the project folder `./`

# Mod Development
## How far can I go?
_What the Hack_ allows all kinds of changes to the game. Each mod will be it's own _game mode_ with unique names, employees, skills and missions. This allows you to create a mod with very specific learning content as well as new gameplay aspects. None of those modifications are mandatory. Here is a list of possible modifications:
- Create a specific set of skills (this is required for any of the following modifications)
- Create unique employees with custom sprites
- Create missions with custom actions for the player
- Create unique names that are used for employees and for placeholders in missions
- Implement multiple difficulty levels

## Getting Started
1. Before you create a mod, have a look at the [development requirements][14]. 
2. After installing Unity, create a new Unity Project and drag the `What_the_Hack Mod Tools.unitypackage` into the Editor. This package contains the API as well as the GUI tools to create new mods.
3. If you want to create interactive missions: Drag the `What the Hack Mod Assets.unitypackage` into the Editor. This contains prefabs for commonly used UI items like buttons, dropdowns, toggles, scroll views and all of the official sprites and fonts.
4. Create a `ModInfo` Scriptable Object: `Assets > Create > What_The_Hack ModApi > Mod Info`. Each mod requires a unique ID and a banner image with the resolution _400\*182_. This scriptable object will be the gathering point for your content. If some content is missing when loading the mod, it probably isn't included in the `ModInfo`.
5. Get started by using the custom Employee, Skill, Mission and Name Creator GUIs which can be found under `Tools > What_The_Hack ModApi`. 
6. All the content you generate from the Creators are stored in `Assets/Data`.

## Creating missions (and other content)
This chapter describes how to create custom missions. Creating Skills, Employees or Names is very similar to creating missions. Therefore there won’t be a section for each of the possible modifications.

1. If you want to create custom missions, you first have to create a skill set in the _Skill Creator_. The missions you define use the skills from your custom skill set. If you want to use the default skill set, you can download it from this repository: `Assets/Data/Skills`.
2. Create a new mission list from the _Mission Creator_.
3. Click on your `ModInfo` file and drag the freshly generated `MissionList` from `Assets/data` into the according field of the `ModInfo`. Otherwise your missions won’t load in What the Hack.
4. Add a new mission in the _Mission Creator_.
5. Don’t forget to hit the **Save** button when you modified your missions.

## Creating a interactive mission
Interactive missions have so called **MissionHooks** that define a interaction. Each mission can have multiple hooks. Each hook contains a _GUI Prefab_ that will be displayed, as soon as the user opens the interaction window in the game. You can specify at which progress level a hook will be spawned.

Each interaction has only two outcomes: Success or Fail.

- Create a new `MissionHook` object: `Assets > Create > What_The_Hack ModApi > Missions > MissionHook`
- Define the spawn time in the inspector.
- Find the `HookTemplate` from the `What the Hack Mod Assets` in `Assets/Prefabs/` and duplicate it.
- Open the prefab. You will see the UI in the Scene view of Unity. Remove the demo buttons from the scene graph.
- You can now create your own UI. Use the Elements you find in the `Assets/Prefabs/` folder.
- The GUI is now created, but it doesn’t contain any logic. You have to report whether the interaction has finished successfully or not. If you use a simple dropdown or toggles to ask the user a question, you can use the included and generic `Verifier` Components from the Mod API: `DropdownHookVerifier`and `ToggleHookVerifier`. Attach one of those components to the root object of your prefab. 
- If you want to create a more complex interaction, you can also write your own code. To do this, create a new Script. Instead of inheriting from `MonoBehaviour`, you must inherit from `ModBehaviour`. You can use this class like any `MonoBehaviour`. If you forget to use `MonoBehaviour` you won’t be able to export the mod.
	- Implement your desired logic. When the interaction is completed and you want to report the success/failure, you need the `MissionHook` object that contains this interaction. Just add it as a public variable and assign it in the inspector. To report success call `Hook.RaiseSuccess()`, for failure call `Hook.RaiseFailure()`.
	- Below you find a reference example:

```cs
public class DropdownHookVerifier : ModBehaviour
{
    public Button OkButton;
    public Dropdown Dropdown;
    public MissionHook Hook;
    public List<int> ValidOptions;

    private void Start()
    {
        OkButton.onClick.AddListener(Verify);
    }

    private void Verify()
    {
        if (ValidOptions.Contains(Dropdown.value))
            Hook.RaiseSuccess();
        else
            Hook.RaiseFailed();
    }

    private void OnDestroy()
    {
        OkButton.onClick.RemoveListener(Verify);
    }
}
```

- Now you have to assign your prefab to the `MissionHook` object you created at the beginning. Do this in the inspector.
- Finally, you have to add your `MissionHook` to your mission. To do this, open the _Mission Creator_ and add your hook.

## Exporting a mod
- To export your mod, go to `Tools > ModTool`.
- Define a mod name, author, description and version number. Also select, for which platforms you want to export the mod.
- Finally select the content you want to export. What the Hack doesn’t support custom Scenes. Therefore you can deselect them. Also deselect _Code_ if you didn’t create any custom code. Otherwise the mod will not work properly.
- Set a output directory.
- Hit `Export`!

## If saving and loading a game doesn’t work
Each ScriptableObject you define (the files in the `Assets/Data` folder) need to be referenced in a `ScriptableObjectDictionary`. This file is created automatically and also managed automatically by the _Creator_ windows. If something goes wrong have a look at this dictionary and whether every ScriptableObject from your mod is included here. Otherwise open the according Creator and click _Save_. This should fix the problem in most cases.

# Known bugs
- Game does not open when tapping on a notification (Android). This is a bug in the Unity Mobile Notifications package.

# Cheers to the original creators! ❤️
- Hendrik Schulte ([@hendrik-schulte][15])
- Florian Kaulfertsch
- Dominik Köhler

# Special Thanks 🎉
- Tobias Melzer ([@tobias-melzer][16])
- Prof. Dr. Holger Schmidt

# Third party libraries
The following third party libraries are used by this game:

## Libraries by Unity3D:
- [2d-extras][17] - MIT
- [Asset Bundle Browser][18] (not included in build) - [Unity Companion License][19]
- [Legacy Image Effects][20] - No Licence

## Other Libraries
- [Unity Enhanced][21] - MIT
- [ModTool][22] - MIT
- [KinoGlitch][23] - No licence
- [unityglitch][24] - [Creative Commons Attribution 3.0 Unported][25]
- [NSubstitute][26] - BSD Licence

## WTH contains code based on projects
- [Pathfinding][27] - MIT

## Sounds
sf3-sfx-menu-select.wav by broumbroum freesound.org

## Fonts
http://www.zone38.net/font/

![][image-3]

[1]:	https://unity3d.com/de/get-unity/download/archive
[2]:	#mod-development
[3]:	https://github.com/geigi/what_the_hack/releases/download/1.0.3/What_the_Hack-1.0.3.apk
[4]:	https://github.com/geigi/what_the_hack/releases/download/1.0.3/What_the_Hack_1.0.3_linux.zip
[5]:	https://github.com/geigi/what_the_hack/releases/download/1.0.3/What_the_Hack_1.0.3_macOS.zip
[6]:	https://github.com/geigi/what_the_hack/releases/download/1.0.3/What_the_Hack_1.0.3_win.zip
[7]:	https://github.com/geigi/wth-java-crypto-mod/releases/download/1.0/What_the_Hack.JCE.v1.0.zip
[8]:	https://github.com/geigi/wth-java-crypto-mod/releases/download/1.0/What_the_Hack.JCE.v1.0.apk
[9]:	https://github.com/geigi/wth-java-crypto-mod
[10]:	https://github.com/geigi/what_the_hack/releases/download/1.0.2/What_the_Hack.Mod.Tools.v1.0.2.unitypackage
[11]:	https://github.com/geigi/what_the_hack/releases/download/1.0.3/What_the_Hack.Mod.Assets.v1.0.unitypackage
[12]:	https://github.com/geigi/wth-examplemod-apk
[13]:	https://unity3d.com/de/get-unity/download/archive
[14]:	#requirements
[15]:	https://github.com/hendrik-schulte
[16]:	https://github.com/Tobias-Melzer
[17]:	https://github.com/Unity-Technologies/2d-extras
[18]:	https://assetstore.unity.com/packages/slug/93571
[19]:	https://unity3d.com/legal/licenses/Unity_Companion_License
[20]:	https://assetstore.unity.com/packages/essentials/legacy-image-effects-83913
[21]:	https://github.com/hendrik-schulte/UnityEnhanced
[22]:	https://github.com/Hello-Meow/ModTool
[23]:	https://github.com/keijiro/KinoGlitch
[24]:	https://github.com/staffantan/unityglitch
[25]:	http://creativecommons.org/licenses/by/3.0/deed.en_GB
[26]:	http://nsubstitute.github.io
[27]:	https://github.com/SebLague/Pathfinding

[image-1]:	Sprites/ui/GameLogo.png
[image-2]:	Sprites/ui/base_game_banner.png
[image-3]:	https://gitlab.com/geigi/what%5C_the%5C_hack/badges/master/build.svg
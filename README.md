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

# How to play
**What the Hack** is designed for your smartphone, but you can also try it on PC.

# Installing a mod
## Addon-Apps on Android
On Android you can install special Addon-Apps, that contain extensions for **What the Hack**!

## Manual
Mods are served as a zip file and contain a folder which needs to be copied to specific locations depending on your operating system.
- **Android:** `Android/data/com.hsd.wth/files/Mods/` (on external storage or SD card)
- **Linux & macOS & Windows:** `Mods` directory side by side with the executable


# Development
## Requirements
Whether you want to contribute to the source code or you want to create a mod, the only thing you'll need is [**Unity v2018.3.5f1**][3]. 

**Important**: You must use this exact version of Unity because **What the Hack** can only load Mods that are compiled with the same version as the base game. 

## Components
There are four main components:
- The *Main Game* Unity project which is located in this directory.
- The *ModApi* Visual Studio Solution which is located in the `Wth.ModApi` folder. This solution builds two DLLs which are used in the ModCreator Package.
- The *AndroidPlugin* Android Studio project which is located in the `AndroidPlugin` folder. It handles native Android integration such as Notifications. 
- The *Example Mod* Unity Project which will be included later on.

## Getting the project started
1. Clone the repository
2. Before opening Unity, open the solution `Wth.ModApi/Wth.ModApi.sln`
3. Build the solution. This will generate the two DLLs `Wth.ModApi[.Editor].dll` and copies them to `Assets/ModTool`. Those contain the Mod API code and will be used by Unity.
4. Open Unity and the project folder `./`
5. In the Inspector make sure that `Wth.ModApi.Editor.dll` will be excluded from any builds
6. That’s it :) If you change the Mod API you have to rebuild the solution manually before you can use your changes in Unity and the main project.

# Mod Development
## How far can I go?
What the Hack allows all kinds of changes to the game. Each mod will be it's own "game mode" with unique names, employees, skills and missions. This allows you to create a mod with very specific learning content as well as new gameplay aspects. None of those modifications are mandatory. Here is a list of possible modifications:
- Create unique employees with custom sprites
- Create a specific set of skills
- Create missions with custom actions for the player
- Implement multiple difficulty levels

## Getting Started
1. Before you create a mod, have a look at the [development requirements][4]. 
2. After installing Unity, create a new Unity Project and drag the `Wth.ModCreator.unitypackage` into the Editor. 
3. Create a `ModInfo` Scriptable Object: `Assets > Create > What_The_Hack ModApi > Mod Info`. Each mod requires a unique ID and a banner image with the resolution _400\*182_. This scriptable object will be the gathering point for your content.
4. Get started by using the custom Employee, Skill, Mission and Name Creator GUIs which can be found under `Tools > What_The_Hack ModApi`. 

To export your mod, go to `Tools > ModTool` and hit `Export`. Your code will be checked for any incompatibilities with the main game. 

# Third party libraries
The following third party libraries are used by this game:

## Libraries by Unity3D:
- [2d-extras][5] - MIT
- [Asset Bundle Browser][6] (not included in build) - [Unity Companion License][7]
- [Legacy Image Effects][8] - No Licence

## Other Libraries
- [Unity Enhanced][9] - MIT
- [ModTool][10] - MIT
- [KinoGlitch][11] - No licence
- [unityglitch][12] - [Creative Commons Attribution 3.0 Unported][13]
- [NSubstitute][14] - BSD Licence

## WTH contains code based on projects
- [Pathfinding][15] - MIT

## Sounds
sf3-sfx-menu-select.wav by broumbroum freesound.org

## Fonts
http://www.zone38.net/font/

![][image-3]

[1]:	https://unity3d.com/de/get-unity/download/archive
[2]:	#mod-development
[3]:	https://unity3d.com/de/get-unity/download/archive
[4]:	#requirements
[5]:	https://github.com/Unity-Technologies/2d-extras
[6]:	https://assetstore.unity.com/packages/slug/93571
[7]:	https://unity3d.com/legal/licenses/Unity_Companion_License
[8]:	https://assetstore.unity.com/packages/essentials/legacy-image-effects-83913
[9]:	https://github.com/hendrik-schulte/UnityEnhanced
[10]:	https://github.com/Hello-Meow/ModTool
[11]:	https://github.com/keijiro/KinoGlitch
[12]:	https://github.com/staffantan/unityglitch
[13]:	http://creativecommons.org/licenses/by/3.0/deed.en_GB
[14]:	http://nsubstitute.github.io
[15]:	https://github.com/SebLague/Pathfinding

[image-1]:	Sprites/ui/GameLogo.png
[image-2]:	Sprites/ui/base_game_banner.png
[image-3]:	https://gitlab.com/geigi/what%5C_the%5C_hack/badges/master/build.svg
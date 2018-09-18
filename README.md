![What\_the\_Hack][image-1]

![][image-2]

Have you ever wondered what "White Hat Hacking" is all about? Try it yourself as the head of an white hat hacking company in **What the Hack**!
Get started by hiring your first employee, buy your first computer and accept one of many generated missions. But it's not as easy as it sounds - each employee has it's strengths and weaknesses, so be sure to find the perfect mission for your team.
While playing the game you'll learn a lot about IT security, hacking and prevention of cyber attacks.

**What the Hack** is a modular game. Creating extensions is easy with the free [Unity Editor][1]. Have a look at section [Creating a Mod][2] for more information.

# Development Requirements
Whether you want to contribute to the source code or you want to create a mod, the only thing you'll need is [**Unity \>= 2018.2.6f**][3].

# Getting the project started
1. Clone the repository
2. Before opening Unity, open the solution `Wth.ModApi/Wth.ModApi.sln`
3. Build the solution. This will generate the two DLLs `Wth.ModApi[.Editor].dll` and copies them to `Assets/ModTool`. Those contain the Mod API code and will be used by Unity.
4. Open Unity and the project folder `./`
5. In the Inspector make sure that `Wth.ModApi.Editor.dll` will be excluded from any builds
6. That’s it :) If you develop on the Mod API you have to rebuild the solution manually before you can use your changes in Unity and the main project.

# Components
There are four main components:
- The *Main Game* Unity project which is located in this directory.
- The *ModApi* Visual Studio Solution which is located in the `Wth.ModApi` folder. This solution builds two DLLs which are used in the ModCreator Package.
- The *AndroidPlugin* Android Studio project which is located in the `AndroidPlugin` folder. It handles native Android integration such as Notifications. 
- The *Example Mod* Unity Project which will be included later on.

# Creating a Mod
What the Hack allows all kinds of changes to the game. Each mod will be it's own "game mode" with unique skill requirements, employees, skills and missions. This allows you to create a mod with very specific learning content aswell as new gameplay aspects. Here is a list of possible modifications:
- Create unique employees with special behaviour
- Create unique employee behaviour extensions for the default employees
- Create a specific set of skills
- Create missions with custom actions for the player
- Create new items for the office

Before you create a mod, have a look at the [development requirements][4]. After installing Unity, create a new Unity Project and drag the `Wth.ModCreator.unitypackage` into the Editor. 

Get started by using the custom Employee, Skill, Mission and Item Editor windows. 

To export your mod, go to `Tools > ModTool` and hit Export. Your code will be checked for any incompatibilities with the main game.

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
- [NuGetForUnity][14] - MIT

## WTH contains code based on projects
- [Pathfinding][15] - MIT

[1]:	https://unity3d.com/de/get-unity/download/archive
[2]:	#creating-a-mod
[3]:	https://unity3d.com/de/get-unity/download/archive
[4]:	#development-requirements
[5]:	https://github.com/Unity-Technologies/2d-extras
[6]:	https://assetstore.unity.com/packages/slug/93571
[7]:	https://unity3d.com/legal/licenses/Unity_Companion_License
[8]:	https://assetstore.unity.com/packages/essentials/legacy-image-effects-83913
[9]:	https://github.com/hendrik-schulte/UnityEnhanced
[10]:	https://github.com/Hello-Meow/ModTool
[11]:	https://github.com/keijiro/KinoGlitch
[12]:	https://github.com/staffantan/unityglitch
[13]:	http://creativecommons.org/licenses/by/3.0/deed.en_GB
[14]:	https://github.com/GlitchEnzo/NuGetForUnity
[15]:	https://github.com/SebLague/Pathfinding

[image-1]:	Assets/Sprites/ui/GameLogo.png
[image-2]:	Assets/Sprites/ui/base_game_banner.png
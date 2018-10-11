# **What_the_Hack** ModApi

# Development
## Requirements
The only thing you'll need is [**Unity version 2018.2.9f**][1]. It is important that you use exactly the version of Unity that is used to build the game and the mod package.

## How far can I go?
What the Hack allows all kinds of changes to the game. Each mod will be it's own "game mode" with unique skill requirements, employees, skills and missions. This allows you to create a mod with very specific learning content aswell as new gameplay aspects. Here is a list of possible modifications:
- Create unique employees with special behaviours
- Create unique employee behaviour extensions for the default employees
- Create a specific set of skills
- Create missions with custom actions for the player
- Create new items for the office
- Implement multiple difficulty levels

## Getting Started
1. Before you create a mod, have a look at the [development requirements][2]. 
2. After installing Unity, create a new Unity Project and drag the `Wth.ModCreator.unitypackage` into the Editor. 
3. Create a `ModInfo` Scriptable Object: `Assets > Create > What_The_Hack ModApi > Mod Info`. Each mod requires a unique ID and a banner image with the resolution _400\*182_. This scriptable object will be the gathering point for your content.
4. Get started by using the custom Employee, Skill, Mission and Item Creater windows which can be found under `Tools > What_The_Hack ModApi`. 

To export your mod, go to `Tools > ModTool` and hit `Export`. Your code will be checked for any incompatibilities with the main game.

[1]:    https://unity3d.com/de/get-unity/download/archive
[2]:    #requirements
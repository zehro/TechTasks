# TechTasks
4455 Final Project

## Group Members
Andy Xue, me@andrewxue.com, axue9
James Lee, lee.zehro@gatech.edu, slee863
Yingqiong Shi, yshi84@gatech.edu, yshi84
Alex Rushing, rushing3@gatech.edu, rushing3
Liubov Nikolenko, lnikolenko3@gatech.edu, lnikolenko3

## Installation Requirements
None. This is tested with a Windows OS with keyboard AND mouse. Using a mouse is required if the player wants to pause the game. There is controller support but it has not been tested.

## Gameplay Instructions
- Use WASD to move around
- Use the mouse to move the camera around
- Explore, collect stars, and don't die!

## Requirements
- The game is a 3D platformer with a goal (of collecting stars) with a game over screen, a main menu, and replay capabilities.
- The player controller follows with humanoid root motion with fluid animation and a camera that follows the player while changing the orientation of where the player moves. It also has animation events to callback events for sounds/particles.
- There is a 3D world with variable height for exploration. It also has a bridge that moves like a see-saw based on the player's location that is pivoted around a joint.
- There are two types of AI that can hurt the player.
- For polish, there is UI for the game objectives (stars) and features (health). The player has particles when running fast on grass and can only move on certain surfaces. There is also auditory feedback for many events such as jumping, killing enemies, enemies exploding, collecting stars, etc.

## Bugs
- If the player jumps near a wall, they can potentially jump extremely far up
- Sometimes the player's mesh stops flickering/rendering when getting hit.
- The player can potentially get stuck when enemies are directly below the player.

## External Resources
- 3D Models by alecpike
(https://www.models-resource.com/nintendo_64/supermario64/)
- SFX by Deezer
(http://themushroomkingdom.net/media/sm64/wav)
- Background Music Extended Remixes by Venom45000VR and SledgeBro64
(https://www.youtube.com/watch?v=YKtFsLyz6Zo)
(https://www.youtube.com/watch?v=-mtcfkWDbOU)

## Who Did What
Andy Xue: Game Feel Engineer
James Lee: Player Controller, Art/Music Assets
Yingqiong Shi: AI
Alex Rushing: Main Menu Scripting
Liubov Nikolenko: AI

## Scenes to Open
The 'MainMenu' scene in the Scenes folder is the main scene to open.
The 'Play Scene' scene in the Scenes folder is the scene where the player is in a level.

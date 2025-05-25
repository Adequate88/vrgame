# *Intruder*
---
This repository contains the source Unity files for *Intruder*. All files within the `assets` folder are relevant to the structure of the game.

## Overview

In this README we cover the following topics.
- Produced scripts and assets: Created entirely by our team.
- Adapted scripts/assets: Modified from external sources.
- Unmodified scripts/assets: Used as-is.
- Relevant content (scripts, assets, scenes) for each custom feature.

## General Folder Structure

The general structure of our `assets` folder is broken down into the following general structure:
1. Scene folders: one folder for each scene exists. Each of these contains:
  - The scene itself
  - Asset folder containing any objects/meshes from external sources
  - Script folder: containing all relevant scripts
  - Sounds/Audio: relevant sound effects for a scene
  - Materials: If specfic materials were created for the scene they are found here.
  - Prefabs: Any prefabs created for the scene (some of these leak out into other scenes)
2. `Global_Prefabs` folder: contains prefabs common throughout the game.
3. Miscellaneous folders: some folders do not fall into the structure above and are simply left in the assets folder. Some examples are relevant `TextMesh Pro` asset folder or the imported `XR`/`VR` assets.

### Produced Scripts and Assets

- `Miscellaneous_Menu_Scene\`
  - `GameOverMenu.cs`: Handles the appearance of the game over screen
  - `SettingsMenu.cs`: Handles settings for player to choose from, i.e. snap/continuous turn or vignette on/off.
  - `GlobalSettingsManager.cs`: Ensures that the settings chosen are applied to all scenes.
- `Scene0_Tutorial_Entry\`
  - `PuzzleControllerTutorial.cs`: Child of `PuzzleController` - required for allowing player to pass through to the next room when using door.
  - `WaterControllerTutorial.cs`: Child of `WaterController`. Specifcally ensures that the water stays static (since it is not in bathroom).
- `Scene1_Bedroom\`
  - `CamScript.cs`:
  - `Crawling.cs`: Handles player crawling mechanism
  - `Grabbed.cs`:
  - `OpenWithPuzzle.cs`: Child of `PuzzleController`, handles bedroom puzzle completion.
  - `PushButton.cs`: Handles the pressing of buttons for the bedroom puzzle
  - `RespawnManager.cs`:
  - `SocketEvent.cs`:
  - `SpawnCube6.cs`:
- `Scene2_Bathroom\`
  - `PuzzleControllerBathroom.cs`: Child of `PuzzleController`, handles bathroom puzzle.
  - `PuzzleController.cs`: Parent object to handle puzzles of each scene and to allow the player to proceed to next room.
  - `DoorHandler.cs`: Script that handles transitions between scenes when the door is interacted with.
  - `DuckTrigger.cs`: Handles the placements of the duck puzzle to ensure they are in correct order.
  - `RoomStateController.cs`:
  - `WaterController.cs`: Handles water rising and swimming trigger for bathroom.
  - `FadeScreen.cs`: Handles the fading screen when entering and leaving a scene.
  - `Swimming.cs`: Handles swimming movements
- `Scene3_Attic\`
  - `DoorHandlerAttic.cs`: Door handler for attic since it requires a key.
  - `HideController.cs`: Handles hiding mechanism
  - `KeySelected.cs`: Handles key/player interaction
- `Scene4_Kitchen\`
  - `DoorHandlerKitchen.cs`: Child of `DoorHandler` to handle door for kitchen.
  - `OvenBakingController.cs`: Handles when the pot is in the oven and timer for baking to finish.
  - `PlaceIngredientsInPot.cs`: Handles placing of ingredients in pot.
  - `PotStirManager.cs`: Handles the mixing of ingredients in the pot
  - `PlayerSafetyState.cs`: Handles when player is in the safe location to hide from the monster.
  - `PursuerSequence.cs`: Handles monster knocking on door and entering room, and acts as trigger to game over.
  - `StickyNoteFunctionality.cs`: Script for special 'sticky note'-like interaction.
  - `PuzzleControllerKitchen.cs`: Handles when puzzle in kitchen is completed.
- `Scene5_Final_Room\`
  - `DoorHandlerFinalRoom.cs`: Handles transition from final door back to main menu.
  
### Adapted Scripts/Assets:

 - Locker, Wardrobe, Bathtub, Large Fridge: Modified to be openable with hiding mechanic. Also changes color in proximity of player.
 - Bed: Modified so that the asset changes color in proximity of player.
 - Washing Machine, Fridge, Oven: Altered color to indicate that it is openable. Additionally added hinge joints and grab interaction to make it openable.
 - Ducks: Made to be interactable objects
 - Candles: Altered in bathroom puzzle to give a hint.
 - Pot in kitchen: Altered to contain unbaked cake mix when mixing is complete
 - Cake Batter: Flattened out a cake to create the cake mix in the pot referenced above.
 - Spoons: colored to make part of puzzle using different materials.
 - Scenes in Tutorial have images to teach user the movements and buttons - created by getting images and making sprites using the images.

### Unmodified Scripts/Assets:

 - All other assets, except for those mentioned in the previous section are unmodified and taken from the Unity Asset Store.

### Relevant Content (scripts, assets, scenes) for each custom feature.

- Swimming Mechanic: The relevant scripts are `WaterController.cs` and `Swimming.cs`. The relevant objects are the `ocean` and `Swimming` which is found as a child of the XR Origin/Locomotion object.

- Crawling Mechanic:

- Cooking Mechanic: The relevant scripts are `PlaceIngredientsInPot.cs`, `OvenBakingController.cs`, `PotStirManager`, and `PlaceIngredientsInPot.cs`. The cooking mechanic uses functionalities such as grabbing and placing in a `Pot` object, then holding the `Pot` and mixing using a `Spoon` by grabbing the correct `Spoon` and moving it within the `Pot`'s area to "mix" in the ingredients (`Cheese`, `Milk`, `Egg`), to have cake Batter. Step 2 is placing the batter and rotating the knob of the oven and waiting until the cake is complete.

- Hiding Mechanic: The relevant scripts are `HideController.cs` ,`FadeScreen.cs` and `PlayerSafetyState.cs`. For each hiding spot a `SafeZone`, an `ÈxitPosition` and a `HiddenPosition` are determined. Each hiding spot also uses the asset `BreathingSound` as well as an individual opening/closing sound for the particular object.

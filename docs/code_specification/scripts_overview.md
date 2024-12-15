## Scripts Overview

### GameManager.cs

The **GameManager** script controls the overall flow of the game, including:

- Starting and stopping levels
- Managing player resources
- Handling victory and defeat conditions

**Key Methods:**

- `StartGame()`: Initializes the game
- `EndGame()`: Ends the game and displays results


### AttackController.cs

The **AttackController** script controls the overall attack cycles of towers

**Key Methods:**

- `initTowers()`: Initializes the position set of towers
- `xxxAttack()`: Realization of specific kind of attack 


### WaveSpawner.cs

The **WaveController** script controls the overall enemy spawn cycles

**Key Methods:**

- `ReleaseFromQueue()`: Releasing prepared enemy from wave queue
- `AddWaveToQueue()`: Seting up wave queue by generated enemy wave
- `getWave(int waveCnt)`: Wave generating logic
- `isWaveAlive()`: Wave state check
### GridController.cs and PutMenu.cs

The **GridController** and **PutMenu** scripts realizes the overall tower management logic

**GridController Key Methods:**

- `updateTilesEvent(Vector3Int tilePos, int towerId)`: updating grid state event from PutMenu
- `TileMapEvent()`: PutMenu call logic
- `DrawPath()`: Path drawing logic. Draws path tiles from level data

**PutMenu Key Methods:**

- `Put()`: Updating GridController tilemap state logic
- `showUpgradeMenu()`: PutMenu regenerating logic
### Level.cs

Manages the levels by loading JSON configurations and initializing the game scene.

**Key Responsibilities:**

- Parsing level configuration from `StreamingAssets`
- Instantiating towers, enemies, and objectives
### Player.cs

Encapsulates generic player logic e.g. score, health and balance management.

**Key Responsibilities:**

- Parsing level configuration from `StreamingAssets`
- Instantiating towers, enemies, and objectives

### MainMenuController.cs

Controls the main menu UI, including:

- Start Game button
- Level selection
- Exiting the game

### Rocket.cs

Defines the behavior of projectile objects such as rockets.

**Key Features:**

- Movement logic towards target
- Collision detection with enemies
- Damage application

### DatabaseManager.cs

Realizes persistence access features. Uses simple SQLite specific ORM realization

### AuthController.cs

Realizes authentication features e.g. Google Auth, Anonymous auth.

### PathSquarer.cs

Specific helper for path normalization.

### XXXHandlers.cs

Simple proxies for collision/click event catching.

### DebugLogObject.cs

Utility script for debugging purposes during development.

### SerializableStructures.cs

Serialization structures collection. Used by level loader.



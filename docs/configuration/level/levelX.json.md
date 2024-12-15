### Specific Level Configuration (`level1.json`)

The `level1.json` file defines the layout, waves, and game parameters for a specific level.

**Structure:**
```json
{
  "mapSize": { "width": 10, "height": 10 },
  "waves": [30, 50, 90, 110, 130],
  "player": {
    "hp": 11,
    "funds": 1000
  },
  "tiles": [
    { "x": -9, "y": 3, "tileType": 0 },
    ...
  ],
  "path": [
    { "x": 300, "y": 1000 },
    ...
  ],
  "towers": [
    {
      "towerTypeId": 1,
      "tileId": 1,
      "tier": 1,
      "cost": 150,
      "damage": 25,
      "range": 450,
      "cooldown": 1.5,
      "attackType": 1,
      "grades": [2]
    }
  ]
}
```

**Fields Explanation:**

- `mapSize`: Defines the grid dimensions of the level.
    
- `waves`: An array of integers representing the size of each wave of enemies.
    
- `player`: Player-specific settings:
    
    - `hp`: Starting health points.
        
    - `funds`: Initial resources available to the player.
        
- `tiles`: A list of tile objects with their coordinates (`x`, `y`) and `tileType` values:
    
    - `tileType`: Defines the type of the tile (e.g., 0 for walkable path).
        
- `path`: A series of coordinates that define the path enemies will follow.
    
- `towers`: An array defining pre-placed towers on the map. Each tower includes:
    
    - `towerTypeId`: The ID of the tower type.
        
    - `tileId`: The tile where the tower is placed.
        
    - `tier`: The tower's upgrade tier.
        
    - `cost`, `damage`, `range`, `cooldown`: Gameplay parameters.
        
    - `attackType`: The attack behavior of the tower.
        
    - `grades`: Possible upgrade paths for the tower.
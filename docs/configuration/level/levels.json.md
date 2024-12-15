## Configuration Files

### Levels Configuration

Levels are configured using JSON files located in `StreamingAssets`:

- **levels.json**: Contains general information about available levels.
    
- **level1.json**: Example configuration for the first level.
    

**Example** `**levels.json**`**:**
```json
{
  "levels": [
    { "id": 1, "name": "Level 1", "difficulty": "Easy" },
    { "id": 2, "name": "Level 2", "difficulty": "Medium" }
  ]
}
```
#### To add new levels:

1. Create a new JSON file in `StreamingAssets` (e.g., `level2.json`).
2. Add an entry in `levels.json`.
3. Modify `Level.cs` if custom logic is required.

#### To add new towers:

1. Update the Tileset inside `/Pallete` and `/Tileset` folders.
2. Add new tiles inside GridController tile_to_id map.
3. Add new towers in `levelX.json` tower section and include it inside upgrade tree.
4. Modify `AttackConntroller.cs` if custom logic is required.
#### To add new enemies or towers:

1. Create new scripts inheriting from base classes (e.g., `EnemyBase`, `TowerBase`).
2. Update prefabs in Unity Editor.
3. Adjust balance/configuration in level JSON files.

#### Generic tips:
- Ensure all JSON configurations match the expected schema to avoid runtime errors.
- Use `DebugLogObject.cs` to help troubleshoot during development.
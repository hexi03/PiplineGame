using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;


public class MapLoader
{
    public Level level;
    public IEnumerator  LoadMapFromJson(string jsonPath, System.Action callback)
    {
        // Парсинг JSON
        
        JSONLoader loader = new JSONLoader();
        yield return loader.LoadJSON(jsonPath, () =>
        {
            Debug.Log("Загружаемый JSON: " + loader.jsonString);
            DebugLogObject.log("Trying to load level");
            DefaultNamespace.LevelData levelData = JsonUtility.FromJson<DefaultNamespace.LevelData>(loader.jsonString);
            DebugLogObject.log("Loaded: " + levelData.ToString());
            List<TileData> tiles = levelData.tiles.ToList();

            // Создание пути
            List<Vector3> waypoints = new List<Vector3>();
            foreach (PositionData pathNode in levelData.path)
            {
                Vector3 position = new Vector3(pathNode.x, pathNode.y, 0);
                waypoints.Add(position);
            }

            
            List<TowerData> towers = levelData.towers.ToList();
            List<int> waves = levelData.waves.ToList();
            PlayerData playerData = levelData.player;

            level = new Level(waypoints, tiles, towers, waves, playerData);
            DebugLogObject.log("Level loaded: " + level.levelId);
            callback();
        });
        
        
    }
}

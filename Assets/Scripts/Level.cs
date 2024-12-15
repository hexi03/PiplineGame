using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class LevelState
    {
        public bool isCompleted;
        public int maxScore;

        public LevelState(bool isCompleted, int maxScore)
        {
            this.isCompleted = isCompleted;
            this.maxScore = maxScore;
        }
    }
    
    
    
    public class Level
    {
        public int levelId;
        public List<Vector3> waypoints;
        public List<TileData> tiles;
        public List<TowerData> towers;
        public PlayerData playerInitials;
        public List<int> waves;

        public Level(List<Vector3> waypoints, List<TileData> tiles, List<TowerData> towers, List<int> waves, PlayerData player)
        {
            this.levelId = levelId;
            this.playerInitials = player;
            this.waypoints = waypoints;
            this.tiles = tiles;
            this.towers = towers;
            this.waves = waves;
        }

        public TowerData getTowerById(int towerId)
        {
            foreach (var tower in towers)
            {
                if (tower.towerTypeId == towerId) return tower;
            }

            return null;
        }

        public void setId(int id)
        {
            levelId = id;
        }

    }
}
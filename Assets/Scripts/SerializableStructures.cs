using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    [System.Serializable]
    public enum AttackType {
        Disabled,
        Single,
        Multiple,
        Laser
    }
    
    [System.Serializable]
    public class TowerData
    {
        public int towerTypeId;
        public int tileId;
        public int tier;
        public int cost;
        public int damage;
        public int range;
        public float cooldown;
        public AttackType attackType;
        public int[] grades; //towerTypeId грейда
    }


    [System.Serializable]
    public class PlayerData
    {
        public int hp;
        public int funds;
    }

    [System.Serializable]
    public class LevelData
    {
        public MapSize mapSize;
        public TileData[] tiles;
        public PositionData[] obstacles;
        public PositionData[] path;
        public TowerData[] towers;
        public PlayerData player;
        public int[] waves;
    }

    [System.Serializable]
    public class MapSize
    {
        public int width;
        public int height;
    }

    [System.Serializable]
    public class TileData
    {
        public int x;
        public int y;
        public int towerType;
    }

    [System.Serializable]
    public class PositionData
    {
        public int x;
        public int y;
    }
}
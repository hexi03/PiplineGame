using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using Unity.VisualScripting;
using TileData = DefaultNamespace.TileData;

public class Tuple<R,T>{
    public R First;
    public T Second;

    public Tuple(R r, T t){
        First = r;
        Second = t;
    }
}

public class Tower{

    
    public float deltaTime;
    public Vector3Int tilePos;
    public TowerData stats;

    public Tower(Vector3Int tilePos, TowerData stats){
        this.tilePos = tilePos;
        this.stats = stats;
    }
}


public class AttackController : MonoBehaviour
{
    public GridController _gridController;
    public WaveSpawner _waveSpawner;
    public Rocket rocket;
    public  GameManager gameManager;
    
    List<Tower> towerList;
    public Player player;

    private List<GameObject> destroyQueue = new List<GameObject>();
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        initTowers();
    }

    static Vector3 Mult(Vector3 a, Vector3 b){
        return new Vector3(a.x*b.x,a.y*b.y,a.z*b.z);
    }

    static Vector3 Devide(Vector3 a, Vector3 b){
        return new Vector3(a.x/b.x,a.y/b.y,a.z/b.z);
    }

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
        if (!player.isAlive) return;

        foreach (Tower tower in towerList)
        {
            bool onCooldown = tower.deltaTime < tower.stats.cooldown;
            tower.deltaTime += onCooldown ? Time.deltaTime : 0;

            switch (tower.stats.attackType) {
                case AttackType.Single:
                    ////Debug.Log("AttackType Single");
                    singleAttack(tower);
                    break;
                case AttackType.Multiple:
                    multipleAttack(tower);
                    break;
                case AttackType.Laser:
                    laserAttack(tower);
                    break;
            }

        }
        
    }

    void singleAttack(Tower tower) {
        if (tower.deltaTime >= tower.stats.cooldown) {
                tower.deltaTime = 0;
        //Debug.Log("OnSingleAttack");
        foreach (EnemyDataPacket enemy in _waveSpawner.wave)
        {
            if (!enemy.isAlive) continue;
            //Debug.Log(tilemap.CellToLocal(tower.tilePos).ToString());
            ////Debug.Log((tilemap.GetCellCenterLocal((tilemap.origin + tilemap.size))/2 + tilemap.transform.position).ToString());
            //Vector3 tilemapShift = - (Devide(tilemap.CellToWorld(tilemap.origin + tilemap.size) - tilemap.CellToWorld(tilemap.origin) - tilemap.cellSize, 2 * tilemap.transform.localScale) + tilemap.transform.position);
            //Debug.Log("Tilemap Origin in world" + tilemapShift.ToString());
            //- (tilemap.GetCellCenterLocal(tilemap.origin) - tilemap.CellToLocal(tilemap.origin));
            //Vector3 position = Mult(Mult(tower.tilePos + new Vector3(-0.5f,-1,0) + new Vector3(0.5f,0.5f,0), tilemap.cellSize), tilemap.transform.localScale) + tilemapShift;
            Vector3 towerPosition = _gridController.getTileWorldPosByTilemapPos(tower.tilePos);
            Vector3 enemyPosition = enemy.transform.position;
            enemyPosition.z = 0;
            towerPosition.z = 0;
            float distance = Vector3.Distance(enemyPosition,towerPosition);
            Debug.Log("Дистанция до врага: " + distance.ToString() + " (макс " + tower.stats.range + ")");
            if (distance < tower.stats.range){

                Rocket rocket1 = Instantiate(rocket);
                destroyQueue.Add(rocket1.gameObject);
                rocket1.setTarget(towerPosition);
                rocket1.transform.position = enemyPosition;
                rocket1.baseTower = tower.tilePos;
                rocket1.release();
                enemy.hit(tower.stats.damage);
                return;
            }
        }
        }
    }

    

    void multipleAttack(Tower tower)
    {
        if (tower.deltaTime >= tower.stats.cooldown) {
            tower.deltaTime = 0;
        //Debug.Log("OnSingleAttack");
        foreach (EnemyDataPacket enemy in _waveSpawner.wave)
        {

            if (!enemy.isAlive) continue;
            //Debug.Log("enemy");

            //Debug.Log("enemyTimeOK");
            //Debug.Log(tilemap.CellToLocal(tower.tilePos).ToString());
            ////Debug.Log((tilemap.GetCellCenterLocal((tilemap.origin + tilemap.size))/2 + tilemap.transform.position).ToString());
            //Vector3 tilemapShift = - (Devide(tilemap.CellToWorld(tilemap.origin + tilemap.size) - tilemap.CellToWorld(tilemap.origin) - tilemap.cellSize, 2 * tilemap.transform.localScale) + tilemap.transform.position);
            //Debug.Log("Tilemap Origin in world" + tilemapShift.ToString());
            //- (tilemap.GetCellCenterLocal(tilemap.origin) - tilemap.CellToLocal(tilemap.origin));
            //Vector3 position = Mult(Mult(tower.tilePos + new Vector3(-0.5f,-1,0) + new Vector3(0.5f,0.5f,0), tilemap.cellSize), tilemap.transform.localScale) + tilemapShift;
            Vector3 towerPosition = _gridController.getTileWorldPosByTilemapPos(tower.tilePos);
            Vector3 enemyPosition = enemy.transform.position;
            enemyPosition.z = 0;
            towerPosition.z = 0;
            float distance = Vector3.Distance(enemyPosition,towerPosition);
            Debug.Log(distance.ToString());
            if (distance < tower.stats.range){
                Debug.Log("enemyDistanceOK");
                Rocket rocket1 = Instantiate(rocket);
                rocket1.setTarget(towerPosition);
                rocket1.transform.position = enemyPosition;
                rocket1.baseTower = tower.tilePos;
                rocket1.release();
                enemy.hit(tower.stats.damage);

            }
            }
        }
    }

    void laserAttack(Tower tower)
    {

    }

    public void replaceTower(Vector3Int tilePos, TowerData stats)
    {
        Debug.Log("Сработка коллбека замены башни на AttackController " + "Object Instance ID: " + GetInstanceID());
        for (int i = 0; i < towerList.Count; i++)
        {
            if (towerList[i].tilePos == tilePos)
            {
                towerList[i] = new Tower(tilePos, stats);
                return;
            }
        }

        throw new RuntimeWrappedException("Попытка замены несуществующей для AttackController башни");
    }

    public void initTowers()
    {
        Debug.Log("Инициализация башен на AttackController " + "Object Instance ID: " + GetInstanceID());
        List<Tower> towers = new List<Tower>();
        foreach (TileData tile in gameManager.level.tiles)
        {
            
            Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
            //мы ожидаем, что все тайлы - это башни (даже башня 0 - пустая клетка)
            TowerData td = gameManager.level.getTowerById(tile.towerType);
            towers.Add(new Tower(position, td));
        }

        towerList = towers;
    }

    public void OnDestroy()
    {
        for (int i = 0; i < destroyQueue.Count; i++)
        {
            if (!destroyQueue[i].IsDestroyed()) Destroy(destroyQueue[i]);
        }
    }


}

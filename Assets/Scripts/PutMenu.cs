using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PutMenu : MonoBehaviour
{
    public GridController _gridController;
    public Vector3Int tilePosition;
    public TowerData currTowerData;
    
    List<TextMesh> texts = (new ArrayList()).Cast<TextMesh>().ToList();

    private List<GameObject> destroyQueue = new List<GameObject>();
    public Tilemap menuTilemap;
    
    public Player player;

    public GameManager gameManager;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Ивент нажатия по майн тайлмапу
    public IEnumerator TileMapEvent(Vector3 mousePosition) {
        Debug.Log("Обработка логики ивента PutMenu");

        
        Debug.Log("Отработка PutMenu OnEnable");
        tilePosition = _gridController.tilemap.WorldToCell(mousePosition);
        int tileId = _gridController.getTileIdByPos(tilePosition);
        currTowerData = gameManager.level.getTowerById(tileId);
        clearMenu();
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        if (currTowerData != null)
        {
            Debug.Log("Mode.Upgrade");
            showUpgradeMenu();
            
        }
        yield break;

    }

    

    public void Put(int newTileId) //изменение майн тайлмапа
    {
        TowerData towerData = gameManager.level.getTowerById(newTileId);
        if (!player.isAlive) return;
        if (player.getBalance() >= towerData.cost)
        {
            player.subBalance(towerData.cost);
            
            _gridController.updateTilesEvent(tilePosition, newTileId);
        }
    }
    

    public void showUpgradeMenu()
    {
        
        Debug.Log("showUpgradeMenu");
        Debug.Log("ID выбранной башни: " + currTowerData.towerTypeId);
        Debug.Log("Грейды выбранной башни: ");
        for (int i = 0; i < currTowerData.grades.Length; i++)
        {
            int towerId = currTowerData.grades[i];
            TowerData gradeTd = gameManager.level.towers[towerId];
            Debug.Log("Грейд " + towerId);
            Vector3Int loc = new Vector3Int(i, 0, 0);
            Tile gradeTile = _gridController.getTileByTileId(gradeTd.tileId);
            if (gradeTile == null)
            {
                throw new RuntimeWrappedException("В тайлсете GridController не найден тайл для грейда (id: " + gradeTd.tileId + ")");
            }

            menuTilemap.SetTile(loc, gradeTile);
            Vector3 scaledPos = Scaled(menuTilemap.CellToLocal(loc), menuTilemap.transform.lossyScale);
            Vector3 scaledSize = Scaled(menuTilemap.cellSize, menuTilemap.transform.lossyScale);
            Vector3 textpos = scaledPos + new Vector3(scaledSize.x / 2.0f, 0, 0);
            Vector3 textScale = Scaled(new Vector3(0.25f,0.25f,1.0f), menuTilemap.transform.lossyScale);
            textpos.z = 0;

            TextMesh text = new GameObject().AddComponent(typeof(TextMesh)) as TextMesh;
            text.transform.Translate(menuTilemap.transform.position + textpos);
            text.color = Color.yellow;
            text.transform.localScale = textScale;
            text.anchor = TextAnchor.MiddleCenter;
            destroyQueue.Add(text.gameObject);
            //text.transform.SetParent(this.transform);
            text.text = gradeTd.cost.ToString();
            texts.Add(text);
            
            
        }
    }
    //Обработка нажатия по менюшке тайлмапа
    public void PutMenuTilemapEvent()
    {
        Debug.Log("Сработка коллизии PutMenu");
        
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Vector3Int tilePosition1 = menuTilemap.WorldToCell(worldPosition);
        Debug.Log("MenuEvent");
        

        Tile tile = menuTilemap.GetTile<Tile>(tilePosition1);

        if (tile != null)
        {
            transform.position = new Vector3(100, 100, transform.position.z);
            clearMenu();
            Put(currTowerData.grades[tilePosition1.x]); //скорее фича - у нас x координаты тайлмапа связаны с массивом грейдов

        }
        else
        {
            Debug.Log("Не найден тайл в тайлмапе PutMenu для позиции " + tilePosition1);
        }

        gameObject.SetActive(false);

    }

    public void clearMenu() { 
        menuTilemap.ClearAllTiles();
        foreach (TextMesh item in texts)
        {
            Destroy(item.gameObject);
        }
        texts.Clear();
    }
    
    //Коллайдер удален
    private void OnMouseDown()
    {
        Debug.Log("Empty space clicked");
        gameObject.SetActive(false);
        
    }

    Vector3 Scaled(Vector3 baseV, Vector3 scale)
    {
        return new Vector3(baseV.x * scale.x, baseV.y * scale.y, baseV.z * scale.z);
    }
    
    public void OnDestroy()
    {
        for (int i = 0; i < destroyQueue.Count; i++)
        {
            if (!destroyQueue[i].IsDestroyed()) Destroy(destroyQueue[i]);
        }
    }
}

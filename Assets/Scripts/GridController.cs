using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = DefaultNamespace.TileData;

public delegate void UpdateTowerCallback(Vector3Int tilePos, TowerData stats);

public class GridController : MonoBehaviour
{

    public PutMenu putMenu;

    public  GameManager gameManager;
    public AttackController attackController;
    public Player player;
    public Tilemap tilemap;
    public Tilemap bgTilemap;
    public Tile[] tileset;
    public Tile pathTile;
    public Tile cornerPathTile;
    public Tile endPathTile;
    public Tile emptTile;
    public GameObject debugSpherePrefab;  // Префаб сферы для отладки
    public int tilemapWidth;
    public int tilemapHeight;
    
    public int tilemapXOffset;
    public int tilemapYOffset;




    void Start()
    {
        foreach (TileData tile in gameManager.level.tiles)
        {
            
            Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
            //мы ожидаем, что все тайлы - это башни (даже башня 0 - пустая клетка)
            TowerData td = gameManager.level.getTowerById(tile.towerType);
            Debug.Log("Установка тайла: Позиция: " + position + " ID тайла: " + td.tileId);
            tilemap.SetTile(position, tileset[td.tileId]);
        }
        

        DrawPath(gameManager.level.waypoints);

    }

    public void TileMapEvent()
    {
        Debug.Log("Сработка коллайдера GridController");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (!player.isAlive) return;
        
        putMenu.gameObject.SetActive(true);
        StartCoroutine(putMenu.TileMapEvent(mousePosition));
    }


    public void updateTilesEvent(Vector3Int tilePos, int towerId){
        Debug.Log("Вызов ивента обновления тайла " + tilePos + " на " + towerId);
        Debug.Log("tilePos" + tilePos.ToString());
        TowerData stats = gameManager.level.getTowerById(towerId);
        tilemap.SetTile(tilePos, getTileByTileId(stats.tileId));
        attackController.replaceTower(tilePos, stats);
    }
    

    public int getTileIdByPos(Vector3Int tilePos)
    {
        Tile tile = getTileByPos(tilePos);
        if (tile == null) return -1;
        for (int i = 0; i < tileset.Length; i++)
        {
            if (tile.Equals(tileset[i])) return i;
        }
        return -1;
    }

    public Tile getTileByPos(Vector3Int tilePos)
    {
        return tilemap.GetTile<Tile>(tilePos);
    }

    public Tile getTileByTileId(int tileId)
    {
        if (tileId >= tileset.Length) return null;
        return tileset[tileId];
    }

    Vector3 Scaled(Vector3 baseV, Vector3 scale)
    {
        return new Vector3(baseV.x * scale.x, baseV.y * scale.y, baseV.z * scale.z);
    }
    public Vector3 getBGTilemapCellSize()
    {
        return Scaled((tilemap.cellSize), tilemap.transform.localScale);
    }
    
    public Vector3 getTileWorldPosByTilemapPos(Vector3Int tilePos)
    {
        return tilemap.CellToWorld(tilePos) + getBGTilemapCellSize()/2;
    }
    
    public Vector3 getTileWorldPosByBGTilemapPos(Vector3Int tilePos)
    {
        return bgTilemap.CellToWorld(tilePos) + getBGTilemapCellSize()/2;
    }
    
    // Метод для отрисовки пути с интерполяцией на Tilemap
    public void DrawPath(List<Vector3> path)
    {
        Vector3 tileSize = getBGTilemapCellSize();
        FillTilemap(tilemapXOffset,tilemapYOffset, tilemapWidth, tilemapHeight);
            
        // Проходим по всем точкам пути, кроме последней
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 startPoint = path[i];
            Vector3 endPoint = path[i + 1];
            
            if (debugSpherePrefab != null)
            {
                Instantiate(debugSpherePrefab, path[i], Quaternion.identity);
            }

            // Интерполируем сегмент пути между двумя точками с шагом tileSize
            foreach (Vector3 interpolatedPoint in InterpolatePath(startPoint, endPoint, tileSize / 2))
            {

                // Переводим мировые координаты в координаты тайловой карты
                Vector3Int tilePos = bgTilemap.WorldToCell(interpolatedPoint);

                // Вычисляем направление между соседними точками
                Vector3 direction = endPoint - startPoint;

                // Проверяем, является ли путь вертикальным (т.е. движемся по оси Y)
                bool isVertical = Mathf.Abs(direction.y) > Mathf.Abs(direction.x);

                // Определяем матрицу преобразования для поворота тайла
                Matrix4x4 tileTransform = Matrix4x4.identity;
                if (!isVertical)
                {
                    // Поворачиваем тайл на 90 градусов для вертикальных путей
                    tileTransform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90));
                }

                // Устанавливаем заданный тайл на вычисленные координаты с нужным поворотом
                bgTilemap.SetTile(tilePos, pathTile);
                bgTilemap.SetTransformMatrix(tilePos, tileTransform);
                
                 
            }
        }
        
        // Теперь обрабатываем углы
        for (int i = 1; i < path.Count - 1; i++)
        {
            Vector3Int prevPos = bgTilemap.WorldToCell(path[i - 1]);
            Vector3Int currentPos = bgTilemap.WorldToCell(path[i]);
            Vector3Int nextPos = bgTilemap.WorldToCell(path[i + 1]);

            // Вычисляем угловой тайл на основе трех последовательных точек
            if (IsCorner(prevPos, currentPos, nextPos))
            {
                float angle = GetCornerAngle(prevPos, currentPos, nextPos) + -90.0f;
                Matrix4x4 tileTransform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, angle));

                // Устанавливаем угловой тайл
                bgTilemap.SetTile(currentPos, cornerPathTile);
                bgTilemap.SetTransformMatrix(currentPos, tileTransform);
            }
        }
        
    }

    // Метод для интерполяции между двумя точками с заданным шагом
    private IEnumerable<Vector3> InterpolatePath(Vector3 start, Vector3 end, Vector3 step)
    {
        // Вычисляем разницу между точками
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);
        
        // Рассчитываем количество шагов для интерполяции
        int stepsCount = Mathf.CeilToInt(distance / step.magnitude);

        for (int i = 0; i <= stepsCount; i++)
        {
            // Линейная интерполяция между двумя точками с шагом
            float t = (float)i / stepsCount;
            Vector3 interpolatedPoint = Vector3.Lerp(start, end, t);

            // Корректируем координаты по осям в соответствии с размером клетки (tileSize)
            interpolatedPoint = new Vector3(
                Mathf.Round(interpolatedPoint.x / step.x) * step.x,
                Mathf.Round(interpolatedPoint.y / step.y) * step.y,
                Mathf.Round(interpolatedPoint.z / step.z) * step.z
            );
            interpolatedPoint.z = 0;
            yield return interpolatedPoint;
        }
    }

    // Проверка, является ли точка углом на основе трех последовательных точек
    private bool IsCorner(Vector3Int prev, Vector3Int current, Vector3Int next)
    {
        Vector3 dir1 = current - prev;
        Vector3 dir2 = next - current;

        // Используем векторное произведение для определения угла
        float crossProduct = Vector3.Cross(dir1, dir2).z; // Z-компонента для 2D

        // Если векторное произведение ненулевое, значит угол не прямой
        return Mathf.Abs(crossProduct) > Mathf.Epsilon;
    }

    private float GetCornerAngle(Vector3Int prev, Vector3Int current, Vector3Int next)
    {
        Vector3Int dir1 = prev - current;
        Vector3Int dir2 = next - current;

        // Сценарии для углов (левый верхний, правый верхний, правый нижний, левый нижний)
        
        if (dir1.y > 0 && dir2.x > 0 || dir1.x > 0 && dir2.y > 0)
        {
            return 180f;
        }
        if (dir1.y < 0 && dir2.x > 0 || dir1.x > 0 && dir2.y < 0 )
        {
            return 90f;
        }
        if (dir1.y < 0 && dir2.x < 0 || dir1.x < 0 && dir2.y < 0 )
        {
            return 0f;
        }
        if (dir1.y > 0 && dir2.x < 0 || dir1.x < 0 && dir2.y > 0 )
        {
            return 270f;
        }

        return 0f; // По умолчанию (если не распознали угол)
    }
    
    public void FillTilemap(int xOff, int yOff, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(xOff + x, yOff + y, 0);
                bgTilemap.SetTile(tilePosition, emptTile); // Устанавливаем тайл
            }
        }
    }
}

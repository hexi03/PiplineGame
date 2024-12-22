using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{
    public static Vector2 bottomLeft;

    public static Vector2 topRight;

    
    public enum TopBarAction
    {
        Stop
    }

    public Level level;
    
    public GridController gridController;
    public ResutlOverlay resutlOverlay;
    public MainMenuController mainMenuController;
    public DatabaseManager databaseManager;

    public void Start()
    {
        //Обработка уровня
        level.waypoints = PathSquarer.SquarePath(level.waypoints, gridController.getBGTilemapCellSize());
        Debug.Log("waypoints выровнены");
        DebugLogObject.log("waypoints aligned");
    }
    public void StartGame(Level level)
    {
        DebugLogObject.log("Starting game");
        resutlOverlay.hide();
        this.level = level;
        gameObject.SetActive(true);
        Debug.Log("GameManager Start");
        DebugLogObject.log("GameManager Start");
    }


    
    
    public void StopGame()
    {
        Debug.Log("Блокирование игры");
        Destroy(gameObject);
        mainMenuController.returnToMenu();
        
    }
    
    public void win(int score)
    {
        // Отображаем результат на оверлее
        resutlOverlay.win(score);
    
        // Добавляем текущий счёт как новую попытку
        int attemptNumber = databaseManager.GetAttemptsForLevel(level.levelId).Count + 1;
        databaseManager.AddScore(level.levelId, score, attemptNumber);

        // Пересчитываем максимальный счёт на основе всех попыток
        var scores = databaseManager.GetAttemptsForLevel(level.levelId);
        int maxScore = scores.Count > 0 ? scores.Max(s => s.Score) : score;

        // Обновляем состояние уровня с новым максимальным счётом
        databaseManager.UpdateLevelState(level.levelId, true, maxScore);
    }

    public void gameOver(int score)
    {
        resutlOverlay.gameOver(score);
        // Добавляем текущий счёт как новую попытку
        int attemptNumber = databaseManager.GetAttemptsForLevel(level.levelId).Count + 1;
        databaseManager.AddScore(level.levelId, score, attemptNumber);

        // Пересчитываем максимальный счёт на основе всех попыток
        var scores = databaseManager.GetAttemptsForLevel(level.levelId);
        int maxScore = scores.Count > 0 ? scores.Max(s => s.Score) : score;
    }
    
    public void MenuEvent(TopBarAction action)
    {
        switch (action)
        {
            case TopBarAction.Stop:
                StopGame();
                break;
            default:
                break;
        }
    }
    

}

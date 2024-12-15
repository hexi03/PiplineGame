using System.Collections;
using System.Collections.Generic;
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
        resutlOverlay.win(score);
        databaseManager.UpdateLevelState(level.levelId, true, score);
    }

    public void gameOver(int score)
    {
        resutlOverlay.gameOver(score);
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

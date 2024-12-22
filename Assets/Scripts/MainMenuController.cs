using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject levelButtonContainer;
    public LevelManager levelManager;
    
    public GameObject statsPanel;
    public StatisticsMenu statsMenu;


    public AuthController authController;
    public enum MenuAction
    {
        Start,
        Stats,
        Settings,
        Help,
        Auth,
        Exit
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void hide()
    {
        gameObject.SetActive(false);
    }

    void show()
    {
        gameObject.SetActive(true);
    }
    
    public void StartGame()
    {
        levelButtonContainer.SetActive(true);
        levelManager.Reload();
    }
    
    public void ShowStats()
    {
        statsPanel.SetActive(true);
        statsMenu.Reload();
    }
    
    public void QuitGame()
    {
        Debug.Log("Game is exiting");
        DebugLogObject.log("Game is exiting");
        #if UNITY_STANDALONE || UNITY_ANDROID
                Application.Quit();
        #endif
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void Auth()
    {
        DebugLogObject.log("Trying to SignInWithGoogle");
        authController.SignInWithGoogle();
    }

    public void MenuEvent(MenuAction action)
    {
        Debug.Log("Сработка MenuEvent");
        switch (action)
        {
            case MenuAction.Start:
                hide();
                StartGame();
                break;
            case MenuAction.Stats:
                hide();
                ShowStats();
                break;
            case MenuAction.Exit:
                QuitGame();
                break;
            case MenuAction.Auth:
                Auth();
                break;
            default:
                break;
        }
    }

    public void returnToMenu()
    {
        levelButtonContainer.SetActive(false);
        gameObject.SetActive(true);
    }
}

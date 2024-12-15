using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuEventHandlers : MonoBehaviour
{
    public String actionName;

    public MainMenuController mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnMouseDown()
    {
        Debug.Log("Сработка коллайдера MenuEventHandlers");
        MainMenuController.MenuAction action;
        if (!Enum.TryParse(actionName, true, out action)) throw new RuntimeWrappedException("Trying to parse unregistered main menu event name");
        
        mainMenu.MenuEvent(action);
            
    }
    
}

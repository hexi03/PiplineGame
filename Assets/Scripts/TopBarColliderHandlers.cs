using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TopBarColliderHandlers : MonoBehaviour
{
    public String actionName;
    public GameManager _gameManager;
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
        Debug.Log("Сработка коллайдера TopBarEventHandlers");
        GameManager.TopBarAction action;
        if (!Enum.TryParse(actionName, true, out action)) throw new RuntimeWrappedException("Trying to parse unregistered top bar event name");
        
        _gameManager.MenuEvent(action);
            
    }
}

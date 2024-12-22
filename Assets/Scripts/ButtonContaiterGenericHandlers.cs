using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
public class ButtonContaiterGenericHandlers : MonoBehaviour
{

    public enum Actions
    {
        Return
    }

    public String actionName;

    public GameObject mainMenu;

    public GameObject panel;

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
        Actions action;
        if (!Enum.TryParse(actionName, true, out action))
            throw new RuntimeWrappedException("Trying to parse unregistered LBC generic event name");

        switch (action)
        {
            case Actions.Return:
                mainMenu.SetActive(true);
                panel.SetActive(false);
                break;
            default:
                break;
        }

    }
}

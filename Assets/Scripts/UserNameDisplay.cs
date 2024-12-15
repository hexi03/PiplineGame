using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserNameDisplay : MonoBehaviour
{
    
    public AuthController authController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TMP_Text>().text = (authController.getUserName());    
    }
}

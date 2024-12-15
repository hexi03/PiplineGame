using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public TextMesh coinLabel;
    public TextMesh healthLabel;

    public Player player;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            coinLabel.text = player.getBalance().ToString();
            healthLabel.text = player.getHealth().ToString();
        }
        
    }
}

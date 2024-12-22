using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResutlOverlay : MonoBehaviour
{
    public GameManager gameManager;
    public TextMesh resultLabel;
    public TextMesh scoreLabel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void win(int score)
    {
        gameObject.SetActive(true);
        Color fill = Color.green;
        fill.a = 0.5f;
        fill.g = 0.9f;
        GetComponent<Image>().color = fill;
        resultLabel.text = "Пакеты успешно утилизированы ^_-";
        scoreLabel.text = "Счет: " + score.ToString();
    }

    public void gameOver(int score)
    {
        gameObject.SetActive(true);
        Color fill = Color.red;
        fill.a = 0.5f;
        fill.r = 0.9f;
        GetComponent<Image>().color = fill;
        resultLabel.text = "Канал потерянных сообщений перегружен >:/";
        scoreLabel.text = "Счет: " + score.ToString();
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }
    
    public void OnMouseDown()
    {
        Debug.Log("Сработка коллайдера ResutlOverlay");
        gameManager.StopGame();
            
    }
}

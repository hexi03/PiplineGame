using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int balance;
    int health;
    int score;
    public bool isAlive = true;

    public GameManager gameManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        balance = gameManager.level.playerInitials.funds;
        health = gameManager.level.playerInitials.hp;
        score = 0;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void win()
    {
        gameManager.win(score);
    }

    public void gameOver()
    {
        gameManager.gameOver(score);
    }

    public void hit(int damage){
        if (health > damage){
            health -= damage;
        }else{
            isAlive = false;
            gameOver();
        }
    }
    public int getBalance()
    {
        return balance;
    }
    public void addBalance(int reward){
        balance += reward;
    }
    
    public void subBalance(int cost){
        balance -= cost;
    }
    public void addScore(int dScore){
        score  += dScore;
    }

    public int getHealth()
    {
        return health;
    }

    public void Reset()
    {
        balance = 0;
        health = 0;
        score = 0;
        isAlive = false;
        
    }
}

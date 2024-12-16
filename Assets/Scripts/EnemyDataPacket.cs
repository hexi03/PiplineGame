using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyDataPacket : MonoBehaviour
{
    public float baseSpeed;
    public float baseSpriteSize;
    public float maxSpeed;
    public float maxSpriteSize;

    public float speedModifier;
    public float spriteSizeModifier;
    public float healthModifier;
    public int moneyModifier;
    public int baseReward;
    public int dificulty;

    public float speed;
    public float spriteSize;
    public int health;
    public int killReward;
    
    public int currentPathNode;

    public List<Vector3> waypoints;
    
    public bool isAlive = true;
    public float lifetime = 0.0f;
    Player player;

    
    // Update is called once per frame

    void Start()
    {
        transform.position = waypoints[0];
    }

    void Update()
    {
        lifetime += Time.deltaTime;
        //killReward = baseReward + moneyModifier * (int)Math.Pow(dificulty, 0.5);
        //speed = Math.Min(maxSpeed, baseSpeed + (float)Math.Pow(speedModifier, 1.0/dificulty));
        if (!isAlive) return;
        if (!player.isAlive) return;
        spriteSize = Math.Min(maxSpriteSize, baseSpriteSize + spriteSizeModifier * (float)Math.Pow(health/healthModifier, 0.5));
        transform.localScale = new Vector3(1,1,1) * spriteSize;

        
        Vector3 moveVec = (waypoints[currentPathNode] - waypoints[currentPathNode - 1]).normalized * speed * Time.deltaTime;
        transform.Translate(moveVec);


        float d1 = Vector3.Distance(transform.position, waypoints[currentPathNode - 1]);
        float d2 = Vector3.Distance(waypoints[currentPathNode], waypoints[currentPathNode - 1]);

        if (d1 >= d2) {
            currentPathNode++;
            transform.position = waypoints[currentPathNode - 1];
        };

        if (currentPathNode == waypoints.Count) {
            isAlive = false;
            GetComponent<Renderer>().enabled = false;

            player.hit(1);
            player.addBalance(killReward);

        }
    } 

    public void Init(Player player, int dificulty, List<Vector3> waypoints)
    {
        Debug.Log("Enemy Init");
        this.player = player;
        currentPathNode = 1;
        this.waypoints = waypoints;
        this.dificulty = dificulty;
        isAlive = true;

        killReward = baseReward + moneyModifier * (int)Math.Pow(dificulty, 0.5);
        speed = Math.Min(maxSpeed, baseSpeed + (float)Math.Pow(speedModifier, 1.0/dificulty));

        health = (int) (dificulty * healthModifier);
        gameObject.SetActive(true);

    }

    public void hit(int damage){
        if (health > damage){
            health -= damage;
        }else{
            isAlive = false;
            Destroy(gameObject);
            player.addBalance(killReward);
            player.addScore((int)(killReward * Math.Pow(1.01, lifetime)));
            
        }
    }
}

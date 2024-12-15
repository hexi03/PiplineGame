using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Rocket : MonoBehaviour
{


    public float accuracy;
    public float maxSpeed;
    public Vector3 tower;
    public Vector3 speed;
    public int damage;
    public int initialSpeed;

    public Vector3Int baseTower;

    public Player player;
    
    public bool isReleased = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = (tower - this.transform.position).normalized * initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReleased) return;
        if (!player.isAlive) return;
        float deltaTime = Time.deltaTime;

        speed += (tower - this.transform.position).normalized * accuracy * deltaTime;
        speed.z = 0;
        speed = Vector3.ClampMagnitude(speed, maxSpeed);
        transform.Translate(speed * deltaTime);
    }

    static Vector3 Mult(Vector3 a, Vector3 b){
        return new Vector3(a.x*b.x,a.y*b.y,a.z*b.z);
    }

    static Vector3 Devide(Vector3 a, Vector3 b){
        return new Vector3(a.x/b.x,a.y/b.y,a.z/b.z);
    }

    public void setAccuracy(float accuracy){
        this.accuracy = accuracy;
    }


    public void setTarget(Vector3 tower){
        this.tower = tower;
    }

    public void release() {
        speed = new Vector3(0.0f,0.0f,0.0f);
        isReleased = true;
        gameObject.SetActive(true);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        
        Debug.Log("Rocket collisionn with: " + collision.gameObject.name);
        //Debug.Log("!!!Коллизия перманентно отключена");return;
        
        Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>() as Tilemap;
        if (tilemap == null) return;
        if (tilemap.WorldToCell(collision.GetContact(0).point) == baseTower)
        Destroy(this.gameObject);
    }
}

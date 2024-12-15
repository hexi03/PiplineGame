using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public EnemyDataPacket enemy;
    public  GameManager gameManager;
    public  Player _player;
    
    public float spawnDelay;

    List<int> queue;
    public  List<EnemyDataPacket> wave = new List<EnemyDataPacket>();

    public float timeDeltaSum = 0;

    int waveNum = 0;
    
    public int sumDificultyProgressionSpeed;
    public int initDificultyValue;

    static System.Random r = new System.Random();

    private bool winLock = true;

    

    // Start is called before the first frame update
    void Start()
    {
        waveNum = 0;
        // enemy.GetComponent<SpriteRenderer>().enabled = false;
        // enemy.transform.position = level.waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        

        timeDeltaSum += Time.deltaTime;
        if (queue != null && queue.Count > 0) {
            if (timeDeltaSum >= spawnDelay) {
                Debug.Log(timeDeltaSum);
                timeDeltaSum = 0;
                ReleaseFromQueue();
                
            }
        }
        if (!isWaveAlive() && (queue == null || queue.Count == 0)) {
            waveNum++;
            timeDeltaSum = 0;
            
            if (isLevelCompleted())
            {
                if (winLock){
                    winLock = false;
                    _player.win();
                }
                return;
            }
            
            AddWaveToQueue();
        }
    }

    bool isWaveAlive() {
        for (int i = 0; i < wave.Count; i++)
        {
            if (wave[i].isAlive) return true;
        }
        return false;
    }

    void ReleaseFromQueue() {
        Debug.Log("Releasing");
        EnemyDataPacket enemy1 = Instantiate(enemy);
        enemy1.Init(_player, queue[0], gameManager.level.waypoints);
        queue.RemoveAt(0);

        enemy1.GetComponent<Renderer>().enabled = true;
        wave.Add(enemy1);
        
    }

    void AddWaveToQueue() {
        queue = getWave(waveNum);
    }

    public List<int> getWave(int waveCnt)
    {
        int sumDificulty = gameManager.level.waves[waveCnt];//initDificultyValue + waveCnt * sumDificultyProgressionSpeed;
        
        List<int> res = (new ArrayList()).Cast<int>().ToList();

        int a;

        while (sumDificulty > 0) {
            a = r.Next (1, sumDificulty);
            sumDificulty -= a;
            res.Add(a);
        }

        return res;
    }

    bool isLevelCompleted()
    {
        return waveNum >= gameManager.level.waves.Count;
    }

    public void OnDestroy()
    {
        for (int i = 0; i < wave.Count; i++)
        {
            if ((wave[i].gameObject != null) && (!wave[i].gameObject.IsDestroyed())) Destroy(wave[i].gameObject);
        }
    }
}

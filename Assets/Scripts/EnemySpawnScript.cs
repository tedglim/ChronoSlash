using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    private Transform[] spawnLocations;
    public float initSpawnNum = 2;
    public GameObject[] enemyTypes;
    public float spawnIntervalDuration = 2.0f;
    private float currentSpawnTime;
    private int enemiesAlive;
    public int maxEnemiesAlive = 10;
    private Vector3 enemy01Offset;
    public bool isGameOver;


    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive = 0;
        GetSpawnLocations();
        InitialSpawn();
        currentSpawnTime = spawnIntervalDuration;
        enemy01Offset = new Vector3 (0.0f, .25f, 0.0f);
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            GetEnemiesAlive();
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        GameObject newEnemy;
        if (currentSpawnTime <= 0.0f)
        {
            if(enemiesAlive < maxEnemiesAlive)
            {
                int rand = UnityEngine.Random.Range(0,enemyTypes.Length);
                if (rand == 0)
                {
                newEnemy = Instantiate(enemyTypes[rand], spawnLocations[UnityEngine.Random.Range(0,spawnLocations.Length)].position, Quaternion.identity);
                } else 
                {
                    newEnemy = Instantiate(enemyTypes[rand], spawnLocations[UnityEngine.Random.Range(0,spawnLocations.Length)].position + enemy01Offset, Quaternion.identity);
                }
                newEnemy.transform.parent = transform;
            }
            currentSpawnTime = spawnIntervalDuration;
        } else
        {
            currentSpawnTime -= Time.deltaTime;
        }
    }

    private void GetEnemiesAlive()
    {
        enemiesAlive = transform.childCount;
        Debug.Log("Enemies alive: " + enemiesAlive);
    }

    private void GetSpawnLocations()
    {
        int i = 0;
        GameObject spawnLocationParent = GameObject.Find("EnemySpawnLocations");
        spawnLocations = new Transform[spawnLocationParent.transform.childCount];
        foreach (Transform child in spawnLocationParent.transform)
        {
            spawnLocations[i] = child;
            i += 1;
        }
    }

    private void InitialSpawn()
    {
        GameObject enemy00 = enemyTypes[0];
        GameObject newEnemy;
        List<int> unique = new List<int>();
        for (int i = 0; i < initSpawnNum; i++)
        {
            
            int rand = UnityEngine.Random.Range(0, spawnLocations.Length - 1);
            while (unique.Contains(rand))
            {
                rand = UnityEngine.Random.Range(0, spawnLocations.Length - 1);
            }
            unique.Add(rand);
            newEnemy = Instantiate(enemy00, spawnLocations[rand].position, Quaternion.identity);
            newEnemy.transform.parent = transform;
        }
    }
}

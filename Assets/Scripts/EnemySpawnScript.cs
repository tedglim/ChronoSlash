using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    private Transform[] spawnLocations;
    public float initSpawnNum = 2;
    public GameObject[] enemyTypes;
    public GameObject[] enemyEntryTypes;
    public float spawnIntervalDuration = 2.0f;
    private float currentSpawnTime;
    private int enemiesAlive;
    public int maxEnemiesAlive = 10;
    private Vector3 enemy01Offset;
    public bool isGameOver;
    public float entryTime = 1.5f;

    public int group1 = 0;
    public int group2 = 0;
    public int group3 = 0;

    private GameManagerScript gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive = 0;
        GetSpawnLocations();
        InitialSpawn();
        currentSpawnTime = spawnIntervalDuration;
        enemy01Offset = new Vector3 (0.0f, .25f, 0.0f);
        isGameOver = false;
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
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
        if (currentSpawnTime <= 0.0f)
        {
                StartCoroutine(makeEnemy());
            if (gameManagerScript.score >= 150)
            {
                StartCoroutine(makeEnemy());
            } else if (gameManagerScript.score >= 60)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    StartCoroutine(makeEnemy());
                }
            }
        } else
        {
            currentSpawnTime -= Time.deltaTime;
        }
    }

    private IEnumerator makeEnemy ()
    {
        GameObject newEnemy;
        GameObject entryEffect;
        currentSpawnTime = spawnIntervalDuration;
        if(enemiesAlive < maxEnemiesAlive)
        {
            int randEnemy = UnityEngine.Random.Range(0,enemyTypes.Length);
            if (gameManagerScript.score >= 180 && randEnemy == 0)
            {
                randEnemy = UnityEngine.Random.Range(0,enemyTypes.Length);
            } else if (gameManagerScript.score >= 150 && randEnemy == 1)
            {
                randEnemy = 0;
            } else if (gameManagerScript.score >= 30 && randEnemy == 0)
            {
                randEnemy = UnityEngine.Random.Range(0,enemyTypes.Length);
            }
            int randSpawnLoc = DistributeEnemies();
            if (randEnemy == 0)
            {
                entryEffect = Instantiate(enemyEntryTypes[randEnemy], spawnLocations[randSpawnLoc].position, Quaternion.identity);
                yield return new WaitForSeconds(entryTime);
                Destroy(entryEffect);
                newEnemy = Instantiate(enemyTypes[randEnemy], spawnLocations[randSpawnLoc].position, Quaternion.identity);
            } else 
            {
                entryEffect = Instantiate(enemyEntryTypes[randEnemy], spawnLocations[randSpawnLoc].position, Quaternion.identity);
                yield return new WaitForSeconds(entryTime);
                Destroy(entryEffect);
                newEnemy = Instantiate(enemyTypes[randEnemy], spawnLocations[randSpawnLoc].position + enemy01Offset, Quaternion.identity);
            }
            newEnemy.transform.parent = transform;
            if (randSpawnLoc <= 2)
            {
                newEnemy.GetComponent<EnemyScript>().group = 1;
                group1++;
                // Debug.Log("Group 1: " + group1);
            } else if (randSpawnLoc >=3 && randSpawnLoc <= 5)
            {
                newEnemy.GetComponent<EnemyScript>().group = 2;
                group2++;
                // Debug.Log("Group 2: " + group2);
            } else if (randSpawnLoc >= 6)
            {
                newEnemy.GetComponent<EnemyScript>().group = 3;
                group3++;
                // Debug.Log("Group 3: " + group3);
            }
        }
    }

    private int DistributeEnemies()
    {
        while (true)
        {
            int rand = UnityEngine.Random.Range(0,spawnLocations.Length);
            if (rand <= 2 && group1 + 2 <= 4)
            {
                return rand;
            } else if (rand >=3 && rand <= 5 && group2 + 2 <= 4)
            {
                return rand;
            } else if (rand >=6 && group3 + 2 <= 4)
            {
                return rand;
            } else {
                // Debug.Log("reRandomize");
            }
        }
    }

    private void GetEnemiesAlive()
    {
        enemiesAlive = transform.childCount;
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
            
            int rand = UnityEngine.Random.Range(3, spawnLocations.Length);
            while (unique.Contains(rand))
            {
                rand = UnityEngine.Random.Range(3, spawnLocations.Length);
            }
            unique.Add(rand);
            newEnemy = Instantiate(enemy00, spawnLocations[rand].position, Quaternion.identity);
            newEnemy.transform.parent = transform;
            if (rand <= 5)
            {
                newEnemy.GetComponent<EnemyScript>().group = 2;
                group2++;
            } else if (rand <= 8)
            {
                newEnemy.GetComponent<EnemyScript>().group = 3;
                group3++;
            }
        }
    }

    public void DestroyAllEnemies(float damageDealt)
    {
        foreach (Transform child in transform)
        {
            // Debug.Log("Destroy");
            child.gameObject.GetComponent<Enemy00Script>().GetDamaged(damageDealt);
            // Destroy(child.gameObject);
        }
    }
}

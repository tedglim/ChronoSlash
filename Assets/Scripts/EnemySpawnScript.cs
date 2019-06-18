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
    public float spawnIntervalDuration = 1.5f;
    private float currentSpawnTime;
    private int enemiesAlive;
    public int maxEnemiesAlive = 9;
    private int spawnNum;
    private Vector3 enemy01Offset;
    private Vector3 enemy02Offset;
    public bool isGameOver;
    public float entryTime = 1.5f;

    private int group1;
    private int group2;
    private int group3;
    public int group1Count;
    public int group2Count;

    public int group3Count;
    private bool isSpawning;

    public int spawnPhase1Start = 5;
    public int spawnPhase1End = 65;
    public int spawnPhase2Start = 65;
    public int spawnPhase2End = 80;
    public int spawnPhase3Start = 80;

    public int enemyPhase1Start = 15;
    public int enemyPhase1End = 30;
    public int enemyPhase2Start = 30;
    public int enemyPhase2End = 65;
    public int enemyPhase3Start = 65;
    public int enemyPhase3End = 120;
    public int enemyPhase4Start = 120;

    private GameManagerScript gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive = 0;
        enemy01Offset = new Vector3 (0.0f, .11f, 0.0f);
        enemy02Offset = new Vector3 (0.0f, .25f, 0.0f);
        currentSpawnTime = spawnIntervalDuration;
        isGameOver = false;
        spawnNum = 1;
        group1 = 2;
        group2 = 5;
        group3 = 8;
        group1Count = 0;
        group2Count = 0;
        group3Count = 0;
        isSpawning = false;

        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        GameObject spawnLocationParent = GameObject.Find("EnemySpawnLocations");
        GetSpawnLocations(spawnLocationParent);
        InitialSpawn();
    }

    private void GetSpawnLocations(GameObject spawnLocationParent)
    {
        int i = 0;
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
        int rand = 0;

        for (int i = 0; i < initSpawnNum; i++)
        {
            rand = UnityEngine.Random.Range(3, spawnLocations.Length);
            while (unique.Contains(rand))
            {
                rand = UnityEngine.Random.Range(3, spawnLocations.Length);
            }
            unique.Add(rand);
            newEnemy = Instantiate(enemy00, spawnLocations[rand].position, Quaternion.identity);
            newEnemy.transform.parent = transform;
            if (rand > group1 && rand <= group2)
            {
                newEnemy.GetComponent<EnemyScript>().group = 2;
                group2Count++;
            } else if (rand > group2 && rand <= group3)
            {
                newEnemy.GetComponent<EnemyScript>().group = 3;
                group3Count++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            return;
        } else 
        {
            GetEnemiesAlive();
            if(!isSpawning)
            {
                SpawnEnemies();
            }
        }
    }

    private void GetEnemiesAlive()
    {
        enemiesAlive = transform.childCount;
    }

    private void SpawnEnemies()
    {
        if (currentSpawnTime <= 0.0f)
        {
            isSpawning = true;
            if (gameManagerScript.score >= spawnPhase1Start && gameManagerScript.score < spawnPhase1End)
            {
                spawnNum = 2;
            } else if (gameManagerScript.score >= spawnPhase2Start && gameManagerScript.score < spawnPhase2End)
            {
                spawnNum = 1;
            } else if (gameManagerScript.score >= spawnPhase3Start)
            {
                spawnNum = 2;
            } else 
            {
                spawnNum = 1;
            }
            if (enemiesAlive <= maxEnemiesAlive - spawnNum)
            {
                for (int i = 0; i < spawnNum; i++)
                {
                    StartCoroutine(makeEnemy(spawnNum));
                }
            }
        } else
        {
            currentSpawnTime -= Time.deltaTime;
        }
    }

    private IEnumerator makeEnemy (int spawnNum)
    {
        GameObject newEnemy;
        GameObject entryEffect;

        //random location
        int randSpawnLoc = DistributeEnemies();
        if(randSpawnLoc != -1)
        {
            int randEnemy = RandomEnemy();
            entryEffect = Instantiate(enemyEntryTypes[randEnemy], spawnLocations[randSpawnLoc].position, Quaternion.identity);
            yield return new WaitForSeconds(entryTime);
            Destroy(entryEffect);
            if (randEnemy == 0)
            {
                newEnemy = Instantiate(enemyTypes[randEnemy], spawnLocations[randSpawnLoc].position, Quaternion.identity);
            } else if (randEnemy == 1)
            {
                newEnemy = Instantiate(enemyTypes[randEnemy], spawnLocations[randSpawnLoc].position + enemy01Offset, Quaternion.identity);
            } else {
                newEnemy = Instantiate(enemyTypes[randEnemy], spawnLocations[randSpawnLoc].position + enemy02Offset, Quaternion.identity);  
            }
            newEnemy.transform.parent = transform;

            if (randSpawnLoc <= group1)
            {
                newEnemy.GetComponent<EnemyScript>().group = 1;
            } else if (randSpawnLoc > group1 && randSpawnLoc <= group2)
            {
                newEnemy.GetComponent<EnemyScript>().group = 2;
            } else if (randSpawnLoc > group2 && randSpawnLoc <= group3)
            {
                newEnemy.GetComponent<EnemyScript>().group = 3;
            }
        }
        yield return new WaitForSeconds(0.1f);
        currentSpawnTime = spawnIntervalDuration;
        isSpawning=false;
    }

    private int DistributeEnemies()
    {
        int rand = UnityEngine.Random.Range(0,spawnLocations.Length);
        if (rand <= group1 && group1Count < (maxEnemiesAlive/3))
        {
            group1Count++;
            return rand;
        }
        if (rand > group1 && rand <= group2 && (group2Count < maxEnemiesAlive/3))
        {
            group2Count++;
            return rand;
        }
        if (rand > group2 && rand <= group3 && (group3Count < maxEnemiesAlive/3))
        {
            group3Count++;
            return rand;
        }
        return -1;
    }

    private int RandomEnemy()
    {
        int rand = 0;
        if (gameManagerScript.score >= enemyPhase1Start && gameManagerScript.score < enemyPhase1End)
        {
            rand = UnityEngine.Random.Range(0,3);//66-33 (type0-type1)
            if(rand <= 1)
            {
                return 0;
            } else {
                return 1;
            }
        } else if (gameManagerScript.score >= enemyPhase2Start && gameManagerScript.score < enemyPhase2End)
        {
            rand = UnityEngine.Random.Range(0,2);//50-50(type0-1)
            return rand;
        } else if (gameManagerScript.score >= enemyPhase3Start && gameManagerScript.score < enemyPhase3End)
        {
            rand = UnityEngine.Random.Range(0,3);//33 of each
            return rand;
        } else if (gameManagerScript.score >= enemyPhase4Start)
        {
            rand = UnityEngine.Random.Range(0,5);//20, 40, 40 (type0,1,2)
            if(rand < 1)
            {
                return 0;
            } else if (rand > 1 && rand <=3)
            {
                return 1;
            } else 
            {
                return 2;
            }
        }
        return rand;
    }



    public void DestroyAllEnemies(float damageDealt)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<Enemy00Script>().GetDamaged(damageDealt);
        }
        group1Count = 0;
        group2Count = 0;
        group3Count = 0;
    }
}

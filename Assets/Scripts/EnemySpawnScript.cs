﻿using System;
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
    public float entryTime = 2.0f;



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
        if (currentSpawnTime <= 0.0f)
        {
            StartCoroutine(makeEnemy());
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
            int randSpawnLoc = UnityEngine.Random.Range(0,spawnLocations.Length);
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
        }
    }

    private void DistributeEnemies()
    {
        //while true
        //randomize
        //if rand <=2 and count1 + 1 <=4
        //count1 ++
        //return rand
        //if rand <=5 and count2 + 1 <=4
        //count2 ++
        //return rand
        //if rand <=8 and count3 + 1 <=4
        //count3 ++
        //return rand
    }
    //add newEnemy.group = rand
    //if group <=2
    //count1--
    //if group <=5
    //count2--
    //if group <=8
    //count3--

    private void GetEnemiesAlive()
    {
        enemiesAlive = transform.childCount;
        // Debug.Log("Enemies alive: " + enemiesAlive);
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
        }
    }

    public void DestroyAllEnemies(float damageDealt)
    {
        foreach (Transform child in transform)
        {
            Debug.Log("Destroy");
            child.gameObject.GetComponent<Enemy00Script>().GetDamaged(damageDealt);
            // Destroy(child.gameObject);
        }
    }
}

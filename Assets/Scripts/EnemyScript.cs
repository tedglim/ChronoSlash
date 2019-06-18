using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public abstract class EnemyScript : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    private int rand;

    protected Vector2 patrolDirection;
    private bool isFacingRight;
    public float patrolDurationMin;
    public float patrolDurationMax;
    protected float patrolDuration;

    public float patrolSpeedMin;
    public float patrolSpeedMax;
    protected float patrolSpeed;

    public float totalHealth;
    private float currentHealth;
    public float damageDealt;
    public GameObject item;

    public Image hp;
    public Image bar;

    public int group;
    private EnemySpawnScript enemySpawnScript;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        enemySpawnScript = enemySpawner.GetComponent<EnemySpawnScript>();
        InitDirection();
        patrolSpeed = UnityEngine.Random.Range(patrolSpeedMin, patrolSpeedMax);
        patrolDuration = UnityEngine.Random.Range(patrolDurationMin, patrolDurationMax);
        currentHealth = totalHealth;
        hp.enabled = false;
        bar.enabled = false;
    }

    protected virtual void InitDirection()
    {
        rand = UnityEngine.Random.Range(0,2);
        if(rand == 0)
        {
            isFacingRight = false;
            patrolDirection = Vector2.left;
        } else {
            isFacingRight = true;
            patrolDirection = Vector2.right;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    protected abstract void Patrol();

    protected virtual void Flip()
    {
        isFacingRight = !isFacingRight;
        if(isFacingRight)
        {
            patrolDirection = Vector2.right;
        } else {
            patrolDirection = Vector2.left;
        }
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void GetDamaged(float damageTaken)
    {
        currentHealth -= damageTaken;
        hp.fillAmount = currentHealth/totalHealth;
        hp.enabled = true;
        bar.enabled = true;
        if (currentHealth <= 0)
        {
            Instantiate(item, transform.position, Quaternion.identity);
            if (group == 1 && enemySpawnScript.group1Count > 0)
            {
                enemySpawnScript.group1Count--;
            } else if (group == 2 && enemySpawnScript.group2Count > 0)
            {
                enemySpawnScript.group2Count--;
            } else if (group == 3 && enemySpawnScript.group3Count > 0)
            {
                enemySpawnScript.group3Count--;
            }
            Destroy(gameObject);
            CameraShaker.Instance.ShakeOnce(1.5f, 1.5f, .1f, .25f);
        }
    }
}

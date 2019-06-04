using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        InitDirection();
        patrolSpeed = UnityEngine.Random.Range(patrolSpeedMin, patrolSpeedMax);
        patrolDuration = UnityEngine.Random.Range(patrolDurationMin, patrolDurationMax);
        currentHealth = totalHealth;
        hp.enabled = false;
        bar.enabled = false;

        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        enemySpawnScript = enemySpawner.GetComponent<EnemySpawnScript>();
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
            // Debug.Log("Enemy destroyed");
            Instantiate(item, transform.position, Quaternion.identity);
            if (group == 1)
            {
                enemySpawnScript.group1--;
            } else if (group == 2)
            {
                enemySpawnScript.group2--;
            } else if (group == 3)
            {
                enemySpawnScript.group3--;
            }
            Destroy(gameObject);
        }
    }
}

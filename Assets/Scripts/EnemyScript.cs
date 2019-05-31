using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        InitDirection();
        patrolSpeed = UnityEngine.Random.Range(patrolSpeedMin, patrolSpeedMax);
        patrolDuration = UnityEngine.Random.Range(patrolDurationMin, patrolDurationMax);
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
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy destroyed");
            Instantiate(item, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

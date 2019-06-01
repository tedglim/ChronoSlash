using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy00Script : EnemyScript
{
    private float currentPatrolTime;
    // Update is called once per frame
    void Awake()
    {
        // patrolSpeed = UnityEngine.Random.Range(patrolSpeedMin, patrolSpeedMax);
    }
    
    void Update()
    {
        Patrol();
    }

    protected override void Patrol()
    {
        if(currentPatrolTime <= 0)
        {
            Flip();
            currentPatrolTime = patrolDuration;
        } else {
            rb2d.MovePosition((Vector2)transform.position + patrolDirection * patrolSpeed * Time.deltaTime);
            currentPatrolTime -= Time.deltaTime;
        }
    }
    
}

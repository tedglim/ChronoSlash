using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float bulletSpeed = 300.0f;
    public float bulletDamage = 1.0f;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = transform.right * bulletSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Enemy00")
        {
            Destroy(gameObject);
            Enemy00Script enemy = hitInfo.GetComponent<Enemy00Script>();
            enemy.GetDamaged(bulletDamage);
        }
        if (hitInfo.gameObject.tag == "StageWall")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Enemy00")
        {
            Destroy(gameObject);
            Enemy00Script enemy = hitInfo.GetComponent<Enemy00Script>();
            enemy.GetDamaged(bulletDamage);
        }
        if (hitInfo.gameObject.tag == "StageWall")
        {
            Destroy(gameObject);
        }
    }
}

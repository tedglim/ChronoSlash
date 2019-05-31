using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 direction;
    private bool isFacingRight;
    private bool canRun;
    public float runSpeed = 1.0f;

    private bool canDashJumpUp;
    private bool canDashJumpDown;
    private int stageLevelMin;
    private int stageLevelMax;
    private int currStageLevel;
    public float dashDistance = 2.64f;

    public float startingHealth = 10.0f;
    private float currentHealth;

    private GameManagerScript gameManagerScript;
    public bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        isGameOver = false;
        
        //auto run right
        direction = Vector2.right;
        isFacingRight = true;
        canRun = true;

        canDashJumpUp = false;
        canDashJumpDown = false;
        stageLevelMin = 0;
        stageLevelMax = 2;
        currStageLevel = stageLevelMin;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRun();
        CheckDashJump();
        // HelperStopAndFlip();
    }

    // private void HelperStopAndFlip()
    // {
    //     if(Input.GetKeyDown(KeyCode.Space))
    //     {
    //         rb2d.velocity = Vector2.zero;
    //         canRun=false;
    //     }
    // }

    private void CheckRun()
    {
        if(isFacingRight)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Flip();
            }
        } else if (!isFacingRight)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        if(isFacingRight)
        {
            direction = Vector2.right;
        } else {
            direction = Vector2.left;
        }
        transform.Rotate(0.0f, 180.0f, 0.0f);
        canRun = true;
    }

    private void CheckDashJump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && currStageLevel < stageLevelMax)
        {
            canDashJumpUp = true;
            currStageLevel++;
        } else if (Input.GetKeyDown(KeyCode.DownArrow) && currStageLevel > stageLevelMin) 
        {
            canDashJumpDown = true;
            currStageLevel--;
        }
    }

    void FixedUpdate()
    {
        if(!isGameOver)
        {
            if (canDashJumpUp)
            {
                DashJumpUp();
            } else if (canDashJumpDown)
            {
                DashJumpDown();
            }
            if (canRun)
            {
                Run();
            }
        }
    }

    private void Run()
    {
        rb2d.MovePosition((Vector2)transform.position + direction * runSpeed * Time.deltaTime);
    }

    private void DashJumpUp()
    {
        rb2d.transform.position = new Vector2(rb2d.transform.position.x, rb2d.transform.position.y + dashDistance);
        canDashJumpUp = false;
    }

    private void DashJumpDown()
    {
        rb2d.transform.position = new Vector2(rb2d.transform.position.x, rb2d.transform.position.y - dashDistance);
        canDashJumpDown = false;
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "StageWall")
        {
            Debug.Log("Hit Wall");
            rb2d.velocity = Vector2.zero;
            canRun=false;
        }
        if (hit.gameObject.tag == "Enemy00")
        {
            Debug.Log("Hit Enemy");
            Enemy00Script enemy = hit.GetComponent<Enemy00Script>();
            TakeDamage(enemy.damageDealt);
            enemy.GetDamaged();
        }
        if (hit.gameObject.tag == "Item")
        {
            Debug.Log("Hit Item");
            ItemScript item = hit.GetComponent<ItemScript>();

            gameManagerScript.AddScore(item.value);
            item.GetDestroyed();
        }
    }

    private void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        gameManagerScript.ManagePlayerHealth(currentHealth, startingHealth);
    }
}

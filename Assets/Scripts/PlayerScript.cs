using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 direction;
    private bool isFacingRight;
    private bool canRun;
    public float runSpeed = 3.5f;

    private bool canDashJumpUp;
    private bool canDashJumpDown;
    private int stageLevelMin;
    private int stageLevelMax;
    private int currStageLevel;
    public float dashDistance = 2.0f;

    public float shootDuration = .15f;
    private float currentShootTime;
    public Transform firePos;
    public GameObject bullet;

    public float startingHealth = 10.0f;
    private float currentHealth;
    public float playerDamage = 1.0f;

    private GameManagerScript gameManagerScript;
    private EnemySpawnScript enemySpawnScript;
    public bool isGameOver;
    
    public float scoreForAbility = 30.0f;
    private float prevScoreForAbility;
    private float currAbilityAmount;
    public float useAbilityDuration = 5.0f;
    private float canUseAbilityTime;
    private bool canUseAbility;

    public float abilityDamage = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
        GameObject gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        enemySpawnScript = enemySpawner.GetComponent<EnemySpawnScript>();
        isGameOver = false;
        
        //auto run right
        direction = Vector2.right;
        isFacingRight = true;
        canRun = true;

        currentShootTime = 0.0f;

        canDashJumpUp = false;
        canDashJumpDown = false;
        stageLevelMin = 0;
        stageLevelMax = 2;
        currStageLevel = stageLevelMin;

        prevScoreForAbility = 0.0f;
        currAbilityAmount = gameManagerScript.score - prevScoreForAbility;
        canUseAbilityTime = useAbilityDuration;
        canUseAbility = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            return;
        }
        CheckRun();
        CheckDashJump();
        CheckBlast();
    }

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
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && currStageLevel < stageLevelMax)
        {
            canDashJumpUp = true;
            currStageLevel++;
        } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currStageLevel > stageLevelMin) 
        {
            canDashJumpDown = true;
            currStageLevel--;
        }
    }

    private void CheckBlast()
    {
        if (currAbilityAmount >= scoreForAbility)
        {
            if (canUseAbilityTime <= 0)
            {
                canUseAbility = false;
                ResetBlastReqs();
            } else {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    canUseAbility = true;
                } else
                {
                    canUseAbilityTime -= Time.deltaTime;
                }
            }
        } else {
            canUseAbility = false;
            currAbilityAmount = gameManagerScript.score - prevScoreForAbility;
            gameManagerScript.ManageAbilityBar(canUseAbility, currAbilityAmount, scoreForAbility);
        }
    }

    private void ResetBlastReqs()
    {
        prevScoreForAbility += scoreForAbility;
        currAbilityAmount = gameManagerScript.score - prevScoreForAbility;
        canUseAbilityTime = useAbilityDuration;
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
            } else if (canUseAbility)
            {
                Blast();
            }
            if (canRun)
            {
                Run();
                Shoot();
            }
        } else {
            return;
        }
    }

    private void Run()
    {
        rb2d.MovePosition((Vector2)transform.position + direction * runSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (currentShootTime <= 0)
        {
            Instantiate(bullet, firePos.position, firePos.rotation);
            currentShootTime = shootDuration;
        } else 
        {
            currentShootTime -= Time.deltaTime;
        }
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

    private void Blast()
    {
        enemySpawnScript.DestroyAllEnemies(abilityDamage);
        ResetBlastReqs();
        canUseAbility = false;
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "StageWall")
        {
            rb2d.velocity = Vector2.zero;
            canRun=false;
        }
        if (hit.gameObject.tag == "Enemy00")
        {
            Enemy00Script enemy = hit.GetComponent<Enemy00Script>();
            TakeDamage(enemy.damageDealt);
            enemy.GetDamaged(playerDamage);
        }
        if (hit.gameObject.tag == "Item")
        {
            ItemScript item = hit.GetComponent<ItemScript>();
            gameManagerScript.AddScore(item.value);
            item.GetDestroyed();
        }
    }

    private void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        gameManagerScript.ManagePlayerHealth(currentHealth, startingHealth);
        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, .4f);
    }
}

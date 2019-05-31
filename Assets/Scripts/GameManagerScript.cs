using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private Image hpBar;
    private GameObject gameOverPanel;
    private int score;
    public Text scoreText;
    public Text bestText;
    public Text yourText;
    public bool isGameOver;
    private EnemySpawnScript enemySpawnScript;
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        GameObject health = GameObject.Find("GameManager/Canvas/Healthbar/Health");
        hpBar = health.GetComponent<Image>();
        gameOverPanel = GameObject.Find("GameManager/Canvas/GameOverPanel");
        GameObject spawner = GameObject.Find("EnemySpawner");
        enemySpawnScript = spawner.GetComponent<EnemySpawnScript>();
        GameObject player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();

        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int value)
    {
        if(!isGameOver)
        {
            score += value;
            scoreText.text = score.ToString();
        }
    }

    public void ManagePlayerHealth(float currHealth, float totalHealth)
    {
        Debug.Log("current Health: " + currHealth);
        if (currHealth <= 0)
        {
            Debug.Log("DEAD");
            GameOver();
        }
        hpBar.fillAmount = currHealth / totalHealth;
    }

    void GameOver()
    {
        // scoreText.gameObject.SetActive(false);
        if(score > PlayerPrefs.GetInt("Best", 0))
        {
            PlayerPrefs.SetInt("Best", score);
        }
        bestText.text = "Best Score: " + PlayerPrefs.GetInt("Best", 0).ToString();
        yourText.text = "Your Score: " + score.ToString();
        enemySpawnScript.isGameOver = true;
        playerScript.isGameOver = true;
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}

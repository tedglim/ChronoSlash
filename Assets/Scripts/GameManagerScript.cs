using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    private GameObject gameOverPanel;
    public int score;
    public Text scoreText;
    public Text bestText;
    public Text yourText;
    public bool isGameOver;

    private Image hpBar;
    private Image abilityBar;

    private EnemySpawnScript enemySpawnScript;
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        GameObject health = GameObject.Find("GameManager/Canvas/Healthbar/Health");
        hpBar = health.GetComponent<Image>();
        GameObject ability = GameObject.Find("GameManager/Canvas/AbilityBar/Charge");
        abilityBar = ability.GetComponent<Image>();
        gameOverPanel = GameObject.Find("GameManager/Canvas/GameOverPanel");
        GameObject spawner = GameObject.Find("EnemySpawner");
        enemySpawnScript = spawner.GetComponent<EnemySpawnScript>();
        GameObject player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        score = 0;
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
        if (currHealth <= 0)
        {
            GameOver();
        }
        hpBar.fillAmount = currHealth / totalHealth;
    }

    public void ManageAbilityBar(bool canUseAbility, float currAbilityAmount, float scoreForAbility)
    {
        if(canUseAbility)
        {
            abilityBar.fillAmount = 1;
        } else {
            abilityBar.fillAmount = currAbilityAmount / scoreForAbility;
        }
    }

    void GameOver()
    {
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
        abilityBar.fillAmount = 0;
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

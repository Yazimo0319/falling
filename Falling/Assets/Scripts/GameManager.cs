using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    
    public Text scoreText;
    public Text finalScoreText;
    public GameObject gameOverPanel;
    public float scoreMultiplier = 1f;
    
    private float score;
    private bool isGameOver;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        score = 0;
        isGameOver = false;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    
    void Update()
    {
        if (!isGameOver)
        {
            // 隨時間增加分數
            score += Time.deltaTime * scoreMultiplier;
            scoreText.text = "分數: " + Mathf.FloorToInt(score);
        }
    }
    
    public void GameOver(bool hitObstacle)
    {
        isGameOver = true;
        
        // 如果撞到障礙物，減少分數
        if (hitObstacle)
        {
            score *= 0.5f;
        }
        
        // 顯示最終分數
        finalScoreText.text = "最終分數: " + Mathf.FloorToInt(score);
        
        // 顯示遊戲結束面板
        gameOverPanel.SetActive(true);
        
        // 暫停遊戲
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        // 重新加載當前場景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

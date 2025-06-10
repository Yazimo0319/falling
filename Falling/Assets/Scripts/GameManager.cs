using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI 元件")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;

    [Header("分數")]
    public int score = 0;
    private float scoreTimer = 0f;          // 每秒加分計時器
    public float scoreInterval = 1f;        // 每幾秒加 1 分（預設 1 秒）

    [Header("遊戲狀態")]
    public bool isGameOver = false;

    [Header("玩家")]
    public GameObject player; // 👉 在 Inspector 拖入 Player 物件
    public TextMeshProUGUI finalScoreText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isGameOver) return;

        scoreTimer += Time.deltaTime;

        if (scoreTimer >= scoreInterval)
        {
            score++;
            scoreTimer = 0f;

            if (scoreText != null)
                scoreText.text = $"Score:{score}";
        }
    }

    public void GameOver(bool isDead)
    {
        isGameOver = true;

        // 顯示結束畫面
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (resultText != null)
            resultText.text = isDead ? "Game Over!" : "Good Jump!";
        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {score}";

        if (scoreText != null)
            scoreText.text = $"Score:{score}";

        // 停止玩家行動
        if (player != null)
        {
            var pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static; // 鎖定玩家不再掉落
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static string CurrentMode { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject scoreReportPanel;

    [Header("時間設定")]
    public float gameDuration = 60f;
    public float gravityScale = 1f;

    [Header("玩家")]
    public GameObject player;

    [Header("遊戲狀態")]
    public bool isGameOver = false;
    public float score = 0f;
    public float timeRemaining { get; private set; }

    [Header("音效")]
    public AudioClip hitSFX;
    public AudioSource sfxSource;

    private float timer = 0f;
    private bool hasStartedFall = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        CurrentMode = PlayerPrefs.GetString("CurrentMode", "限時");
        timeRemaining = gameDuration;
        timer = 0f;

        if (CurrentMode == "無盡" && timerText != null)
            timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        if (CurrentMode == "無盡")
        {
            timer += Time.deltaTime;
            score = timer;

            if (scoreText != null)
                scoreText.text = $"Score: {score:F2}";
        }
        else // 限時模式
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0f) timeRemaining = 0f;

            score = 100f * ((gameDuration - timeRemaining) / gameDuration);
            score = Mathf.Clamp(score, 0f, 100f);

            if (scoreText != null)
                scoreText.text = $"Score: {score:F2}";

            if (timerText != null)
                timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";

            if (timeRemaining <= 0f && !hasStartedFall)
            {
                hasStartedFall = true;
                FindObjectOfType<InfiniteBackgroundManager>()?.StartBottomSlide();
            }
        }
    }

    public void GameOver(bool isDead)
    {
        if (isGameOver) return;
        isGameOver = true;

        // ✅ 撞到障礙物時先停止背景音樂
        var bgm = FindObjectOfType<BGMManager>();
        if (bgm != null && bgm.audioSource != null)
        {
            bgm.audioSource.Stop();
        }

        // ✅ 播音效並延遲顯示成績
        if (isDead && hitSFX != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(hitSFX);
            StartCoroutine(ShowReportAfterSound(hitSFX.length));
        }
        else
        {
            ShowScoreReport();
        }
    }

    IEnumerator ShowReportAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowScoreReport();
    }

    private void ShowScoreReport()
    {
        if (scoreReportPanel != null)
        {
            var report = scoreReportPanel.GetComponent<ScoreReportUI>();
            if (report != null)
                report.Show(score);
            else
                Debug.LogError("❌ ScoreReportPanel 上沒有 ScoreReportUI 組件！");
        }
        else
        {
            Debug.LogWarning("⚠️ ScoreReportPanel 是 null！");
        }

        // ✅ 成績畫面出現時才播放結算 BGM
        FindObjectOfType<BGMManager>()?.PlayResultMusic(score);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnablePlayerFall()
    {
        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = gravityScale;
                rb.velocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }

            var pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;
        }
    }
}

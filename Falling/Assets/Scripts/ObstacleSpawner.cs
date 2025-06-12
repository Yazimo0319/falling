using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("障礙物設定")]
    public GameObject[] obstaclePrefabs;

    [Header("生成區間設定")]
    public float startSpawnRate = 1.5f;   // 初始間隔（秒）
    public float minSpawnRate = 0.5f;     // 最快間隔（越小越快）

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + startSpawnRate;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // ⛔ 限時模式：時間結束就不再生成
        if (GameManager.CurrentMode == "限時" && GameManager.Instance.timeRemaining <= 0f)
            return;

        // ⛔ 所有模式都共用：遊戲結束不再生成
        if (GameManager.Instance.isGameOver)
            return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();

            // 依據遊戲進度加快生成速度（限時模式有效）
            float t = 0f;
            if (GameManager.CurrentMode == "限時")
            {
                t = 1 - (GameManager.Instance.timeRemaining / GameManager.Instance.gameDuration);
            }

            float currentRate = Mathf.Lerp(startSpawnRate, minSpawnRate, t);
            nextSpawnTime = Time.time + currentRate;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;

        // 攝影機邊界內隨機生成
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float randomX = Random.Range(-camWidth, camWidth);
        float spawnY = -Camera.main.orthographicSize - 2f;

        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);
        int index = Random.Range(0, obstaclePrefabs.Length);
        Instantiate(obstaclePrefabs[index], spawnPos, Quaternion.identity);
    }
}

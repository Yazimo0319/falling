using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("障礙物設定")]
    public GameObject[] obstaclePrefabs;

    [Header("生成區間設定")]
    public float startSpawnRate = 1.5f;
    public float minSpawnRate = 0.5f;

    [Header("啟動延遲時間（秒）")]
    public float spawnDelay = 1.5f;

    private float nextSpawnTime;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        nextSpawnTime = Time.time + spawnDelay + startSpawnRate;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // 限時結束不生成
        if (GameManager.CurrentMode == "限時" && GameManager.Instance.timeRemaining <= 0f)
            return;

        // 遊戲結束不生成
        if (GameManager.Instance.isGameOver)
            return;

        // ⏱ 延遲後才啟動
        if (Time.time < startTime + spawnDelay)
            return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();

            float t = 0f;
            if (GameManager.CurrentMode == "限時")
                t = 1 - (GameManager.Instance.timeRemaining / GameManager.Instance.gameDuration);

            float currentRate = Mathf.Lerp(startSpawnRate, minSpawnRate, t);
            nextSpawnTime = Time.time + currentRate;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;

        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float randomX = Random.Range(-camWidth, camWidth);
        float spawnY = -Camera.main.orthographicSize - 2f;

        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);
        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject newObstacle = Instantiate(obstaclePrefabs[index], spawnPos, Quaternion.identity);

        // ⏹ 強制套用 prefab 的 scale
        newObstacle.transform.localScale = obstaclePrefabs[index].transform.localScale;

        // 🔁 重啟碰撞器（避免 scale 錯誤）
        var col = newObstacle.GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.enabled = false;
            col.enabled = true;
        }
    }
}

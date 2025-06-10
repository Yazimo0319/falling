using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("障礙物設定")]
    public GameObject[] obstaclePrefabs;

    [Header("左右牆物件")]
    public Transform leftWall;
    public Transform rightWall;

    [Header("生成 Y 範圍")]
    public float minY = 1000f;
    public float maxY = 1300f;

    [Header("生成頻率（秒）")]
    public float spawnRate = 2f;

    [Header("障礙物存活秒數（0 = 不刪除）")]
    public float obstacleLifetime = 5f;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        // ✅ 如果遊戲結束（不論是死亡或掉到地板）就不再生成
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0 || leftWall == null || rightWall == null) return;

        float leftEdge = leftWall.position.x + leftWall.localScale.x / 2f;
        float rightEdge = rightWall.position.x - rightWall.localScale.x / 2f;

        float randomX = Random.Range(leftEdge, rightEdge);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);

        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject newObstacle = Instantiate(obstaclePrefabs[index], spawnPos, Quaternion.identity);

        if (obstacleLifetime > 0f)
        {
            Destroy(newObstacle, obstacleLifetime);
        }

        Debug.Log("Spawned at: " + spawnPos);
    }
}
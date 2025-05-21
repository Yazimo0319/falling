using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    
    [Header("生成範圍")]
    public float minX = -4f;
    public float maxX = 4f;
    public float minY = -6f;
    public float maxY = 6f;

    [Header("生成頻率（秒）")]
    public float spawnRate = 2f;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnObstacle()
    {
        // 隨機 X, Y 座標
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // 隨機選擇障礙物預製體
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedObstacle = obstaclePrefabs[randomIndex];

        // 生成障礙物
        Instantiate(selectedObstacle, spawnPosition, Quaternion.identity);
    }
}

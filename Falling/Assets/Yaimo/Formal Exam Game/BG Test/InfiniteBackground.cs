using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteBackgroundManager : MonoBehaviour
{
    [Header("遠景背景設定 (視差緩慢)")]
    public Transform farTop;
    public Transform[] farLoop;
    public float farTileHeight = 20f;
    public float farScrollStartSpeed = 3f;
    public float farScrollMaxSpeed = 10f;

    [Header("建築背景設定 (主建構體)")]
    public Transform buildingTop;
    public Transform[] buildingLoop;
    public Transform buildingBottom;
    public float buildingTileHeight = 20f;
    public float buildingScrollStartSpeed = 10f;
    public float buildingScrollMaxSpeed = 30f;
    public float tileOverlapFix = 0.01f;
    public float bottomSlideDuration = 1f;

    [Header("落地判定")]
    public GameObject groundTriggerPrefab;

    [Header("觸發設定")]
    public float bottomSlideTriggerTime = 3f;

    private bool hasStartedFall = false;
    private bool hasSpawnedBottom = false;
    private bool isBottomSliding = false;
    private Transform lastBuildingTile;
    private Transform lastFarTile;
    private float elapsedTime = 0f;

    void Start()
    {
        lastBuildingTile = buildingLoop[buildingLoop.Length - 1];
        lastFarTile = farLoop[farLoop.Length - 1];
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        elapsedTime += Time.deltaTime;
        float timeLeft = GameManager.Instance.timeRemaining;

        // 根據時間內插滾動速度
        float buildingScrollSpeed = Mathf.Lerp(buildingScrollStartSpeed, buildingScrollMaxSpeed, elapsedTime / GameManager.Instance.gameDuration);

        float farScrollSpeed = Mathf.Lerp(farScrollStartSpeed, farScrollMaxSpeed, elapsedTime / GameManager.Instance.gameDuration);


        float deltaBuilding = buildingScrollSpeed * Time.deltaTime;
        float deltaFar = farScrollSpeed * Time.deltaTime;

        // ✅ 玩家還未掉落或 Bottom 正在滑 → 背景繼續滾動
        if (!hasStartedFall || isBottomSliding)
        {
            buildingTop.Translate(Vector3.up * deltaBuilding);
            farTop.Translate(Vector3.up * deltaFar);

            ScrollTiles(buildingLoop, deltaBuilding, buildingTileHeight, ref lastBuildingTile);
            ScrollTiles(farLoop, deltaFar, farTileHeight, ref lastFarTile);
        }

        // ✅ 倒數剩下 bottomSlideTriggerTime 秒 → 觸發滑入
        if (timeLeft <= bottomSlideTriggerTime && !hasStartedFall)
        {
            hasStartedFall = true;
            StartCoroutine(SlideInBottom());
        }
    }

    void ScrollTiles(Transform[] tiles, float moveY, float height, ref Transform lastTile)
    {
        foreach (Transform tile in tiles)
        {
            tile.Translate(Vector3.up * moveY);
        }

        float topLimit = Camera.main.transform.position.y + Camera.main.orthographicSize;
        foreach (Transform tile in tiles)
        {
            if (tile.position.y - height / 2f >= topLimit)
            {
                float lowestY = GetLowestTileY(tiles);
                tile.position = new Vector3(
                    tile.position.x,
                    lowestY - height + tileOverlapFix,
                    tile.position.z
                );
                lastTile = tile;
            }
        }
    }

    float GetLowestTileY(Transform[] tiles)
    {
        float minY = tiles[0].position.y;
        foreach (Transform t in tiles)
        {
            if (t.position.y < minY)
                minY = t.position.y;
        }
        return minY;
    }

    public void StartBottomSlide()
    {
        if (!hasSpawnedBottom)
        {
            StartCoroutine(SlideInBottom());
        }
    }

    IEnumerator SlideInBottom()
    {
        hasSpawnedBottom = true;
        isBottomSliding = true;

        Vector3 bottomStart = new Vector3(
            buildingBottom.position.x,
            Camera.main.transform.position.y - buildingTileHeight * 2f,
            0f
        );

        float camBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
        Vector3 bottomTarget = new Vector3(
            buildingBottom.position.x,
            camBottom + buildingTileHeight / 2f,
            0f
        );

        buildingBottom.position = bottomStart;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / bottomSlideDuration;
            buildingBottom.position = Vector3.Lerp(bottomStart, bottomTarget, t);
            yield return null;
        }

        isBottomSliding = false;

        if (groundTriggerPrefab != null)
        {
            float groundY = -9.56f;
            Instantiate(groundTriggerPrefab, new Vector3(0f, groundY, 0f), Quaternion.identity);
        }

        GameManager.Instance.EnablePlayerFall();
    }
}

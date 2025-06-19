using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Header("移動速度設定")]
    public float baseSpeed = 2f;
    public float maxExtraSpeed = 4f;

    [Header("自轉設定")]
    public float rotationSpeed = 180f;
    private float rotationDir = 1f;

    [Header("大小設定")]
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    [Header("個體速度差異強度 (0=無差異, 1=完全補償)")]
    [Range(0f, 1f)] public float sizeSpeedEffect = 0.5f;

    private float screenTopY;
    private float sizeFactor = 1f;

    void Start()
    {
        screenTopY = Camera.main.orthographicSize + 2f;

        // ✅ 根據模式調整速度
        if (GameManager.CurrentMode == "無盡")
        {
            baseSpeed = 5f;
            maxExtraSpeed = 23f;
        }
        else
        {
            baseSpeed = 5f;
            maxExtraSpeed = 20f;
        }

        rotationDir = Random.Range(0, 2) == 0 ? 1f : -1f;

        float randomScale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);

        float rawFactor = 1f / randomScale;
        sizeFactor = Mathf.Lerp(1f, rawFactor, sizeSpeedEffect);
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver)
            return;

        float t = 0f;

        if (GameManager.CurrentMode == "無盡")
        {
            // 用 GameManager 中 timer 替代（使用 score 當秒數來源）
            t = Mathf.Clamp01(GameManager.Instance.score / 100f);
        }
        else
        {
            t = 1 - (GameManager.Instance.timeRemaining / GameManager.Instance.gameDuration);
        }

        float globalSpeed = baseSpeed + t * maxExtraSpeed;
        float speed = globalSpeed * sizeFactor;

        transform.position += Vector3.up * speed * Time.deltaTime;
        transform.Rotate(0f, 0f, rotationSpeed * rotationDir * Time.deltaTime);

        if (transform.position.y > screenTopY)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver(true);
            Destroy(gameObject);
        }
    }
}

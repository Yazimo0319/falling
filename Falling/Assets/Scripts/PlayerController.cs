using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera mainCamera;
    private float fixedY; // 固定 Y 軸位置

    void Start()
    {
        mainCamera = Camera.main;
        fixedY = transform.position.y;
    }

    void Update()
{
    if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        return;

    Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    float targetX = mouseWorldPos.x;
    float currentX = transform.position.x;

    float newX = Mathf.Lerp(currentX, targetX, moveSpeed * Time.deltaTime);

    // 🧩 1. 攝影機可視範圍
    float halfCamWidth = mainCamera.orthographicSize * mainCamera.aspect;
    float camCenterX = mainCamera.transform.position.x;

    // 🧩 2. 取得角色的半寬（Collider 或 Sprite）
    float playerHalfWidth = 0.5f; // 預設值
    var sr = GetComponent<SpriteRenderer>();
    if (sr != null)
        playerHalfWidth = sr.bounds.extents.x;

    // 🧩 3. 限制 newX，讓整個角色都在畫面內
    float leftLimit = camCenterX - halfCamWidth + playerHalfWidth;
    float rightLimit = camCenterX + halfCamWidth - playerHalfWidth;
    newX = Mathf.Clamp(newX, leftLimit, rightLimit);

    transform.position = new Vector3(newX, fixedY, 0f);
}



    void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Obstacle"))
    {
        GameManager.Instance.GameOver(true); // 撞到障礙物 → 失敗
    }
    else if (collision.gameObject.CompareTag("Ground"))
    {
        GameManager.Instance.GameOver(false); // 掉到地板 → 成功
    }
}

}

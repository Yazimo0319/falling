using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera mainCamera;
    private float fixedY;

    void Start()
    {
        mainCamera = Camera.main;
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

        float targetX = transform.position.x;

        // ✅ 滑鼠控制（無需按住）
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            targetX = mouseWorldPos.x;
        }
        // ✅ 手機觸控控制
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorldPos = mainCamera.ScreenToWorldPoint(touch.position);
            targetX = touchWorldPos.x;
        }

        float currentX = transform.position.x;
        float newX = Mathf.Lerp(currentX, targetX, moveSpeed * Time.deltaTime);

        // 🧩 攝影機邊界限制
        float halfCamWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float camCenterX = mainCamera.transform.position.x;

        float playerHalfWidth = 0.5f;
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            playerHalfWidth = sr.bounds.extents.x;

        float leftLimit = camCenterX - halfCamWidth + playerHalfWidth;
        float rightLimit = camCenterX + halfCamWidth - playerHalfWidth;
        newX = Mathf.Clamp(newX, leftLimit, rightLimit);

        transform.position = new Vector3(newX, fixedY, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log($"Player 碰到了：{collision.gameObject.name}");
        
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver(true);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.GameOver(false);
        }
    }
}

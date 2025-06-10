using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 把滑鼠螢幕座標轉成世界座標
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // 只取 X 座標，Y 維持不動
        float targetX = mouseWorldPos.x;
        float currentX = rb.position.x;

        // 插值移動使動作更平滑（可選）
        float newX = Mathf.Lerp(currentX, targetX, moveSpeed * Time.deltaTime);

        // 實際移動位置
        rb.MovePosition(new Vector2(newX, rb.position.y));
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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

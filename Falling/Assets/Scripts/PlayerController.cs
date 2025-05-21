using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = 0f;

        // 只用 A / D 鍵
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;

        float deltaX = horizontalInput * moveSpeed * Time.deltaTime;
        float nextX = rb.position.x + deltaX;

        // 沒有 Clamp，完全自由移動
        rb.MovePosition(new Vector2(nextX, rb.position.y));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.instance.GameOver(true);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            GameManager.instance.GameOver(false);
        }
    }
}

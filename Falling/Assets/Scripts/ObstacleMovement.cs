using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("成功接住！遊戲成功！");
            GameManager.instance.GameOver(false); // 這裡假設 false 是代表成功結束
            Destroy(gameObject); // 移除障礙物
        }
    }
}

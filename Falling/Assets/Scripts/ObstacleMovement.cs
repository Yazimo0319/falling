using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 2f;
    public float destroyY = 15f; // 障礙物會被銷毀的Y坐標

    void Update()
    {
        // 向上移動（相對於玩家的下降）
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        
        // 如果障礙物超出視野範圍，則銷毀它
        if (transform.position.y > destroyY)
        {
            Destroy(gameObject);
        }
    }

}

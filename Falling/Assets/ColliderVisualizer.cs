using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ColliderVisualizer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 5;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Vector2 offset = col.offset;
        Vector2 size = col.size;

        Vector2[] corners = new Vector2[5];
        Vector2 center = (Vector2)transform.position + offset;
        Vector2 halfSize = size / 2f;

        corners[0] = center + new Vector2(-halfSize.x, halfSize.y);  // top-left
        corners[1] = center + new Vector2(halfSize.x, halfSize.y);   // top-right
        corners[2] = center + new Vector2(halfSize.x, -halfSize.y);  // bottom-right
        corners[3] = center + new Vector2(-halfSize.x, -halfSize.y); // bottom-left
        corners[4] = corners[0]; // Close the loop

        for (int i = 0; i < 5; i++)
            lineRenderer.SetPosition(i, corners[i]);
    }
}

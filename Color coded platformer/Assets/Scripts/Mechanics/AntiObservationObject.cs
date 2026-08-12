using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class AntiObservationObject : MonoBehaviour
{
    public Transform player;
    public float fadeSpeed = 5f;
    
    private SpriteRenderer sr;
    private Collider2D col;
    private float targetAlpha = 1f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (player == null) return;

        Vector2 toObject = (transform.position - player.position).normalized;
        Vector2 playerFacing = player.right; 

        bool isBeingLookedAt = Vector2.Dot(playerFacing, toObject) > 0.5f;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        bool inCamera = viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;

        if (isBeingLookedAt && inCamera)
        {
            targetAlpha = 0.2f; 
            col.enabled = false; 
        }
        else
        {
            targetAlpha = 1f; 
            col.enabled = true;
        }

        Color c = sr.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        sr.color = c;
    }
}

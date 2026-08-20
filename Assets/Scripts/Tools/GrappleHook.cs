using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public LineRenderer line;
    public LayerMask grappleLayers;
    public float maxDistance = 10f;
    public float pullSpeed = 10f;

    private Rigidbody2D playerRb;
    private bool isGrappling;
    private Vector2 grapplePoint;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        if (line) line.enabled = false;
    }

    public void TryGrapple(Vector2 direction, int playerLayer)
    {
        if (isGrappling) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grappleLayers);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == playerLayer)
            {
                grapplePoint = hit.point;
                isGrappling = true;
                if (line) line.enabled = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isGrappling)
        {
            if (line)
            {
                line.SetPosition(0, transform.position);
                line.SetPosition(1, grapplePoint);
            }

            Vector2 dir = (grapplePoint - (Vector2)transform.position).normalized;
            playerRb.linearVelocity = dir * pullSpeed;

            if (Vector2.Distance(transform.position, grapplePoint) < 1f)
            {
                StopGrapple();
            }
        }
    }

    public void StopGrapple()
    {
        isGrappling = false;
        if (line) line.enabled = false;
    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Waddler : MonoBehaviour
{
    public bool lookingRight;
    [SerializeField] float speed;
    private Rigidbody2D rb;
    public Key currentKey { get; private set; }
    [SerializeField] bool grounded;
    private Collider2D waddlerCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        waddlerCollider = GetComponent<Collider2D>();
    }

    public void Launch(Transform cannonTransform, float launchForce, bool facingRight)
    {
        lookingRight = facingRight;

        float angleInRadians = (cannonTransform.eulerAngles.z + 90) * Mathf.Deg2Rad;
        Vector2 launchDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

        rb.linearVelocity = launchDirection * launchForce;

        Collider2D cannonCollider = cannonTransform.GetComponent<Collider2D>();
        if (cannonCollider != null)
        {
            Physics2D.IgnoreCollision(waddlerCollider, cannonCollider, true);
            StartCoroutine(ReenableCollision(cannonCollider, 0.5f));
        }

        Debug.Log($"Waddler launched at angle: {cannonTransform.eulerAngles.z} degrees with force: {launchForce}");
    }

    private IEnumerator ReenableCollision(Collider2D otherCollider, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (otherCollider != null && waddlerCollider != null)
        {
            Physics2D.IgnoreCollision(waddlerCollider, otherCollider, false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle directional platforms first
        if (collision.gameObject.TryGetComponent(out DirectionallySolidPlatform platform))
        {
            if (ShouldPassThrough(platform))
            {
                IgnoreAndPassThrough(collision);
                return; // Skip further processing for this collision
            }
        }

        // Handle ladder collisions
        if (collision.gameObject.CompareTag("Ladder"))
        {
            Collider2D ladderCollider = collision.gameObject.GetComponent<Collider2D>();
            if (ladderCollider != null)
            {
                Physics2D.IgnoreCollision(ladderCollider, waddlerCollider);
                Debug.Log("Ignoring collision with ladder.");
            }
            return;
        }

        // Set grounded state
        if (!collision.gameObject.CompareTag("Player"))
        {
            grounded = true;
        }

        // Handle key pickup
        if (collision.gameObject.CompareTag("Key") && currentKey == null)
        {
            Key key = collision.gameObject.GetComponent<Key>();
            if (key != null && !key.ReadPickedUp())
            {
                currentKey = key;
                currentKey.PickUp(transform);
                Debug.Log("Key detected by Waddler and picked up.");
            }
        }

        // Handle wall collisions and player contact
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.DrawLine(contact.point, contact.point + contact.normal, Color.red, 2f);

            Vector2 collisionNormal = contact.normal;
            float collisionAngle = Vector2.Angle(Vector2.up, collisionNormal);
            Vector2 leftCollision = new Vector2(1, 0);
            Vector2 rightCollision = new Vector2(-1, 0);

            if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Key"))
            {
                if (lookingRight && collisionNormal == rightCollision)
                {
                    lookingRight = false;
                    Debug.Log("Waddler faces right as it walks into wall");
                }
                else if (!lookingRight && collisionNormal == leftCollision)
                {
                    lookingRight = true;
                    Debug.Log("Waddler faces left as it walks into wall");
                }
            }

            if (collision.gameObject.CompareTag("Player") && collisionAngle != 180)
            {
                Debug.Log("Waddler triggered scene reload.");
                GameObject.Find("Game Manager").GetComponent<GameManager>().ReloadScene();
            }
        }
    }

    private bool ShouldPassThrough(DirectionallySolidPlatform platform)
    {
        Vector2 velocity = rb.linearVelocity;
        float threshold = 0.1f;

        switch (platform.SelectedDirection)
        {
            case DirectionallySolidPlatform.DirectionOptions.Up:
                return velocity.y > threshold;

            case DirectionallySolidPlatform.DirectionOptions.Down:
                return velocity.y < -threshold;

            case DirectionallySolidPlatform.DirectionOptions.Left:
                return velocity.x < -threshold;

            case DirectionallySolidPlatform.DirectionOptions.Right:
                return velocity.x > threshold;

            default:
                return false;
        }
    }

    private void IgnoreAndPassThrough(Collision2D collision)
    {
        // Store velocity before collision response kills it
        Vector2 savedVelocity = rb.linearVelocity;

        // If velocity is too low, use walking direction instead
        if (savedVelocity.magnitude < 0.1f)
        {
            savedVelocity = (lookingRight ? Vector2.right : Vector2.left) * speed;
        }

        // Ignore the collision
        Physics2D.IgnoreCollision(collision.collider, waddlerCollider, true);

        // Restore velocity that was lost during collision
        rb.linearVelocity = savedVelocity;

        // Nudge position slightly to escape any overlap
        rb.position += savedVelocity.normalized * 0.1f;

        // Re-enable collision after passing through
        StartCoroutine(ReenableCollision(collision.collider, 0.4f));

        Debug.Log("Waddler passing through directional platform");
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    void FixedUpdate()
    {
        if (currentKey != null)
        {
            currentKey.SetDirection(lookingRight);

            if (currentKey.gameObject.layer != LayerMask.NameToLayer("Default") &&
                currentKey.gameObject.layer != gameObject.layer &&
                gameObject.layer != LayerMask.NameToLayer("Default"))
            {
                currentKey.Drop();
            }
        }

        if (grounded)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (lookingRight)
            {
                spriteRenderer.flipX = true;
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            }
            else
            {
                spriteRenderer.flipX = false;
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            }
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetKey(Key key)
    {
        currentKey = key;
    }
}

/*
using UnityEngine;

public class BoxCastIgnoreTag : MonoBehaviour
{
    public string tagToIgnore = "Ladder";      // Tag to ignore
    public Vector2 direction = Vector2.right; // Direction to cast the box
    public float castDistance = 5f;           // Maximum distance for the BoxCast
    public LayerMask layerMask;               // Layers to include in the collision check

    void Update()
    {
        // Get the origin, size, and angle from the Transform
        Vector2 origin = transform.position;       // Origin is the GameObject's position
        Vector2 boxSize = transform.localScale;    // Box size is the GameObject's scale
        float boxAngle = transform.eulerAngles.z;  // Box angle is the GameObject's Z-axis rotation

        // Perform the BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, boxAngle, direction, castDistance, layerMask);

        // Visualize the BoxCast for debugging
        DebugDrawBoxCast(origin, boxSize, boxAngle, direction, castDistance, Color.red);

        // Check if something was hit
        if (hit.collider != null)
        {
            // Check if the hit object has the tag to ignore
            if (hit.collider.CompareTag(tagToIgnore))
            {
                Debug.Log("Ignored object with tag: " + hit.collider.tag);
            }
            else
            {
                Debug.Log("Hit object: " + hit.collider.name);
                // Handle the hit object here
            }
        }
    }

    // Helper method to visualize the BoxCast in the Scene view
    private void DebugDrawBoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, Color color)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 extents = size * 0.5f;

        // Define the four corners of the box
        Vector2[] corners = new Vector2[4];
        corners[0] = origin + rotation * new Vector2(-extents.x, -extents.y); // Bottom-left
        corners[1] = origin + rotation * new Vector2(-extents.x, extents.y);  // Top-left
        corners[2] = origin + rotation * new Vector2(extents.x, extents.y);   // Top-right
        corners[3] = origin + rotation * new Vector2(extents.x, -extents.y);  // Bottom-right

        // Draw the box
        Debug.DrawLine(corners[0], corners[1], color);
        Debug.DrawLine(corners[1], corners[2], color);
        Debug.DrawLine(corners[2], corners[3], color);
        Debug.DrawLine(corners[3], corners[0], color);

        // Draw the direction line
        Debug.DrawLine(origin, origin + direction.normalized * distance, color);
    }
}
*/


/*
        // Perform the BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.localScale, transform.eulerAngles.z, Vector2.right, boxRadius);
        if (hit.collider != null)
        {
            // Check if the hit object has the tag to ignore
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Ignored object with tag: " + hit.collider.tag);
                Physics2D.IgnoreCollision(hit.collider.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            else
            {
                Debug.Log("Hit object: " + hit.collider.name);
                // Handle the hit object here
            }
        }
*/
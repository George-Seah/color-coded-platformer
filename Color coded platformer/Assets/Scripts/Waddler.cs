using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Waddler : MonoBehaviour
{
    public bool lookingRight;
    [SerializeField] float speed;
    Rigidbody2D rb;
    public Key currentKey { get; set; }
    [SerializeField] bool grounded;
    Collider2D waddlerCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        waddlerCollider = GetComponent<Collider2D>();
    }
    
    void OnEnable()
    {
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            LadderWaddlerCollisionRegistry.RegisterWaddler(col);
        }
    }

    void OnDisable()
    {
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            LadderWaddlerCollisionRegistry.UnregisterWaddler(col);
        }
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

    IEnumerator ReenableCollision(Collider2D otherCollider, float delay)
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
                if (GameManager.Instance != null)
                    GameManager.Instance.ReloadScene();
            }
        }
    }

    bool ShouldPassThrough(DirectionallySolidPlatform platform)
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

    void IgnoreAndPassThrough(Collision2D collision)
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


using UnityEngine;

public class GroundCollisionCheck : MonoBehaviour
{
    [SerializeField] PlayerBehaviour playerBehaviour;
    Rigidbody2D playerRB;
    Collider2D groundCheckCollider;
    
    [SerializeField] float groundTolerance = 0.2f; // Tolerance for physics penetration

    void Awake()
    {
        playerBehaviour = GetComponentInParent<PlayerBehaviour>();
        playerRB = playerBehaviour.GetComponent<Rigidbody2D>();
        groundCheckCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidGround(collision))
        {
            // Only ground if falling or nearly stationary vertically
            if (playerRB.linearVelocity.y <= 0.1f)
            {
                playerBehaviour.EnteredGround();
                Debug.Log("Player landed on ground.");
            }
        }
        
        if (collision.gameObject.CompareTag("Waddler"))
        {
            Bounds groundCollisionBounds = collision.bounds;
            Bounds groundCheckBounds = groundCheckCollider.bounds;

            // Ensure the bottom of the player is above the top of the ground object (with tolerance)
            if (groundCheckBounds.min.y >= groundCollisionBounds.max.y - groundTolerance)
            {
                playerBehaviour.LandedOnWaddler();
                if (collision.gameObject.GetComponentInChildren<Key>() != null)
                {
                    collision.gameObject.GetComponentInChildren<Key>().Drop();
                }
                Destroy(collision.gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (IsValidGround(collision))
        {
            // Keep grounded while in contact, unless actively jumping upward
            if (playerRB.linearVelocity.y <= 0.1f && !playerBehaviour.jumpPressed)
            {
                playerBehaviour.StayingOnGround();
                Debug.Log("Player staying on ground.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidGround(collision))
        {
            playerBehaviour.ExitedGround();
            Debug.Log("Player exited ground.");
        }
    }

    bool IsValidGround(Collider2D collision)
    {
        return collision.gameObject.CompareTag("Ground") ||
               collision.gameObject.CompareTag("Push Block") ||
               collision.gameObject.CompareTag("OneWayPlatform") ||
               collision.gameObject.CompareTag("Cloud") ||
               collision.gameObject.CompareTag("Moving Platform") ||
               collision.gameObject.CompareTag("Cannon") ||
               collision.gameObject.CompareTag("Key");
    }

    void Update()
    {
        // Removed the expensive per-frame Physics2D.IgnoreLayerCollision updates.
        // We now rely on the globally configured Collision Matrix (PhysicsMatrixSetup.cs).
    }
}
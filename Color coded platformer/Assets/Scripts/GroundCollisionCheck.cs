using UnityEngine;

public class GroundCollisionCheck : MonoBehaviour
{
    [SerializeField] PlayerBehaviour playerBehaviour;
    Rigidbody2D playerRB;
    Collider2D groundCheckCollider;
    
    [SerializeField] float groundTolerance = 0.2f; // Tolerance for physics penetration

    void Start()
    {
        playerBehaviour = transform.parent.gameObject.GetComponent<PlayerBehaviour>();
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
               collision.gameObject.CompareTag("Moving Platform");
    }

    void Update()
    {
        if (playerBehaviour.gameObject.GetComponent<ColorPositionChange>() != null)
        {
            int playerLayer = playerBehaviour.gameObject.layer;
            int groundCheckLayer = LayerMask.NameToLayer("GroundCollisionCheck");
            int redLayer = LayerMask.NameToLayer("Red");
            int blueLayer = LayerMask.NameToLayer("Blue");
            int greenLayer = LayerMask.NameToLayer("Green");

            bool isRed = playerLayer == redLayer;
            bool isBlue = playerLayer == blueLayer;
            bool isGreen = playerLayer == greenLayer;

            // Set collision ignoring based on current player layer
            Physics2D.IgnoreLayerCollision(groundCheckLayer, redLayer, !isRed && (isBlue || isGreen));
            Physics2D.IgnoreLayerCollision(groundCheckLayer, blueLayer, !isBlue && (isRed || isGreen));
            Physics2D.IgnoreLayerCollision(groundCheckLayer, greenLayer, !isGreen && (isRed || isBlue));

            // If none of the colors, enable collision with all
            if (!isRed && !isBlue && !isGreen)
            {
                Physics2D.IgnoreLayerCollision(groundCheckLayer, redLayer, false);
                Physics2D.IgnoreLayerCollision(groundCheckLayer, blueLayer, false);
                Physics2D.IgnoreLayerCollision(groundCheckLayer, greenLayer, false);
            }
        }
    }
}
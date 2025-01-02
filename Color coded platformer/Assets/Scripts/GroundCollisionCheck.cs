using UnityEngine;

public class GroundCollisionCheck : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    private void Start()
    {
        playerBehaviour = transform.parent.gameObject.GetComponent<PlayerBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidGround(collision))
        {
            Bounds groundCollisionBounds = collision.bounds;
            Bounds groundCheckBounds = GetComponent<Collider2D>().bounds;

            // Ensure the bottom of the player is above the top of the ground object
            if (groundCheckBounds.min.y >= groundCollisionBounds.max.y)
            {
                playerBehaviour.EnteredGround();
                Debug.Log("Player landed on ground.");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsValidGround(collision))
        {
            Bounds groundCollisionBounds = collision.bounds;
            Bounds groundCheckBounds = GetComponent<Collider2D>().bounds;

            if (groundCheckBounds.min.y >= groundCollisionBounds.max.y)
            {
                playerBehaviour.EnteredGround();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidGround(collision))
        {
            playerBehaviour.ExitedGround();
            Debug.Log("Player exited ground.");
        }
    }

    private bool IsValidGround(Collider2D collision)
    {
        return collision.gameObject.CompareTag("Ground") ||
               collision.gameObject.CompareTag("Push Block") ||
               collision.gameObject.CompareTag("OneWayPlatform") ||
               collision.gameObject.CompareTag("Cloud") ||
               collision.gameObject.CompareTag("Moving Platform");
    }
}

/*
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.DrawLine(contact.point, contact.point + contact.normal, Color.red, 2f);
            Debug.Log($"Normal: {contact.normal}");

            Vector2 collisionNormal = contact.normal;
            float collisionAngle = Vector2.Angle(Vector2.up, collisionNormal);
            Vector2 leftCollision = new Vector2(1, 0);
            Vector2 rightCollision = new Vector2(-1, 0);
            Debug.Log($"Collision Angle: {collisionAngle}°");
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block")
            || collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Cloud"))
            {
                if(collisionAngle == 180){
                    playerBehaviour.EnteredGround();
                }
                
                //Debug.Log("Player entered ground");
            }
        }
*/
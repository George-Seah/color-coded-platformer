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
    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block") ||
        collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Cloud"))
    {
        // Get the bounds of the colliders
        Bounds groundCollisionBounds = collision.bounds;
        Bounds groundCheckBounds = GetComponent<Collider2D>().bounds;

        // Check if the bottom of the player (via GroundCollisionCheck) is above the top of the ground object
        if (groundCheckBounds.min.y >= groundCollisionBounds.max.y)
        {
            playerBehaviour.EnteredGround();
            Debug.Log("Player landed on ground.");
        }
    }
}

    private void OnTriggerStay2D(Collider2D collision)
    {
    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block") ||
        collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Cloud"))
    {
        // Get the bounds of the colliders
        Bounds groundCollisionBounds = collision.bounds;
        Bounds groundCheckBounds = GetComponent<Collider2D>().bounds;

        // Check if the bottom of the player (via GroundCollisionCheck) is above the top of the ground object
        if (groundCheckBounds.min.y >= groundCollisionBounds.max.y)
        {
            playerBehaviour.EnteredGround();
            Debug.Log("Player landed on ground.");
        }
    }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block")
         || collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Cloud"))
        {
            playerBehaviour.ExitedGround();
            //Debug.Log("Player exited ground");
        }
    }
    private bool IsCollisionFromBelow(Collider2D collision)
    {
        // Get the bounds of the collision and player
        Bounds collisionBounds = collision.bounds;
        Bounds playerBounds = GetComponent<Collider2D>().bounds;

        // Check if the collision is from below (bottom of the player touching the top of the ground object)
        return playerBounds.min.y > collisionBounds.center.y;
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
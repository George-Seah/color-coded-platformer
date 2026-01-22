using UnityEngine;

public class GroundCollisionCheck : MonoBehaviour
{
    [SerializeField] PlayerBehaviour playerBehaviour;
    Rigidbody2D playerRB;

    void Start()
    {
        playerBehaviour = transform.parent.gameObject.GetComponent<PlayerBehaviour>();
        playerRB = playerBehaviour.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidGround(collision))
        {
            Bounds groundCollisionBounds = collision.bounds;
            Bounds groundCheckBounds = GetComponent<Collider2D>().bounds;

            // Ensure the bottom of the player is above the top of the ground object
            if (groundCheckBounds.min.y >= groundCollisionBounds.max.y && playerRB.linearVelocity.y <= 0)
            {
                playerBehaviour.EnteredGround();
                Debug.Log("Player landed on ground.");
            }
        }
        if(collision.gameObject.CompareTag("Waddler")){
            Bounds groundCollisionBounds = collision.bounds;
            Bounds groundCheckBounds = GetComponent<Collider2D>().bounds;

            // Ensure the bottom of the player is above the top of the ground object
            if (groundCheckBounds.min.y >= groundCollisionBounds.max.y)
            {
                playerBehaviour.LandedOnWaddler();
                if(collision.gameObject.GetComponentInChildren<Key>() != null){
                    collision.gameObject.GetComponentInChildren<Key>().Drop();
                }
                Destroy(collision.gameObject);
            }
        }
    }

    /*
    void OnTriggerStay2D(Collider2D collision)
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
    */

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

    void Update(){
        if(playerBehaviour.gameObject.GetComponent<ColorPositionChange>() != null){
            if(playerBehaviour.gameObject.layer == LayerMask.NameToLayer("Red")){
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), true);
            }
            else if(playerBehaviour.gameObject.layer == LayerMask.NameToLayer("Green")){
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), true);
            }
            else if(playerBehaviour.gameObject.layer == LayerMask.NameToLayer("Blue")){
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), true);
            }
            else{
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), false);
            }
        }

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
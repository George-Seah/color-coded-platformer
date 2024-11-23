using UnityEngine;

public class GroundCollisionCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // LayerMask for ground detection
    //[SerializeField] private float rayLength = 0.1f; // Length of the ray
    [SerializeField] bool isGrounded;

    private PlayerBehaviour playerBehaviour;

    void Start()
    {
        playerBehaviour = transform.parent.GetComponent<PlayerBehaviour>();
    }

    void FixedUpdate()
    {
        // Perform the raycast from the GroundCollisionCheck's position
        isGrounded = Physics2D.OverlapBox(transform.position, transform.localScale, 0, groundLayer);

        // Debug the raycast in the Scene view
        DebugDrawBox(transform.position, transform.localScale, isGrounded ? Color.green : Color.red);

        // Notify the PlayerBehaviour script about the grounded state
        if (isGrounded)
        {
            playerBehaviour.enteredGround();
        }
        else
        {
            playerBehaviour.exitedGround();
        }
    }
    // Custom Debug function to draw the box (visualize the OverlapBox)
    void DebugDrawBox(Vector2 center, Vector2 size, Color color)
    {
        Vector2 topLeft = center + new Vector2(-size.x / 2, size.y / 2);
        Vector2 topRight = center + new Vector2(size.x / 2, size.y / 2);
        Vector2 bottomLeft = center + new Vector2(-size.x / 2, -size.y / 2);
        Vector2 bottomRight = center + new Vector2(size.x / 2, -size.y / 2);

        // Draw the edges of the box
        Debug.DrawLine(topLeft, topRight, color);     // Top edge
        Debug.DrawLine(topRight, bottomRight, color); // Right edge
        Debug.DrawLine(bottomRight, bottomLeft, color); // Bottom edge
        Debug.DrawLine(bottomLeft, topLeft, color);   // Left edge
    }
}

/*
using UnityEngine;
using System.Collections;

public class GroundCollisionCheck : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D bc;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        playerBehaviour = transform.parent.gameObject.GetComponent<PlayerBehaviour>();
    }
    void FixedUpdate(){

    }


    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            playerBehaviour.enteredGround();
            Debug.Log("You're grounded");
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            playerBehaviour.exitedGround();
            Debug.Log("You're not grounded");
            //if(!playerBehaviour.jumping && !playerBehaviour.isGrounded){}
        }
    }
    
    void OnTriggerStay2D(Collider2D collision) {
        
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
        //    isGrounded = true; // Ensure grounded state during the collision
        //    jumpCancelled = false;
        //    jumping = false;
        //}
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
*/
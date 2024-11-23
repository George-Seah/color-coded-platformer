using UnityEngine;

public class GroundCollisionCheck : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private PlayerBehaviour playerBehaviour;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        playerBehaviour = transform.parent.gameObject.GetComponent<PlayerBehaviour>();
    }
    void FixedUpdate(){

    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            playerBehaviour.enteredGround();
            Debug.Log("You're grounded");
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            //playerBehaviour.exitedGround();
            Debug.Log("You're not grounded");
            //if(!playerBehaviour.jumping && !playerBehaviour.isGrounded){}
        }
    }
    
    void OnCollisionStay2D(Collision2D collision) {
        
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
        //    isGrounded = true; // Ensure grounded state during the collision
        //    jumpCancelled = false;
        //    jumping = false;
        //}
        
    }
    /*
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
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}

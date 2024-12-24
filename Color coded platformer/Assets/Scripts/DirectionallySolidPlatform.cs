using UnityEngine;

public class DirectionallySolidPlatform : MonoBehaviour
{
    [SerializeField] private Collider2D collisionCollider;
    [SerializeField] private Collider2D triggerCollider;
    public enum DirectionOptions
    {
        Left,
        Right,
        Up,
        Down
    }
    
    [SerializeField] private DirectionOptions selectedDirection; // Expose this as a property

    public DirectionOptions SelectedDirection => selectedDirection; // Public getter for VelocityChecker
    [SerializeField] bool ignoreLadder;
    //[SerializeField] GameObject player;
    private void Start()
    {
        if (collisionCollider != null && !collisionCollider.isTrigger)
        {
            Debug.Log($"collisionCollider set correctly: {collisionCollider.name}");
        }
        if (triggerCollider != null && triggerCollider.isTrigger)
        {
            Debug.Log($"triggerCollider set correctly: {triggerCollider.name}");
        }
        /*
        // Assign the colliders
        collisionCollider = GetComponent<Collider2D>();


        // Validate collisionCollider
        if (collisionCollider != null && !collisionCollider.isTrigger)
        {
            Debug.Log($"collisionCollider set correctly: {collisionCollider.name}");
        }
        else
        {
            Debug.LogWarning("collisionCollider is either null or set as a trigger. Please verify your setup.");
            collisionCollider = null; // Clear invalid assignment
        }
        triggerCollider = GetComponent<Collider2D>();
        // Validate triggerCollider
        if (triggerCollider != null && triggerCollider.isTrigger)
        {
            Debug.Log($"triggerCollider set correctly: {triggerCollider.name}");
        }
        else
        {
            Debug.LogWarning("triggerCollider is either null or not set as a trigger. Please verify your setup.");
            triggerCollider = null; // Clear invalid assignment
        }
        */
        // Optional: Rotate based on direction
        switch (selectedDirection)
        {
            case DirectionOptions.Left:
                transform.Rotate(0f, 0f, 90f, Space.Self);
                break;
            case DirectionOptions.Right:
                transform.Rotate(0f, 0f, 270f, Space.Self);
                break;
            case DirectionOptions.Up:
                transform.Rotate(0f, 0f, 0f, Space.Self);
                break;
            case DirectionOptions.Down:
                transform.Rotate(0f, 0f, 180f, Space.Self);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (objectRB != null)
        {
            /*
            if(selectedDirection == DirectionOptions.Left && objectRB.linearVelocity.x < 0){
                Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            }
            else if(selectedDirection == DirectionOptions.Right && objectRB.linearVelocity.x > 0){
                Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            }
            else if(selectedDirection == DirectionOptions.Up && objectRB.linearVelocity.y > 0){
                Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            }
            else if(selectedDirection == DirectionOptions.Down && objectRB.linearVelocity.y < 0){
                Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            }
            */
            if((selectedDirection == DirectionOptions.Left && objectRB.linearVelocity.x < 0) || 
            (selectedDirection == DirectionOptions.Right && objectRB.linearVelocity.x > 0) || 
            (selectedDirection == DirectionOptions.Up && objectRB.linearVelocity.y > 0) || 
            (selectedDirection == DirectionOptions.Down && objectRB.linearVelocity.y < 0) ||
            (selectedDirection == DirectionOptions.Down && objectRB.linearVelocity.y == 0)
            ){
                Physics2D.IgnoreCollision(collisionObject, collisionCollider, true);
                Debug.Log($"OnTriggerEnter2D is Ignoring collision with {collision.gameObject}.");
            }
            //Debug.Log("Ignoring collision with ladder.");
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (objectRB != null)
        {
            /*
            if(selectedDirection == DirectionOptions.Left && objectRB.linearVelocity.x < 0){
                Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            }
            else if(selectedDirection == DirectionOptions.Right && objectRB.linearVelocity.x > 0){
                Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            }
            else if(selectedDirection == DirectionOptions.Up && objectRB.linearVelocity.y > 0){
                Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            }
            else if(selectedDirection == DirectionOptions.Down && objectRB.linearVelocity.y < 0){
                Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            }
            */
            if((selectedDirection == DirectionOptions.Left && objectRB.linearVelocity.x < 0) || 
            (selectedDirection == DirectionOptions.Right && objectRB.linearVelocity.x > 0) || 
            (selectedDirection == DirectionOptions.Up && objectRB.linearVelocity.y > 0) || 
            (selectedDirection == DirectionOptions.Down && objectRB.linearVelocity.y < 0) ||
            (selectedDirection == DirectionOptions.Down && objectRB.linearVelocity.y == 0)
            ){
                Physics2D.IgnoreCollision(collisionObject, collisionCollider, true);
                Debug.Log($"OnTriggerEnter2D is Ignoring collision with {collision.gameObject}.");
            }
            //Debug.Log("Ignoring collision with ladder.");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(collisionObject, collisionCollider, false);
        Debug.Log($"{collision.gameObject.name} exited platform. Collision re-enabled.");
    }
    
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ladder") && ignoreLadder)
        {
            // Ignore collisions with objects tagged as "Ladder"
            Collider2D ladderCollider = collision.gameObject.GetComponent<Collider2D>();
            if (ladderCollider != null)
            {
                //Physics2D.IgnoreCollision(ladderCollider, GetComponent<Collider2D>());
                //Debug.Log("Ignoring collision with ladder.");
            }
            return; // Skip further processing for ladder collisions
        }
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            //Physics2D.IgnoreCollision(collisionObject, collisionCollider, true);
        }
        else if(collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collisionObject, collisionCollider, false);
            Debug.Log("OnCollisionEnter dictates player should be touching block.");
        }
    }
    void OnCollisionStay2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ladder") && ignoreLadder)
        {
            // Ignore collisions with objects tagged as "Ladder"
            Collider2D ladderCollider = collision.gameObject.GetComponent<Collider2D>();
            if (ladderCollider != null)
            {
                Physics2D.IgnoreCollision(ladderCollider, GetComponent<Collider2D>());
                Debug.Log("Ignoring collision with ladder.");
            }
            return; // Skip further processing for ladder collisions
        }
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        /*
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collisionObject, collisionCollider, true);
        }
        */
        if(collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collisionObject, collisionCollider, false);
            Debug.Log("OnCollisionStay dictates player should be touching block.");
        }
    }
    void OnCollisionExit2D(Collision2D collision){
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if(collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collisionObject, collisionCollider, false);
        }
    }
    private void FixedUpdate()
    {
        
    }
}
/*
    
*/
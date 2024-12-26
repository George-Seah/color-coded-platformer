using UnityEngine;

public class UpDirectionalPlatform : MonoBehaviour
{
    [SerializeField] private Collider2D collisionCollider;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] bool partOfLadder;
    [SerializeField] Collider2D collisionObject;
    [SerializeField] Rigidbody2D objectRB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //collisionCollider = GetComponent<Collider2D>();

    }
    /*
    void OnCollisionEnter2D(Collision2D collision){
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (objectRB != null && objectRB.linearVelocity.y > 0)
        {

            Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            //Debug.Log("Ignoring collision with ladder.");
        }
    }
    void OnCollisionExit2D(Collision2D collision){
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();

            Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), false);
            //Debug.Log("Ignoring collision with ladder.");
    }
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(partOfLadder && !collision.gameObject.CompareTag("Ladder")){
            collisionObject = collision.gameObject.GetComponent<Collider2D>();
            objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        if (objectRB != null && objectRB.linearVelocity.y > 0)
        {

            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, true);
            //Debug.Log("Ignoring collision with ladder.");
        }
        else if(objectRB != null && collision.gameObject.CompareTag("Player")
         && collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, true);
        }
        else if(collision.gameObject.CompareTag("Player")
         && !collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            //Testing
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, false);
        }
        else{
            //Testing
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(partOfLadder && !collision.gameObject.CompareTag("Ladder")){
            collisionObject = collision.gameObject.GetComponent<Collider2D>();
            objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        if (objectRB != null && objectRB.linearVelocity.y > 0)
        {

            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, true);
            //Debug.Log("Ignoring collision with ladder.");
        }
        else if(objectRB != null && collision.gameObject.CompareTag("Player")
         && collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, true);
        }
        else if(collision.gameObject.CompareTag("Player")
         && !collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            //Testing
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, false);
        }
        else{
            //Testing
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Testing
        
        collisionObject = collision.gameObject.GetComponent<Collider2D>();
        objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        //Physics2D.IgnoreCollision(collisionObject, collisionCollider, false);
        
        //Debug.Log("Ignoring collision with ladder.");
    }
    
    void OnCollisionEnter2D(Collision2D collision){
        //Testing
        /*
        if(partOfLadder && !collision.gameObject.CompareTag("Ladder")){
            collisionObject = collision.gameObject.GetComponent<Collider2D>();
            objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        */
        /*
        if(collision.gameObject.CompareTag("GroundCollisionCheck")
         && collision.gameObject.GetComponentInParent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponentInParent<Collider2D>(), collisionCollider, true);//////
        }
        else if(collision.gameObject.CompareTag("GroundCollisionCheck")
         && !collision.gameObject.GetComponentInParent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponentInParent<Collider2D>(), collisionCollider, false);
        }
        */
        

        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, true);//////
        }
        else if(collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, false);
            Debug.Log("OnCollisionEnter dictates player should be touching block.");
        }
        
    }
    void OnCollisionStay2D(Collision2D collision){
        //Testing
        /*
        if((partOfLadder && !collision.gameObject.CompareTag("Ladder"))){
            collisionObject = collision.gameObject.GetComponent<Collider2D>();
            objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        */
        /*
        if(collision.gameObject.CompareTag("GroundCollisionCheck")
         && collision.gameObject.GetComponentInParent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponentInParent<Collider2D>(), collisionCollider, true);//////
        }
        else if(collision.gameObject.CompareTag("GroundCollisionCheck")
         && !collision.gameObject.GetComponentInParent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponentInParent<Collider2D>(), collisionCollider, false);
        }
        */
        
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, true);
        }
        
        if(collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collisionCollider, false);
            Debug.Log("OnCollisionStay dictates player should be touching block.");
        }
        
    }
    void OnCollisionExit2D(Collision2D collision){
        //Testing
        /*
        if(partOfLadder && !collision.gameObject.CompareTag("Ladder")){
            collisionObject = collision.gameObject.GetComponent<Collider2D>();
            objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        if(collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Physics2D.IgnoreCollision(collisionObject, collisionCollider, false);
        }
        */
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //Testing
        /*
        if(partOfLadder && collisionObject != null && collisionObject.gameObject.CompareTag("Player")){
            Debug.Log("Player detected.");
            if(collisionObject.gameObject.GetComponent<PlayerBehaviour>().onLadder){
                Physics2D.IgnoreCollision(collisionObject, collisionCollider, true);
            }
            else{
                //Physics2D.IgnoreCollision(collisionObject, collisionCollider, false);
            }
        }
        */


        /*
        else if (objectRB != null && objectRB.linearVelocity.y > 0)
        {

            Physics2D.IgnoreCollision(collisionObject, collisionCollider, true);
            //Debug.Log("Ignoring collision with ladder.");
        }
        else if(collisionObject != null){
            Physics2D.IgnoreCollision(collisionObject, collisionCollider, false);
        }
        */
    }
}

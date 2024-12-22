using UnityEngine;

public class UpDirectionalPlatform : MonoBehaviour
{
    private Collider2D platformCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformCollider = GetComponent<Collider2D>();

    }
    void OnCollisionEnter2D(Collision2D collision){
        Collider2D collisionObject = collision.gameObject.GetComponent<Collider2D>();
        Rigidbody2D objectRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (objectRB != null && objectRB.linearVelocity.y > 0)
        {

            Physics2D.IgnoreCollision(collisionObject, GetComponent<Collider2D>(), true);
            //Debug.Log("Ignoring collision with ladder.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

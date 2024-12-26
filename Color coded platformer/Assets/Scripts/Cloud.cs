using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 5f; // Speed of the cloud
    public Vector3 direction = Vector3.right; // Direction of movement
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.useGravity = false; // Ensure gravity is off
        
    }
    void FixedUpdate(){
        rb.linearVelocity = direction.normalized * speed;
        //transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Cloud collided with {collision.gameObject.name}");

        if (!collision.gameObject.CompareTag("Cannon") && !collision.gameObject.CompareTag("Waddler") &&
        !collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Cannonball hit the Ground using OnCollisionEnter2D!");
            Destroy(gameObject);
        }
        
    }
    public void SetDirection(){

    }
    public void SetSpeed(){

    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground")){
            Debug.Log($"Cannonball detected {collision.gameObject} using OnTriggerEnter2D");
        }
    }
    */
}

using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float speed = 20f; // Speed of the cannonball
    public Vector3 direction = Vector3.forward; // Direction of movement
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.useGravity = false; // Ensure gravity is off
        rb.linearVelocity = direction.normalized * speed;
    }
    void FixedUpdate(){
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Cannonball collided with {collision.gameObject.name}");
        if(collision.gameObject.CompareTag("Player")){
            GameObject.Find("Game Manager").GetComponent<GameManager>().ReloadScene();
        }
        else if(collision.gameObject.CompareTag("Waddler")){
            if(collision.gameObject.GetComponentInChildren<Key>() != null){
                collision.gameObject.GetComponentInChildren<Key>().Drop();
            }
            Destroy(collision.gameObject);
        }
        if (!collision.gameObject.CompareTag("Cannon") && !collision.gameObject.CompareTag("Ladder") && !collision.gameObject.CompareTag("Cloud"))
        {
            //Debug.Log("Cannonball hit the Ground using OnCollisionEnter2D!");
            Destroy(gameObject);
        }
        
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

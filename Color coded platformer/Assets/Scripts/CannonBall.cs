using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float speed = 20f; // Speed of the cannonball
    public Vector3 direction = Vector3.forward; // Direction of movement

    //private Rigidbody2D rb;

    
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        
    }
    void FixedUpdate(){
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision){
        Debug.Log($"Cannonball detected {collision.gameObject} using OnCollisionEnter2D");
        Destroy(gameObject);
        if(collision.gameObject.CompareTag("Ground")){
            
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Cannonball detected {collision.gameObject} using OnTriggerEnter2D");
    }
    
}

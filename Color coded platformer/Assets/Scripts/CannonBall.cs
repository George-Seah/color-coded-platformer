using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float speed = 20f; // Speed of the cannonball
    public Vector3 direction = Vector3.forward; // Direction of movement

    private Rigidbody rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    void FixedUpdate(){
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}

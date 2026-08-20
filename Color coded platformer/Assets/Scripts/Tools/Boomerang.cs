using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed = 15f;
    public float returnDelay = 0.5f;
    
    private Transform thrower;
    private bool returning = false;
    private Rigidbody2D rb;

    public void Throw(Transform throwerTransform, Vector2 direction)
    {
        thrower = throwerTransform;
        rb = GetComponent<Rigidbody2D>();
        
        rb.linearVelocity = direction.normalized * speed;
        Invoke(nameof(StartReturn), returnDelay);
    }

    void StartReturn()
    {
        returning = true;
    }

    void FixedUpdate()
    {
        if (returning && thrower != null)
        {
            Vector2 direction = (thrower.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;

            if (Vector2.Distance(transform.position, thrower.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}

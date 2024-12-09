using UnityEngine;

public class Waddler : MonoBehaviour
{
    [SerializeField] bool lookingRight;
    [SerializeField] float speed;
    private Rigidbody2D rb;
    [SerializeField] private Key currentKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.DrawLine(contact.point, contact.point + contact.normal, Color.red, 2f);
            Debug.Log($"Normal: {contact.normal}");

            Vector2 collisionNormal = contact.normal;
            float collisionAngle = Vector2.Angle(Vector2.up, collisionNormal);

            Debug.Log($"Collision Angle: {collisionAngle}°");
            //
            //Vector2 leftCollision = Vector2(1, 0);
            if(collisionNormal.x == 1 && !lookingRight){
                lookingRight = true;
            }
            else if(collisionNormal.x == -1 && lookingRight){
                lookingRight = false;
            }

        }

        if (collision.gameObject.CompareTag("Key"))
        {
            Key key = collision.gameObject.GetComponent<Key>();
            if (key != null && !key.ReadPickedUp())
            {
                currentKey = key;
                currentKey.PickUp(transform);
                Debug.Log("Key detected. Ready to pick up.");
            }
        }
    }
    void FixedUpdate()
    {
        if(currentKey != null){
            currentKey.SetDirection(lookingRight);
        }
            
        //
        if(lookingRight){
            GetComponent<SpriteRenderer>().flipX = true;
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            
        }
        else{
            GetComponent<SpriteRenderer>().flipX = false;
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }
    }
}

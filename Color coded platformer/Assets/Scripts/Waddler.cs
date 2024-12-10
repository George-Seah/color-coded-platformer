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
            Vector2 leftCollision = new Vector2(1, 0);
            Vector2 rightCollision = new Vector2(-1, 0);
            Debug.Log($"Collision Angle: {collisionAngle}°");
            //Debug.Log($"collisionNormal.x: {collisionNormal.x}");
            //
            //
            //collisionNormal.x == 1
            if(!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Key")){
                if(lookingRight && collisionNormal == rightCollision){
                    lookingRight = false;
                    Debug.Log($"Waddler faces right as it walks into wall");
                }
                //collisionNormal.x == -1
                else if(!lookingRight && collisionNormal == leftCollision){
                    lookingRight = true;
                    Debug.Log($"Waddler faces left as it walks into wall");
                }
            }




        }

        if (collision.gameObject.CompareTag("Key"))
        {
            Key key = collision.gameObject.GetComponent<Key>();
            if (key != null && !key.ReadPickedUp())
            {
                currentKey = key;
                currentKey.PickUp(transform);
                Debug.Log("Key detected by Waddler and picked up.");
            }
        }
    }
    public void SetKey(Key key){
        currentKey = key;
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
            /*
            if(currentKey != null){
                currentKey.SetDirection(true);
            }
            */
        }
        else{
            GetComponent<SpriteRenderer>().flipX = false;
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            /*
            if(currentKey != null){
                currentKey.SetDirection(false);
            }
            */   
        }
    }
}

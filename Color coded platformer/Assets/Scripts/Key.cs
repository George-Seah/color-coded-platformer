using UnityEngine;

public class Key : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D keyCollider;
    [SerializeField] private bool isPickedUp = false;

    private Transform playerTransform;
    [SerializeField] bool canBeStolen = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        keyCollider = GetComponent<Collider2D>();

        if (rb == null || keyCollider == null)
        {
            Debug.LogError("Key requires both Rigidbody2D and Collider2D components!");
        }
    }

    public void PickUp(Transform person)
    {
        isPickedUp = true;

        Debug.Log($"{person.gameObject} picked up the key");

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        transform.SetParent(person);
        transform.localPosition = new Vector3(-1f, 0f, 0f);

        keyCollider.isTrigger = true;
    }

    public void Drop()
    {
        if (isPickedUp)
        {
            // Detach from the player
            transform.SetParent(null);

            // Reset Rigidbody2D properties for physical interaction
            rb.bodyType = RigidbodyType2D.Dynamic; 
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero; // Clear any residual velocity

            // Ensure the collider is no longer a trigger for physical collision
            keyCollider.isTrigger = false;

            // Optional: Add a small impulse to visually separate the dropped key
            rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);

            isPickedUp = false; // Update the state
            canBeStolen = true;
            Debug.Log("Key dropped");
        }
    }


    public bool ReadPickedUp()
    {
        return isPickedUp;
    }
    public void SetDirection(bool holderIsLookingRight){
        if(isPickedUp){
            if(holderIsLookingRight){
                transform.localPosition = new Vector3(1f, 0f, 0f);
                GetComponent<SpriteRenderer>().flipX = true;
                Debug.Log("Key should be facing right.");
            }
            else{
                transform.localPosition = new Vector3(-1f, 0f, 0f);
                GetComponent<SpriteRenderer>().flipX = false;
                Debug.Log("Key should be facing left.");
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Player steals key from Waddler
        if (collision.gameObject.CompareTag("Player") && transform.parent.gameObject.CompareTag("Waddler"))
        {

            Debug.Log("Key detects player while being held by Waddler.");
            if(canBeStolen){
                collision.gameObject.GetComponent<PlayerBehaviour>().SetKey(gameObject.GetComponent<Key>());
                Debug.Log("Key should be stolen by Player from Waddler.");
                canBeStolen = false;
                PickUp(collision.gameObject.transform);
            }
        }
        //Waddler steals key from Player
        else if (collision.gameObject.CompareTag("Waddler") && transform.parent.gameObject.CompareTag("Player"))
        {

            Debug.Log("Key detects Waddler while being held by Player.");
            if(canBeStolen){
                collision.gameObject.GetComponent<Waddler>().SetKey(gameObject.GetComponent<Key>());
                Debug.Log("Key should be stolen by Waddler from Player.");
                canBeStolen = false;
                PickUp(collision.gameObject.transform);
                //SetDirection(collision.gameObject.GetComponent<Waddler>().);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Player") && transform.parent.gameObject.CompareTag("Waddler"))
        {
            //collision.gameObject.GetComponent<PlayerBehaviour>().SetKey(null);
            Debug.Log("Player has left key hitbox while being held by Waddler.");
        }
        else if (collision.gameObject.CompareTag("Waddler") && transform.parent.gameObject.CompareTag("Player"))
        {
            //collision.gameObject.GetComponent<Waddler>().SetKey(null);
            Debug.Log("Waddler has left key hitbox while being held by Player.");
        }
        */
        if(isPickedUp && !canBeStolen){
            if(transform.parent.gameObject.CompareTag("Player") && collision.gameObject.CompareTag("Waddler")){
                canBeStolen = true;
            }
            else if(transform.parent.gameObject.CompareTag("Waddler") && collision.gameObject.CompareTag("Player")){
                canBeStolen = true;
            }
        }

        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
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
    }
    void Update()
    {
        if (isPickedUp && transform.parent.gameObject.layer != LayerMask.NameToLayer("Player")
        && transform.parent.gameObject.layer != LayerMask.NameToLayer("Default")
        && transform.parent.gameObject.layer != gameObject.layer
        && gameObject.layer != LayerMask.NameToLayer("Default"))
        {
            Drop();
        }
    }
}

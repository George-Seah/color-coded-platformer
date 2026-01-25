using UnityEngine;

public class Key : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D keyCollider;
    [SerializeField] bool isPickedUp = false;

    [SerializeField] Transform playerTransform;
    [SerializeField] bool canBeStolen = true;
    [SerializeField] bool facingRight = true;
    void Awake()
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
            if(transform.parent.gameObject.CompareTag("Waddler")){
                transform.parent.gameObject.GetComponent<Waddler>().SetKey(null);
            }
            else if(transform.parent.gameObject.CompareTag("Player")){
                transform.parent.gameObject.GetComponent<PlayerBehaviour>().SetKey(null);
            }
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
            facingRight = holderIsLookingRight;
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Player steals key from Waddler
        if (collision.gameObject.CompareTag("Player") && transform.parent.gameObject.CompareTag("Waddler"))
        {

            Debug.Log("Key detects player while being held by Waddler.");
            if(canBeStolen && collision.gameObject.GetComponent<PlayerBehaviour>().currentKey == null){
                transform.parent.gameObject.GetComponent<Waddler>().SetKey(null);
                collision.gameObject.GetComponent<PlayerBehaviour>().SetKey(gameObject.GetComponent<Key>());
                SetDirection(collision.gameObject.GetComponent<PlayerBehaviour>().lookingRight);
                /*
                if(!collision.gameObject.GetComponent<PlayerBehaviour>().lookingRight){
                    transform.localPosition = new Vector3(1f, 0f, 0f);
                    GetComponent<SpriteRenderer>().flipX = true;
                    Debug.Log("Key should be facing right.");
                }
                else{
                    transform.localPosition = new Vector3(-1f, 0f, 0f);
                    GetComponent<SpriteRenderer>().flipX = false;
                    Debug.Log("Key should be facing left.");
                }*/
                Debug.Log("Key should be stolen by Player from Waddler.");
                canBeStolen = false;
                PickUp(collision.gameObject.transform);
            }
        }
        //Waddler steals key from Player
        else if (collision.gameObject.CompareTag("Waddler") && transform.parent.gameObject.CompareTag("Player"))
        {

            Debug.Log("Key detects Waddler while being held by Player.");
            if(canBeStolen && collision.gameObject.GetComponent<Waddler>().currentKey == null){
                transform.parent.gameObject.GetComponent<PlayerBehaviour>().SetKey(null);
                collision.gameObject.GetComponent<Waddler>().SetKey(gameObject.GetComponent<Key>());
                SetDirection(collision.gameObject.GetComponent<Waddler>().lookingRight);
                /*
                if(!collision.gameObject.GetComponent<Waddler>().lookingRight){
                    transform.localPosition = new Vector3(1f, 0f, 0f);
                    GetComponent<SpriteRenderer>().flipX = true;
                    Debug.Log("Key should be facing right.");
                }
                else{
                    transform.localPosition = new Vector3(-1f, 0f, 0f);
                    GetComponent<SpriteRenderer>().flipX = false;
                    Debug.Log("Key should be facing left.");
                }
                */
                Debug.Log("Key should be stolen by Waddler from Player.");
                canBeStolen = false;
                PickUp(collision.gameObject.transform);
                //SetDirection(collision.gameObject.GetComponent<Waddler>().);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
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
        if(isPickedUp && transform.parent.gameObject.layer != LayerMask.NameToLayer("Default") &&
        gameObject.layer != LayerMask.NameToLayer("Default") &&
        transform.parent.gameObject.layer != LayerMask.NameToLayer("Player") &&
        transform.parent.gameObject.layer != gameObject.layer){
            Drop();
        }
        else if(isPickedUp && gameObject.layer != LayerMask.NameToLayer("Default") &&
        transform.parent.gameObject.layer == LayerMask.NameToLayer("Player") &&
        Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), gameObject.layer)){
            Drop();
        }

        if (isPickedUp)
        {
            if (transform.parent != null && (transform.parent.gameObject.CompareTag("Waddler") || transform.parent.gameObject.CompareTag("Player")))
            {
                if(transform.parent.gameObject.CompareTag("Waddler")){
                    SetDirection(transform.parent.gameObject.GetComponent<Waddler>().lookingRight);
                }
                else if(transform.parent.gameObject.CompareTag("Player") && transform.parent.gameObject.GetComponent<PlayerBehaviour>().lookingRight){
                    facingRight = true;
                }
                else if(transform.parent.gameObject.CompareTag("Player") && !transform.parent.gameObject.GetComponent<PlayerBehaviour>().lookingRight){
                    facingRight = false;
                }
                if(facingRight){
                    if(transform.parent.gameObject.CompareTag("Waddler")){
                        transform.localPosition = new Vector3(1f, 0f, 0f);
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                    if(transform.parent.gameObject.CompareTag("Player")){
                        transform.localPosition = new Vector3(1f, 0f, 0f);
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    
                    Debug.Log("Key should be facing right.");
                }
                else{
                    if(transform.parent.gameObject.CompareTag("Waddler")){
                        transform.localPosition = new Vector3(-1f, 0f, 0f);
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    if(transform.parent.gameObject.CompareTag("Player")){
                        transform.localPosition = new Vector3(1f, 0f, 0f);
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    Debug.Log("Key should be facing left.");
                }
            }
            /*
            if(transform.parent != null && transform.parent.gameObject.CompareTag("Player")){
                if(transform.parent.gameObject.GetComponent<PlayerBehaviour>().lookingRight){
                    GetComponent<SpriteRenderer>().flipX = true;
                    facingRight = true;
                }
                else{
                    GetComponent<SpriteRenderer>().flipX = false;
                    facingRight = false;
                }
            }
            */
        }


    }
}

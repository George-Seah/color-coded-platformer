using UnityEngine;

public class Key : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D keyCollider;
    private bool isPickedUp = false;

    void Start()
    {
        // Cache Rigidbody2D and Collider2D
        rb = GetComponent<Rigidbody2D>();
        keyCollider = GetComponent<Collider2D>();

        if (rb == null || keyCollider == null)
        {
            Debug.LogError("Key requires both Rigidbody2D and Collider2D components!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;

            Debug.Log("Player picked up the key");

            // Disable Rigidbody physics
            rb.isKinematic = true;

            // Enable the key to be a child of the player
            transform.SetParent(collision.transform);

            // Adjust the key's position relative to the player
            transform.localPosition = new Vector3(0f, 1f, 0f);

            // Optional: Disable collisions to prevent interaction with other objects
            keyCollider.isTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited key area");
        }
    }

    public void DropKey()
    {
        if (isPickedUp)
        {
            isPickedUp = false;

            // Detach the key from the player
            transform.SetParent(null);

            // Re-enable Rigidbody physics
            rb.isKinematic = false;

            // Re-enable normal collisions
            keyCollider.isTrigger = false;

            Debug.Log("Key dropped");
        }
    }
}

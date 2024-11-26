using UnityEngine;

public class Key : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D keyCollider;
    [SerializeField] private bool isPickedUp = false;

    private Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        keyCollider = GetComponent<Collider2D>();

        if (rb == null || keyCollider == null)
        {
            Debug.LogError("Key requires both Rigidbody2D and Collider2D components!");
        }
    }

    public void PickUp(Transform player)
    {
        isPickedUp = true;

        Debug.Log("Player picked up the key");

        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;

        transform.SetParent(player);
        transform.localPosition = new Vector3(0f, 1f, 0f);

        keyCollider.isTrigger = true;
    }

public void Drop()
{
    if (isPickedUp)
    {
        // Detach from the player
        transform.SetParent(null);

        // Reset Rigidbody2D properties for physical interaction
        rb.isKinematic = false; 
        rb.simulated = true;
        rb.linearVelocity = Vector2.zero; // Clear any residual velocity

        // Ensure the collider is no longer a trigger for physical collision
        keyCollider.isTrigger = false;

        // Optional: Add a small impulse to visually separate the dropped key
        rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);

        isPickedUp = false; // Update the state
        Debug.Log("Key dropped");
    }
}


    public bool ReadPickedUp()
    {
        return isPickedUp;
    }
}

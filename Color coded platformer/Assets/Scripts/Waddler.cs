using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Waddler : MonoBehaviour
{
    [SerializeField] bool lookingRight;
    [SerializeField] float speed;
    private Rigidbody2D rb;
    [SerializeField] private Key currentKey;
    bool grounded;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Transform cannonTransform, float launchForce, bool facingRight)
    {
        lookingRight = facingRight;
        // Calculate the launch direction based on the cannon's rotation
        float angleInRadians = (cannonTransform.eulerAngles.z + 90) * Mathf.Deg2Rad;
        Vector2 launchDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

        // Apply the force to the Rigidbody2D
        rb.linearVelocity = launchDirection * launchForce;

        // Optional debug information
        Debug.Log($"Waddler launched at angle: {cannonTransform.eulerAngles.z} degrees with force: {launchForce}");
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            grounded = false;
        }
    }
    void FixedUpdate()
    {
        if (currentKey != null)
        {
            currentKey.SetDirection(lookingRight);

            if (currentKey.gameObject.layer != LayerMask.NameToLayer("Default") &&
                currentKey.gameObject.layer != gameObject.layer &&
                gameObject.layer != LayerMask.NameToLayer("Default"))
            {
                currentKey.Drop();
            }
        }
        if(grounded){
            if (lookingRight)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            }
        }

    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetKey(Key key)
    {
        currentKey = key;
    }
}


/*
using UnityEngine;

public class BoxCastIgnoreTag : MonoBehaviour
{
    public string tagToIgnore = "Ladder";      // Tag to ignore
    public Vector2 direction = Vector2.right; // Direction to cast the box
    public float castDistance = 5f;           // Maximum distance for the BoxCast
    public LayerMask layerMask;               // Layers to include in the collision check

    void Update()
    {
        // Get the origin, size, and angle from the Transform
        Vector2 origin = transform.position;       // Origin is the GameObject's position
        Vector2 boxSize = transform.localScale;    // Box size is the GameObject's scale
        float boxAngle = transform.eulerAngles.z;  // Box angle is the GameObject's Z-axis rotation

        // Perform the BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, boxAngle, direction, castDistance, layerMask);

        // Visualize the BoxCast for debugging
        DebugDrawBoxCast(origin, boxSize, boxAngle, direction, castDistance, Color.red);

        // Check if something was hit
        if (hit.collider != null)
        {
            // Check if the hit object has the tag to ignore
            if (hit.collider.CompareTag(tagToIgnore))
            {
                Debug.Log("Ignored object with tag: " + hit.collider.tag);
            }
            else
            {
                Debug.Log("Hit object: " + hit.collider.name);
                // Handle the hit object here
            }
        }
    }

    // Helper method to visualize the BoxCast in the Scene view
    private void DebugDrawBoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, Color color)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 extents = size * 0.5f;

        // Define the four corners of the box
        Vector2[] corners = new Vector2[4];
        corners[0] = origin + rotation * new Vector2(-extents.x, -extents.y); // Bottom-left
        corners[1] = origin + rotation * new Vector2(-extents.x, extents.y);  // Top-left
        corners[2] = origin + rotation * new Vector2(extents.x, extents.y);   // Top-right
        corners[3] = origin + rotation * new Vector2(extents.x, -extents.y);  // Bottom-right

        // Draw the box
        Debug.DrawLine(corners[0], corners[1], color);
        Debug.DrawLine(corners[1], corners[2], color);
        Debug.DrawLine(corners[2], corners[3], color);
        Debug.DrawLine(corners[3], corners[0], color);

        // Draw the direction line
        Debug.DrawLine(origin, origin + direction.normalized * distance, color);
    }
}
*/


/*
        // Perform the BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.localScale, transform.eulerAngles.z, Vector2.right, boxRadius);
        if (hit.collider != null)
        {
            // Check if the hit object has the tag to ignore
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Ignored object with tag: " + hit.collider.tag);
                Physics2D.IgnoreCollision(hit.collider.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            else
            {
                Debug.Log("Hit object: " + hit.collider.name);
                // Handle the hit object here
            }
        }
*/
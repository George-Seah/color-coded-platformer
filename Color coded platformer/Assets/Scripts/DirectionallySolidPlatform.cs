using UnityEngine;

public class DirectionallySolidPlatform : MonoBehaviour
{
    private Collider2D platformCollider;
    public enum DirectionOptions
    {
        Left,
        Right,
        Up,
        Down
    }
    public DirectionOptions selectedDirection;
    
    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        switch (selectedDirection)
        {
            case DirectionOptions.Left:
                Debug.Log("Left selected");
                gameObject.transform.Rotate(0f, 0f, 90f, Space.Self);
            break;
            case DirectionOptions.Right:
                Debug.Log("Right selected");
                gameObject.transform.Rotate(0f, 0f, 270f, Space.Self);
            break;
            case DirectionOptions.Up:
                Debug.Log("Up selected");
                gameObject.transform.Rotate(0f, 0f, 0f, Space.Self);
            break;
            case DirectionOptions.Down:
                Debug.Log("Down selected");
                gameObject.transform.Rotate(0f, 0f, 180f, Space.Self);
            break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*
        Rigidbody2D rb = collision.rigidbody;
        if (rb != null)
        {
            // Continuously check if collision should be ignored
            bool shouldIgnore = ShouldIgnoreCollision(rb);
            Physics2D.IgnoreCollision(platformCollider, collision.collider);
        }
        */
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*
        // Re-enable collision when the object leaves the platform
        Collider2D otherCollider = collision.collider;
        if (otherCollider != null)
        {
            Physics2D.IgnoreCollision(platformCollider, otherCollider, false);
        }
        */
    }

}

using UnityEngine;

public class VelocityChecker : MonoBehaviour
{
    private Collider2D platformCollider;
    private DirectionallySolidPlatform.DirectionOptions selectedDirection;
    private ColorPicker colorPicker;
    private void Start()
    {
        // Find and read the DirectionallySolidPlatform's direction
        DirectionallySolidPlatform parentPlatform = GetComponentInParent<DirectionallySolidPlatform>();
        //Get collider of directionally solid platform
        platformCollider = parentPlatform.gameObject.GetComponent<Collider2D>();
        colorPicker = parentPlatform.gameObject.GetComponent<ColorPicker>();
        if (parentPlatform != null)
        {
            selectedDirection = parentPlatform.SelectedDirection;
            Debug.Log($"VelocityChecker detected direction: {selectedDirection}");
        }
        else
        {
            Debug.LogError("Parent platform with DirectionallySolidPlatform script not found!");
        }
        gameObject.layer = LayerMask.NameToLayer($"{colorPicker.selectedColor}");
        if(gameObject.layer == LayerMask.NameToLayer("White")){
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb == null) return;

        Vector2 velocity = rb.linearVelocity;

        // Check directionality and ignore collision accordingly
        switch (selectedDirection)
        {
            case DirectionallySolidPlatform.DirectionOptions.Right:
                if (velocity.x > 0)
                {
                    Physics2D.IgnoreCollision(platformCollider, collision.gameObject.GetComponent<Collider2D>(), true);
                    Debug.Log($"{collision.gameObject} is moving right. Collision ignored.");
                }
                break;
            case DirectionallySolidPlatform.DirectionOptions.Left:
                if (velocity.x < 0)
                {
                    Physics2D.IgnoreCollision(platformCollider, collision.gameObject.GetComponent<Collider2D>(), true);
                    Debug.Log($"{collision.gameObject} is moving left. Collision ignored.");
                }
                break;
            case DirectionallySolidPlatform.DirectionOptions.Up:
                if (velocity.y > 0)
                {
                    Physics2D.IgnoreCollision(platformCollider, collision.gameObject.GetComponent<Collider2D>(), true);
                    Debug.Log($"{collision.gameObject} is moving up. Collision ignored.");
                }
                break;
            case DirectionallySolidPlatform.DirectionOptions.Down:
                if (velocity.y < 0)
                {
                    Physics2D.IgnoreCollision(platformCollider, collision.gameObject.GetComponent<Collider2D>(), true);
                    Debug.Log($"{collision.gameObject} is moving down. Collision ignored.");
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(platformCollider, collision.gameObject.GetComponent<Collider2D>(), false);
        Debug.Log($"{collision.gameObject} exited platform. Collision re-enabled.");
    }
}

using UnityEngine;

public class VelocityChecker : MonoBehaviour
{
    private Collider2D platformCollider;
    private DirectionallySolidPlatform.DirectionOptions selectedDirection;
    private ColorPicker colorPicker;

    private void Start()
    {
        // Find and set up references
        DirectionallySolidPlatform parentPlatform = GetComponentInParent<DirectionallySolidPlatform>();
        if (parentPlatform != null)
        {
            platformCollider = parentPlatform.GetComponent<Collider2D>();
            colorPicker = parentPlatform.GetComponent<ColorPicker>();
            selectedDirection = parentPlatform.SelectedDirection;

            Debug.Log($"VelocityChecker detected direction: {selectedDirection}");
        }
        else
        {
            Debug.LogError("Parent platform with DirectionallySolidPlatform script not found!");
            return;
        }

        // Set game object layer based on color
        if (colorPicker != null)
        {
            
            if (gameObject.layer == LayerMask.NameToLayer("White")){
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else{
                gameObject.layer = LayerMask.NameToLayer($"{colorPicker.selectedColor}");
            }
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Re-enable collisions when the player exits the platform
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(platformCollider, collision, false);
            Debug.Log($"{collision.gameObject.name} exited platform. Collision re-enabled.");
        }
    }

    private void HandleCollision(Collider2D collision)
    {
        if (platformCollider == null)
        {
            Debug.LogError("Platform collider is null!");
            return;
        }

        // Check for ladder condition
        if (collision.CompareTag("Player"))
        {
            PlayerBehaviour playerBehaviour = collision.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null)
            {
                Debug.Log($"Player onLadder value: {playerBehaviour.onLadder}");
                if (playerBehaviour.onLadder)
                {
                    Physics2D.IgnoreCollision(platformCollider, collision, true);
                    Debug.Log("Collision ignored because player is on ladder.");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("PlayerBehaviour script not found on player!");
            }
        }

        // Check directional collision logic
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb == null)
        {
            Debug.LogWarning($"No Rigidbody2D found on {collision.gameObject.name}");
            return;
        }

        Vector2 velocity = rb.linearVelocity;
        Debug.Log($"{collision.gameObject.name} velocity: {velocity}");

        switch (selectedDirection)
        {
            case DirectionallySolidPlatform.DirectionOptions.Right:
                if (velocity.x > 0) IgnoreCollision(collision, "right");
                break;
            case DirectionallySolidPlatform.DirectionOptions.Left:
                if (velocity.x < 0) IgnoreCollision(collision, "left");
                break;
            case DirectionallySolidPlatform.DirectionOptions.Up:
                if (velocity.y > 0) IgnoreCollision(collision, "up");
                break;
            case DirectionallySolidPlatform.DirectionOptions.Down:
                if (velocity.y < 0) IgnoreCollision(collision, "down");
                break;
            default:
                Debug.LogWarning("Unknown direction selected.");
                break;
        }
    }


    private bool IsPlayerOnLadder(GameObject player)
    {
        // Check for a custom PlayerBehaviour script and if the player is flagged as "onLadder"
        PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
        return playerBehaviour != null && playerBehaviour.onLadder;
    }

    private void IgnoreCollision(Collider2D collision, string direction)
    {
        Physics2D.IgnoreCollision(platformCollider, collision, true);
        Debug.Log($"{collision.gameObject.name} is moving {direction}. Collision ignored.");
    }
}


/*using UnityEngine;

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
*/
/*
using UnityEngine;

public class VelocityChecker : MonoBehaviour
{
    private Collider2D platformCollider;
    private DirectionallySolidPlatform.DirectionOptions selectedDirection;
    private ColorPicker colorPicker;
    //[SerializeField] bool playerOnLadder;
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
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
            Debug.Log($"Velocity Checker should make player go through platform at top of ladder.");
            Physics2D.IgnoreCollision(platformCollider, collision.gameObject.GetComponent<Collider2D>(), true);
        }
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
                else if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().onLadder){
                    Physics2D.IgnoreCollision(platformCollider, collision.gameObject.GetComponent<Collider2D>(), true);
                    Debug.Log($"{collision.gameObject} is moving down while on ladder. Collision ignored.");
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
*/

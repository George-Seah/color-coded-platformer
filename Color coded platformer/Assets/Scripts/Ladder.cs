using UnityEngine;
using UnityEngine.InputSystem;

public class Ladder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float climbSpeed = 0.25f;
    [SerializeField] private float normalGravity = 10f;
    [SerializeField] private float exitGravity = 30f;

    [Header("Debug")]
    [SerializeField] private Vector2 moveDirection = Vector2.zero;
    [SerializeField] private bool playerOnLadder;
    [SerializeField] private GameObject player;

    private PlayerInputActions playerControls;
    private InputAction move;
    private GameObject groundChecker;
    private Rigidbody2D playerRB;
    private GameObject ladderPlatform;
    private PlayerBehaviour playerBehaviour;
    private Collider2D playerCollider;
    private Collider2D groundCheckerCollider;
    private Collider2D ladderPlatformCollider;

    private void Awake()
    {
        ladderPlatform = GetComponentInChildren<PlatformEffector2D>()?.gameObject;
        ladderPlatformCollider = ladderPlatform?.GetComponent<Collider2D>();

        playerControls = new PlayerInputActions();
        if (playerControls == null)
        {
            Debug.LogError("PlayerInputActions not initialized!");
        }
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsValidPlayer(collision)) return;

        moveDirection = move.ReadValue<Vector2>();
        CachePlayerReferences(collision);

        if (moveDirection.y != 0 && !playerOnLadder)
        {
            EnterLadder();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsValidPlayer(collision)) return;

        Debug.Log("Player exited Ladder Checker.");
        ExitLadder();
        ClearPlayerReferences();
    }

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();

        if (!playerOnLadder) return;

        // Handle climbing
        playerRB.gravityScale = 0;
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, moveDirection.y * climbSpeed);

        // Lock player X position to ladder
        player.transform.position = new Vector2(
            transform.position.x, 
            player.transform.position.y + (moveDirection.y * climbSpeed)
        );

        // Only exit if the player intentionally moves horizontally 
        // while still on the ladder (not due to auto-slide)
        if (ShouldExitLadder())
        {
            ExitLadder();
            playerRB.gravityScale = exitGravity;
            playerOnLadder = false;
            player = null;
        }
    }

    private bool IsValidPlayer(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return false;

        var pb = collision.GetComponent<PlayerBehaviour>();
        if (pb == null) return false;

        string ladderLayer = LayerMask.LayerToName(gameObject.layer);
        return pb.color == ladderLayer || ladderLayer == "Default";
    }

    private void CachePlayerReferences(Collider2D collision)
    {
        player = collision.gameObject;
        playerRB = collision.GetComponent<Rigidbody2D>();
        playerBehaviour = collision.GetComponent<PlayerBehaviour>();
        playerCollider = collision.GetComponent<Collider2D>();

        groundChecker = collision.GetComponentInChildren<GroundCollisionCheck>()?.gameObject;
        groundCheckerCollider = groundChecker?.GetComponent<Collider2D>();
    }

    private void ClearPlayerReferences()
    {
        player = null;
        playerRB = null;
        playerBehaviour = null;
        playerCollider = null;
        groundChecker = null;
        groundCheckerCollider = null;
    }

    private void EnterLadder()
    {
        SetLadderPlatformCollisions(true);
        playerRB.gravityScale = 0;
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0f);
        playerOnLadder = true;
        playerBehaviour.onLadder = true;
    }

    private void ExitLadder()
    {
        SetLadderPlatformCollisions(false);
        if (playerBehaviour != null) playerBehaviour.onLadder = false;
        if (playerRB != null) playerRB.gravityScale = normalGravity;
        playerOnLadder = false;
    }

    private bool ShouldExitLadder()
    {
        // Exit only if the player intentionally moves horizontally
        if (Mathf.Abs(moveDirection.x) > 0.01f)
        {
            // Only exit if the player is still on the ladder trigger
            // Prevents auto-slide from exiting prematurely
            return true;
        }

        return false;
    }

    private void SetLadderPlatformCollisions(bool ignore)
    {
        if (ladderPlatformCollider == null) return;

        if (groundCheckerCollider != null)
            Physics2D.IgnoreCollision(groundCheckerCollider, ladderPlatformCollider, ignore);

        if (playerCollider != null)
            Physics2D.IgnoreCollision(playerCollider, ladderPlatformCollider, ignore);
    }

    // Keeping these for potential external use
    void DisactivateLadderPlatformCollisions() => SetLadderPlatformCollisions(true);
    void ActivateLadderPlatformCollisions() => SetLadderPlatformCollisions(false);
}
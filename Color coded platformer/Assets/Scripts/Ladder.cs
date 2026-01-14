using UnityEngine;
using UnityEngine.InputSystem;

public class Ladder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float climbSpeed = 0.25f;
    [SerializeField] float normalGravity = 10f;
    [SerializeField] float exitGravity = 30f;

    [Header("Debug")]
    [SerializeField] Vector2 moveDirection = Vector2.zero;

    PlayerInputActions playerControls;
    InputAction move;
    Rigidbody2D playerRB;
    GameObject ladderPlatform;
    PlayerBehaviour playerBehaviour;
    Collider2D playerCollider;
    Collider2D groundCheckerCollider;
    Collider2D ladderPlatformCollider;
    [SerializeField] Collider2D colliderForPlayerIgnoreClimbing;
    Collider2D physicalCollider;//The collider that is not a trigger on the ladder object

    void Awake()
    {
        ladderPlatform = GetComponentInChildren<PlatformEffector2D>()?.gameObject;
        ladderPlatformCollider = ladderPlatform?.GetComponent<Collider2D>();
        
        physicalCollider = GetComponent<Collider2D>();

        while(GetComponent<Collider2D>().isTrigger)
            physicalCollider = GetComponent<Collider2D>();

        if(ladderPlatformCollider) 
            Physics2D.IgnoreCollision(physicalCollider, ladderPlatformCollider, true);

        playerControls = new PlayerInputActions();
        LadderWaddlerCollisionRegistry.RegisterLadder(physicalCollider);
    }

    void OnEnable()
    {
        playerControls.Player.Enable();
        move = playerControls.Player.Move;
        move.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        playerControls.Player.Disable();
    }

    void OnDestroy()
    {
        LadderWaddlerCollisionRegistry.UnregisterLadder(physicalCollider);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Waddler waddler = collision.GetComponentInParent<Waddler>();
        if (waddler != null)
        {
            Collider2D waddlerCollider = waddler.GetComponent<Collider2D>();
            if (waddlerCollider != null)
            {
                Physics2D.IgnoreCollision(waddlerCollider, physicalCollider, true);
            }
            return;
        }

        playerBehaviour = collision.GetComponentInParent<PlayerBehaviour>();
        if (playerBehaviour == null) return;

        playerRB = playerBehaviour.GetComponent<Rigidbody2D>();
        playerCollider = playerBehaviour.GetComponent<Collider2D>();
        groundCheckerCollider = collision.GetComponentInChildren<GroundCollisionCheck>()
            ?.GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (playerBehaviour == null) return;

        moveDirection = move.ReadValue<Vector2>();

        if (moveDirection.y != 0 && !playerBehaviour.onLadder)
        {
            EnterLadder();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        ExitLadder();
        ClearPlayerReferences();
    }

    void FixedUpdate()
    {
        if (playerBehaviour == null || !playerBehaviour.onLadder || playerRB == null)
            return;

        moveDirection = move.ReadValue<Vector2>();

        playerRB.gravityScale = 0f;
        playerRB.linearVelocity = new Vector2(
            playerRB.linearVelocity.x,
            moveDirection.y * climbSpeed
        );

        playerRB.MovePosition(new Vector2(
            transform.position.x,
            playerRB.position.y + (moveDirection.y * climbSpeed)
        ));

        if (ShouldExitLadder())
        {
            ExitLadder();
            playerRB.gravityScale = exitGravity;
        }
    }

    void EnterLadder()
    {
        SetLadderPlatformCollisions(true);

        if (playerBehaviour != null)
        {
            playerBehaviour.onLadder = true;
            playerBehaviour.SetIgnoreMovementCollisions(true);
        }

        playerRB.gravityScale = 0f;
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0f);
    }

    void ExitLadder()
    {
        SetLadderPlatformCollisions(false);

        if (playerBehaviour != null)
        {
            playerBehaviour.onLadder = false;
            playerBehaviour.SetIgnoreMovementCollisions(false);
        }

        if (playerRB != null)
            playerRB.gravityScale = normalGravity;
    }

    bool ShouldExitLadder()
    {
        return Mathf.Abs(moveDirection.x) > 0.01f;
    }

    void ClearPlayerReferences()
    {
        playerRB = null;
        playerBehaviour = null;
        playerCollider = null;
        groundCheckerCollider = null;
    }

    void SetLadderPlatformCollisions(bool ignore)
    {
        if(playerCollider && colliderForPlayerIgnoreClimbing)
        {
            Physics2D.IgnoreCollision(playerCollider, colliderForPlayerIgnoreClimbing, ignore);
            Physics2D.IgnoreCollision(groundCheckerCollider, colliderForPlayerIgnoreClimbing, ignore);
        }
        
        if (ladderPlatformCollider == null) return;

        if (groundCheckerCollider != null)
            Physics2D.IgnoreCollision(groundCheckerCollider, ladderPlatformCollider, ignore);

        if (playerCollider != null)
            Physics2D.IgnoreCollision(playerCollider, ladderPlatformCollider, ignore);
    }

    void DisactivateLadderPlatformCollisions() => SetLadderPlatformCollisions(true);
    void ActivateLadderPlatformCollisions() => SetLadderPlatformCollisions(false);
}

using UnityEngine;
using UnityEngine.InputSystem;

public class Ladder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float climbSpeed = 3f;

    [Header("Debug")]
    Vector2 moveDirection;

    PlayerInputActions controls;
    InputAction move;

    Rigidbody2D rb;
    PlayerBehaviour player;
    Collider2D playerCollider;
    Collider2D groundChecker;
    Collider2D ladderCollider;
    Collider2D ladderPlatformCollider;

    [SerializeField] Collider2D colliderForPlayerIgnoreClimbing;

    bool playerInside;

    const float JOYSTICK_DEADZONE = 0.25f;
    const float KEYBOARD_THRESHOLD = 0.01f;

    void Awake()
    {
        controls = new PlayerInputActions();
        ladderCollider = GetComponent<Collider2D>();

        PlatformEffector2D effector = GetComponentInChildren<PlatformEffector2D>();
        if (effector)
            ladderPlatformCollider = effector.GetComponent<Collider2D>();

        if (ladderPlatformCollider)
            Physics2D.IgnoreCollision(ladderCollider, ladderPlatformCollider, true);

        LadderWaddlerCollisionRegistry.RegisterLadder(ladderCollider);
    }

    void OnEnable()
    {
        controls.Player.Enable();
        move = controls.Player.Move;
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void OnDestroy()
    {
        LadderWaddlerCollisionRegistry.UnregisterLadder(ladderCollider);
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();

        if (!playerInside || player == null)
            return;

        if (!player.onLadder && HasVerticalIntent())
            EnterLadder();

        if (player.onLadder && ShouldExitLadder())
            ExitLadder();
    }

    void FixedUpdate()
    {
        if (player == null || !player.onLadder || !playerInside)
            return;

        rb.gravityScale = 0f;

        // Stay on ladder even when input is zero
        rb.linearVelocity = new Vector2(0f, moveDirection.y * climbSpeed);

        // Lock X to ladder
        rb.position = new Vector2(transform.position.x, rb.position.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Waddler waddler = collision.GetComponentInParent<Waddler>();
        if (waddler)
        {
            Physics2D.IgnoreCollision(
                waddler.GetComponent<Collider2D>(),
                ladderCollider,
                true
            );
            return;
        }

        player = collision.GetComponentInParent<PlayerBehaviour>();
        if (!player) return;

        rb = player.GetComponent<Rigidbody2D>();
        playerCollider = player.GetComponent<Collider2D>();
        groundChecker = collision.GetComponentInChildren<GroundCollisionCheck>()?.GetComponent<Collider2D>();

        playerInside = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (player == null)
            return;

        if (collision.GetComponentInParent<PlayerBehaviour>() != player)
            return;

        playerInside = false;

        if (player.onLadder)
            ExitLadder();

        ClearPlayerReferences();
    }

    /* ========================== LADDER STATE ========================== */

    void EnterLadder()
    {
        if (player.onLadder)
            return;

        SetLadderPlatformCollisions(true);

        player.onLadder = true;
        player.SetIgnoreMovementCollisions(true);
        player.SetIgnoreOneWayPlatforms(true);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        rb.position = new Vector2(transform.position.x, rb.position.y);
    }

    void ExitLadder()
    {
        SetLadderPlatformCollisions(false);

        player.onLadder = false;
        player.SetIgnoreMovementCollisions(false);
        player.SetIgnoreOneWayPlatforms(false);

        player.GetComponentInParent<Rigidbody2D>().gravityScale = player.fallingGravityScale;
    }

    /* ========================== INPUT ========================== */

    bool IsJoystick()
    {
        return move.activeControl != null &&
               move.activeControl.device is Gamepad;
    }

    bool HasVerticalIntent()
    {
        float threshold = IsJoystick() ? JOYSTICK_DEADZONE : KEYBOARD_THRESHOLD;
        return Mathf.Abs(moveDirection.y) > threshold;
    }

    bool ShouldExitLadder()
    {
        if (IsJoystick())
        {
            // Horizontal must dominate
            return Mathf.Abs(moveDirection.x) > JOYSTICK_DEADZONE &&
                   Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y);
        }

        // Keyboard / D-pad
        return Mathf.Abs(moveDirection.x) > KEYBOARD_THRESHOLD;
    }

    /* ========================== COLLISIONS ========================== */

    void SetLadderPlatformCollisions(bool ignore)
    {
        if (playerCollider && colliderForPlayerIgnoreClimbing)
        {
            Physics2D.IgnoreCollision(playerCollider, colliderForPlayerIgnoreClimbing, ignore);
            if (groundChecker)
                Physics2D.IgnoreCollision(groundChecker, colliderForPlayerIgnoreClimbing, ignore);
        }

        if (ladderPlatformCollider == null)
            return;

        if (groundChecker)
            Physics2D.IgnoreCollision(groundChecker, ladderPlatformCollider, ignore);

        if (playerCollider)
            Physics2D.IgnoreCollision(playerCollider, ladderPlatformCollider, ignore);
    }

    void ClearPlayerReferences()
    {
        rb = null;
        player = null;
        playerCollider = null;
        groundChecker = null;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class Ladder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float climbSpeed = 3f;

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
        ladderCollider = GetComponent<Collider2D>();

        PlatformEffector2D effector = GetComponentInChildren<PlatformEffector2D>();
        if (effector)
            ladderPlatformCollider = effector.GetComponent<Collider2D>();

        if (ladderPlatformCollider)
            Physics2D.IgnoreCollision(ladderCollider, ladderPlatformCollider, true);

        LadderWaddlerCollisionRegistry.RegisterLadder(ladderCollider);
    }

    void OnDestroy()
    {
        LadderWaddlerCollisionRegistry.UnregisterLadder(ladderCollider);
    }

    void Update()
    {
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
        rb.linearVelocity = new Vector2(0f, player.moveDirection.y * climbSpeed);

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
        return player.move.activeControl != null &&
               player.move.activeControl.device is Gamepad;
    }

    bool HasVerticalIntent()
    {
        float threshold = IsJoystick() ? JOYSTICK_DEADZONE : KEYBOARD_THRESHOLD;
        return Mathf.Abs(player.moveDirection.y) > threshold;
    }

    bool ShouldExitLadder()
    {
        if (!IsJoystick())
            return Mathf.Abs(player.moveDirection.x) > KEYBOARD_THRESHOLD;

        float x = Mathf.Abs(player.moveDirection.x);
        float y = Mathf.Abs(player.moveDirection.y);

        // Require:
        // 1. Strong horizontal push
        // 2. Almost no vertical intent
        return x > 0.6f && y < 0.15f;
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

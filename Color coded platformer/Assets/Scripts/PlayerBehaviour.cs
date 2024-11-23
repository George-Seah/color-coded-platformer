using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    public PlayerInputActions playerControls;
    private Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction jump;

    public float gravityScale = 10f;
    public float fallingGravityScale = 40f;
    public float buttonTime = 0.3f;
    [SerializeField] private float jumpTime = 0f;
    [SerializeField] private bool jumpInputPressed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpAmount = 1.25f;

    [SerializeField] private float jumpForce;

    private bool jumpCancelled;
    private bool jumping = false;
    private Transform groundCollisionCheck;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        if (playerControls == null)
        {
            Debug.LogError("PlayerInputActions not initialized!");
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCollisionCheck = transform.Find("Player/GroundCollisionCheck");
        jumpForce = Mathf.Sqrt(jumpAmount * -2 * Physics2D.gravity.y);
    }

    private void OnEnable()
    {
        playerControls.Player.Enable(); // Enable the Player action map
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;

        move.Enable();
        jump.Enable();

        jump.performed += ctx => OnJumpPerformed(ctx);
        jump.canceled += ctx => OnJumpCanceled(ctx);
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();

        jump.performed -= OnJumpPerformed;
        jump.canceled -= OnJumpCanceled;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded && !jumping)
        {
            jumping = true;
            jumpTime = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Jump force
            rb.gravityScale = gravityScale;
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (!isGrounded && rb.linearVelocity.y > 0)
        {
            jumpCancelled = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); // Cancel upward velocity
        }
    }

    public void enteredGround()
    {
        isGrounded = true;
        jumping = false;
        jumpCancelled = false;
        rb.gravityScale = gravityScale;
    }

    public void exitedGround()
    {
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y); // Move left/right

        if (isGrounded)
        {
            rb.gravityScale = gravityScale;
        }
        else if (!jumping && !isGrounded)
        {
            rb.gravityScale = fallingGravityScale; // Increased gravity while falling
        }

        if (jumping && !jumpCancelled && jumpTime < buttonTime && jump.IsPressed() && !isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply jump force
            jumpTime += Time.deltaTime;
        }

        if (jumpTime > buttonTime)
        {
            jumpCancelled = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); // Reduce jump height
        }
    }
}

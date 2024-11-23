using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    public PlayerInputActions playerControls;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction jump;

    public float gravityScale = 10;
    public float fallingGravityScale = 40;
    public float buttonTime = 0.3f;
    [SerializeField] float jumpTime = 0;
    [SerializeField] bool jumpInputPressed;
    [SerializeField] bool isGrounded;
    public float jumpAmount = 1.25f;

    [SerializeField] float jumpForce;

    [SerializeField] bool jumpCancelled;
    [SerializeField] bool jumpPressed;
    [SerializeField] bool jumping = false;
    [SerializeField] private Transform groundCollisionCheck;

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
        jumpForce = Mathf.Sqrt(jumpAmount * -2 * (Physics2D.gravity.y * rb.gravityScale));
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
        // Only allow jumping when grounded and not already jumping
        if (isGrounded && !jumping)
        {
            jumping = true;
            jumpTime = 0; // Reset jump time
            Debug.Log("Jump performed");
        }
        else if (!jumping && !isGrounded)
        {
            jumping = true;
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (!isGrounded && rb.linearVelocity.y > 0)
        {
            jumpCancelled = true; // Cancel the jump if still going up
            Debug.Log("Jump canceled");
        }
    }

    public void enteredGround()
    {
        isGrounded = true;
        jumping = false;
        jumpCancelled = false;
    }

    public void exitedGround()
    {
        isGrounded = false;
    }

    void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y); // Only modify X velocity

        // Jumping logic
        if (jumping && !jumpCancelled && jumpTime < buttonTime && jump.IsPressed() && !isGrounded)
        {
            Debug.Log("Jumping function activated");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply jump force
            jumpTime += Time.deltaTime;
            rb.gravityScale = gravityScale; // Ensure normal gravity while jumping
        }
        else if (isGrounded)
        {
            Debug.Log("isGrounded is true");
            jumpTime = 5;
            rb.gravityScale = gravityScale; // Reset gravity to normal
        }
        else if (!jumping && !isGrounded)
        {
            Debug.Log("You're not jumping or grounded");
            rb.gravityScale = fallingGravityScale; // Apply falling gravity
        }

        if (rb.linearVelocity.y > 0 && jumping)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.linearVelocity.y <= 0)
        {
            rb.gravityScale = fallingGravityScale; // Apply different gravity for falling
        }

        if (jumping && (jumpCancelled || jumpTime > buttonTime))
        {
            // Apply additional downward force to cancel the jump or fall faster
            rb.AddForce(Vector2.down * 100);
            rb.gravityScale = fallingGravityScale;
        }
    }
}

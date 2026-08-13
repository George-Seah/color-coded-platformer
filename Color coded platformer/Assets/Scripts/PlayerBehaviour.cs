using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 12f;
    
    [SerializeField] bool isGrounded, holdingKey, landedOnWaddler;
    public bool jumpPressed { get; private set; }
    
    // Coyote Time & Jump Buffering
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    [SerializeField] public Key currentKey { get; set; }
    [SerializeField] GameObject canvasPrefab;
    
    public PlayerInputActions playerControls;
    public readonly float gravityScale = 2f, fallingGravityScale = 5f;
    public string color;
    public bool onLadder, lookingRight;
    
    public Vector2 moveDirection { get; private set; }
    public InputAction move { get; private set; }
    InputAction jump, drop, interact, pause; 
    Collider2D[] playerColliders;
    
    readonly float waddlerBounceHeld = 1.25f, waddlerBounceUnheld = 0.8f;
    static readonly string[] colorLayers = { "Red", "Blue", "Green" };
    public event System.Action InteractPressed;
    void Awake()
    {
        playerControls = new PlayerInputActions();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerColliders = GetComponentsInChildren<Collider2D>();
        if (CanvasScript.Instance == null) Instantiate(canvasPrefab);
    }

    void OnEnable()
    {
        playerControls.Player.Enable();
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        drop = playerControls.Player.Drop;
        interact = playerControls.Player.Interact;
        pause = playerControls.Player.Pause;

        move.Enable(); jump.Enable(); drop.Enable(); interact.Enable(); pause.Enable();

        pause.performed += OnPause;
        jump.performed += OnJump;
        jump.canceled += OnJumpCanceled;
        drop.performed += OnDrop;
        interact.performed += OnInteract;
    }

    void OnDisable()
    {
        move.Disable(); jump.Disable(); drop.Disable(); interact.Disable(); pause.Disable();
        playerControls.Player.Disable();
        
        pause.performed -= OnPause;
        jump.performed -= OnJump;
        jump.canceled -= OnJumpCanceled;
        drop.performed -= OnDrop;
        interact.performed -= OnInteract;
    }

    void OnPause(InputAction.CallbackContext ctx)
    {
        if (CanvasScript.Instance != null) CanvasScript.Instance.PauseGame();
    }

    void OnJump(InputAction.CallbackContext ctx) 
    {
        jumpPressed = true;
        jumpBufferCounter = jumpBufferTime;
    }

    void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        jumpPressed = false;
        
        // Cut jump height dynamically
        if (rb.linearVelocityY > 0 && !onLadder)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        coyoteTimeCounter = 0;
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (currentKey != null && !currentKey.ReadPickedUp() && !holdingKey)
        {
            currentKey.PickUp(transform);
            holdingKey = currentKey != null && currentKey.ReadPickedUp();
        }

        InteractPressed?.Invoke();
    }

    void OnDrop(InputAction.CallbackContext ctx)
    {
        if (currentKey == null) return;
        currentKey.Drop();
        currentKey = null;
        holdingKey = false;
    }

    public void EnteredGround()
    {
        if (isGrounded) return;
        isGrounded = true;
        rb.gravityScale = gravityScale;
    }

    public void StayingOnGround()
    {
        if (isGrounded) return;
        isGrounded = true;
        rb.gravityScale = gravityScale;
    }

    public void ExitedGround() => isGrounded = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving Platform"))
        {
            isGrounded = true;
        }
        
        if (collision.gameObject.CompareTag("Key") && !holdingKey)
        {
            if (collision.gameObject.TryGetComponent<Key>(out var key))
            {
                if (!key.ReadPickedUp()) currentKey = key;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving Platform") && !jumpPressed)
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!holdingKey && collision.gameObject.TryGetComponent<Key>(out var key))
        {
            if (key == currentKey && !key.ReadPickedUp()) currentKey = null;
        }
        if (collision.gameObject.CompareTag("Moving Platform")) isGrounded = false;
    }

    public void ColorChanged(Color newColor)
    {
        // 1. Change Player Layer
        string targetLayer = newColor == Color.red ? "Red" : 
                             newColor == Color.blue ? "Blue" : 
                             newColor == Color.green ? "Green" : "White";

        int layerID = LayerMask.NameToLayer(targetLayer);
        if (layerID != -1)
        {
            gameObject.layer = layerID;
            
            // 2. Change Ground Check Layer so it perfectly inherits the Collision Matrix rules!
            Transform groundCheck = transform.Find("GroundCollisionCheck");
            if (groundCheck != null)
            {
                groundCheck.gameObject.layer = layerID;
            }
        }

        // Update tint
        GetComponent<SpriteRenderer>().color = newColor;

        if (currentKey != null && color != LayerMask.LayerToName(currentKey.gameObject.layer) 
            && currentKey.ReadPickedUp() && currentKey.GetComponent<SpriteRenderer>().color != Color.white)
        {
            currentKey.Drop();
            currentKey = null;
        }
    }

    void FixedUpdate()
    {
        // Movement
        moveDirection = move.ReadValue<Vector2>();
        
        // Ladder Override (Kinematic State)
        if (onLadder)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
            rb.gravityScale = 0;
            
            // Ignore semi-solids while climbing
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SemiSolid"), true);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SemiSolid"), false);
        }

        // Coyote Time Update
        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.fixedDeltaTime;

        // Jump Buffer Update
        if (!jumpPressed) jumpBufferCounter -= Time.fixedDeltaTime;

        // Execute Jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !onLadder)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            landedOnWaddler = false;
        }

        // Waddler bounce initiation
        if (jumpBufferCounter > 0f && landedOnWaddler && !onLadder)
        {
            float multiplier = jumpPressed ? waddlerBounceHeld : waddlerBounceUnheld;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * multiplier);
            jumpBufferCounter = 0f;
            landedOnWaddler = false;
        }

        // Dynamic Gravity
        if (!onLadder)
        {
            if (rb.linearVelocityY < 0) rb.gravityScale = fallingGravityScale;
            else rb.gravityScale = gravityScale;
        }

        // Animation & Facing
        anim.SetFloat("XMovement", moveDirection.x);
        if (moveDirection.x != 0) lookingRight = moveDirection.x > 0;
        transform.rotation = Quaternion.Euler(0, lookingRight ? 0 : 180, 0);

        // Key management
        if (currentKey != null && currentKey.ReadPickedUp() && currentKey.transform.parent != null && currentKey.transform.parent.CompareTag("Waddler"))
            currentKey = null;

        // Grounded state animation
        anim.SetBool("grounded", isGrounded);
        if (isGrounded) landedOnWaddler = false;
    }

    public void SetIgnoreMovementCollisions(bool ignore)
    {
        foreach (var tag in new[] { "OneWayPlatform", "Cloud" })
            foreach (var obj in GameObject.FindGameObjectsWithTag(tag))
                SetCollision(obj, ignore);
    }

    public void SetIgnoreOneWayPlatforms(bool ignore)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("OneWayPlatform"))
            SetCollision(obj, ignore);
    }

    void SetCollision(GameObject obj, bool ignore)
    {
        var col = obj.GetComponent<Collider2D>();
        if (col == null) return;
        foreach (var pc in playerColliders)
            Physics2D.IgnoreCollision(pc, col, ignore);
    }

    public bool readJumping() => jumpPressed && jumpTime > 0;
    public bool readGrounded() => isGrounded;
    
    public void SetKey(Key key)
    {
        currentKey = key;
        holdingKey = key != null;
    }

    public void LandedOnWaddler()
    {
        landedOnWaddler = true;
        jumpTime = initialJumpTime;
    }

    public void RespawnPositionAtPosition(Vector2 respawnPosition) => 
        transform.position = respawnPosition;
}
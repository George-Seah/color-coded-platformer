using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float initialJumpForce = 5f;
    [SerializeField] float initialJumpTime = 0.3f;
    [SerializeField] float jumpTime;
    float jumpForce;
    
    [SerializeField] bool isGrounded, holdingKey, landedOnWaddler;
    public bool jumpPressed { get; private set; }
    bool jumpConsumed;
    [SerializeField] public Key currentKey { get; set; }
    [SerializeField] GameObject canvasPrefab;
    
    public PlayerInputActions playerControls;
    public readonly float gravityScale = 7f, fallingGravityScale = 9f;
    public string color;
    public bool onLadder, lookingRight;
    
    Vector2 moveDirection;
    InputAction move, jump, drop, interact, pause;
    Collider2D[] playerColliders;
    
    readonly float waddlerBounceHeld = 1.25f, waddlerBounceUnheld = 0.8f;
    static readonly string[] colorLayers = { "Red", "Blue", "Green" };

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

        pause.performed += ctx => CanvasScript.Instance.PauseGame();
        jump.performed += OnJump;
        jump.canceled += OnJumpCanceled;
        drop.performed += OnDrop;
        interact.performed += OnInteract;
    }

    void OnDisable()
    {
        move.Disable(); jump.Disable(); drop.Disable(); interact.Disable(); pause.Disable();
        playerControls.Player.Disable();
        
        pause.performed -= ctx => CanvasScript.Instance.PauseGame();
        jump.performed -= OnJump;
        jump.canceled -= OnJumpCanceled;
        drop.performed -= OnDrop;
        interact.performed -= OnInteract;
    }

    void OnJump(InputAction.CallbackContext ctx) => jumpPressed = true;

    void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        jumpPressed = false;
        jumpConsumed = false;
        jumpForce = 0;
        jumpTime = 0;
        
        // Only apply fall physics if actually in the air
        if (!isGrounded)
        {
            rb.gravityScale = fallingGravityScale;
            if (rb.linearVelocityY > 0) rb.linearVelocityY = 0;
        }
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (currentKey != null && !currentKey.ReadPickedUp() && !holdingKey)
        {
            currentKey.PickUp(transform);
            holdingKey = currentKey != null && currentKey.ReadPickedUp();
        }
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
            var key = collision.gameObject.GetComponent<Key>();
            if (key != null && !key.ReadPickedUp()) currentKey = key;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving Platform") && !jumpPressed)
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Key>() == currentKey && !holdingKey)
        {
            var key = collision.gameObject.GetComponent<Key>();
            if (currentKey != null && !key.ReadPickedUp()) currentKey = null;
        }
        if (collision.gameObject.CompareTag("Moving Platform")) isGrounded = false;
    }

    public void ColorChanged(Color newColor)
    {
        foreach (var layer in colorLayers)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(layer), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer(layer), true);
        }

        string targetLayer = newColor == Color.red ? "Red" : newColor == Color.blue ? "Blue" : 
                             newColor == Color.green ? "Green" : null;

        if (targetLayer != null)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(targetLayer), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer(targetLayer), false);
            color = targetLayer;
        }
        else if (newColor == Color.white)
        {
            foreach (var layer in colorLayers)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(layer), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer(layer), false);
            }
            color = "White";
        }

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
        if (!onLadder) rb.linearVelocityX = moveDirection.x * moveSpeed;
        
        // Animation & Facing
        anim.SetFloat("XMovement", moveDirection.x);
        if (moveDirection.x != 0) lookingRight = moveDirection.x > 0;
        transform.rotation = Quaternion.Euler(0, lookingRight ? 0 : 180, 0);

        // Key management
        if (currentKey != null && currentKey.ReadPickedUp() && currentKey.transform.parent.CompareTag("Waddler"))
            currentKey = null;

        // Grounded state
        anim.SetBool("grounded", isGrounded);
        if (isGrounded) landedOnWaddler = false;

        // Jump initiation
        if (jumpPressed && isGrounded && !jumpConsumed)
        {
            jumpForce = initialJumpForce;
            jumpTime = initialJumpTime;
            jumpConsumed = true;
        }

        // Waddler bounce initiation
        if (jumpPressed && landedOnWaddler && !jumpConsumed)
        {
            float multiplier = jump.IsPressed() ? waddlerBounceHeld : waddlerBounceUnheld;
            jumpForce = initialJumpForce * multiplier;
            jumpTime = initialJumpTime;
            landedOnWaddler = false;
            jumpConsumed = true;
        }

        // Sustained jump (decaying force)
        if (jumpPressed && jumpTime > 0)
        {
            jumpTime -= Time.fixedDeltaTime;
            jumpForce *= 0.9f;
            rb.linearVelocityY += jumpForce;
        }
        else if (!isGrounded && (jumpTime <= 0 || rb.linearVelocityY <= 0))
        {
            rb.gravityScale = fallingGravityScale;
        }

        // Ladder override
        if (onLadder) rb.gravityScale = 0;
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
        transform.position = GameObject.Find("Game Manager").GetComponent<GameManager>().playerSpawnPoint;
}
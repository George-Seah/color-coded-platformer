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
    
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    public Key currentKey { get; set; }
    [SerializeField] GameObject canvasPrefab;
    
    public PlayerInputActions playerControls;
    public readonly float gravityScale = 2f, fallingGravityScale = 5f;
    public string color;
    public bool onLadder, lookingRight;
    
    public Vector2 moveDirection { get; private set; }
    public InputAction move { get; private set; }
    InputAction jump, drop, interact, pause; 
    Collider2D[] playerColliders;
    Collider2D mainCollider;
    
    readonly float waddlerBounceHeld = 1.25f, waddlerBounceUnheld = 0.8f;
    public event System.Action InteractPressed;
    
    void Awake()
    {
        playerControls = new PlayerInputActions();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerColliders = GetComponentsInChildren<Collider2D>();
        mainCollider = GetComponent<Collider2D>();
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

    public void EnteredGround() { }
    public void StayingOnGround() { }
    public void ExitedGround() { }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Key") && !holdingKey)
        {
            if (collision.gameObject.TryGetComponent<Key>(out var key))
            {
                if (!key.ReadPickedUp()) currentKey = key;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) { }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!holdingKey && collision.gameObject.TryGetComponent<Key>(out var key))
        {
            if (key == currentKey && !key.ReadPickedUp()) currentKey = null;
        }
    }

    public void ColorChanged(GameColor newColor)
    {
        string targetLayer = newColor == GameColor.Default ? "Default" : newColor.ToString();

        int layerID = LayerMask.NameToLayer(targetLayer);
        if (layerID != -1)
        {
            gameObject.layer = layerID;
        }

        // SWAP TO DICTIONARY MATERIAL
        if (ColorDictionary.Instance != null)
        {
            Material mat = ColorDictionary.Instance.GetMaterial(newColor);
            if (mat != null) 
            {
                GetComponent<SpriteRenderer>().material = mat;
            }
            else 
            {
                GetComponent<SpriteRenderer>().color = ColorDictionary.Instance.GetColorTint(newColor);
            }
        }

        if (currentKey != null && gameObject.layer != currentKey.gameObject.layer 
            && currentKey.ReadPickedUp())
        {
            currentKey.Drop();
            currentKey = null;
        }
    }

    void FixedUpdate()
    {
        CheckGrounded();
        
        moveDirection = move.ReadValue<Vector2>();
        
        if (onLadder)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
            rb.gravityScale = 0;
            
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SemiSolid"), true);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SemiSolid"), false);
        }

        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.fixedDeltaTime;

        if (!jumpPressed) jumpBufferCounter -= Time.fixedDeltaTime;

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !onLadder)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            landedOnWaddler = false;
        }

        if (jumpBufferCounter > 0f && landedOnWaddler && !onLadder)
        {
            float multiplier = jumpPressed ? waddlerBounceHeld : waddlerBounceUnheld;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * multiplier);
            jumpBufferCounter = 0f;
            landedOnWaddler = false;
        }

        if (!onLadder)
        {
            if (rb.linearVelocityY < 0) rb.gravityScale = fallingGravityScale;
            else rb.gravityScale = gravityScale;
        }

        anim.SetFloat("XMovement", moveDirection.x);
        if (moveDirection.x != 0) lookingRight = moveDirection.x > 0;
        transform.rotation = Quaternion.Euler(0, lookingRight ? 0 : 180, 0);

        if (currentKey != null && currentKey.ReadPickedUp() && currentKey.transform.parent != null && currentKey.transform.parent.CompareTag("Waddler"))
            currentKey = null;

        anim.SetBool("grounded", isGrounded);
        if (isGrounded) landedOnWaddler = false;
    }

    void CheckGrounded()
    {
        if (rb.linearVelocityY > 0.01f || onLadder) 
        {
            isGrounded = false;
            return;
        }

        Vector2 boxCenter = (Vector2)mainCollider.bounds.center + Vector2.down * (mainCollider.bounds.extents.y + 0.05f);
        Vector2 boxSize = new Vector2(mainCollider.bounds.size.x * 0.9f, 0.1f);

        int layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, layerMask);
        
        bool foundGround = false;
        foreach (var hit in hits)
        {
            if (hit != mainCollider && !hit.isTrigger) 
            {
                foundGround = true;
                
                if (hit.CompareTag("Waddler"))
                {
                    LandedOnWaddler();
                    if (hit.GetComponentInChildren<Key>() != null)
                    {
                        hit.GetComponentInChildren<Key>().Drop();
                    }
                    Destroy(hit.gameObject);
                }
                break;
            }
        }
        isGrounded = foundGround;
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

    public bool readJumping() => jumpPressed && jumpBufferCounter > 0;
    public bool readGrounded() => isGrounded;
    
    public void SetKey(Key key)
    {
        currentKey = key;
        holdingKey = key != null;
    }

    public void LandedOnWaddler()
    {
        landedOnWaddler = true;
        jumpBufferCounter = jumpBufferTime;
    }

    public void RespawnPositionAtPosition(Vector2 respawnPosition) => 
        transform.position = respawnPosition;
}

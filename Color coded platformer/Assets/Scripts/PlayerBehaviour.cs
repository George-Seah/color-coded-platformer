using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    public PlayerInputActions playerControls;
    private Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction jump;
    private InputAction drop;

    public float gravityScale = 10f;
    public float fallingGravityScale = 40f;
    public float buttonTime = 0.3f;
    [SerializeField] private float jumpTime = 0f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpAmount = 1.25f;
    [SerializeField] private float jumpForce;

    private bool jumpCancelled;
    [SerializeField] private bool jumping = false;
    [SerializeField] private float moveDirectionX;
    [SerializeField] private float moveDirectionY;
    [SerializeField] private Key currentKey;
    [SerializeField] private bool holdingKey;
    public string color;
    public bool onLadder;
    bool lookingRight;
    [SerializeField] private bool landedOnWaddler;

    float waddlerBounceForceButtonHeld = 1.25f;
    float waddlerBounceForceButtonUnheld = 0.8f;
    //[SerializeField] Vector2 respawnPosition;
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
        jumpForce = Mathf.Sqrt(jumpAmount * -2 * Physics2D.gravity.y);
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        drop = playerControls.Player.Drop;

        move.Enable();
        jump.Enable();
        drop.Enable();

        jump.performed += ctx => OnJump(ctx);
        jump.canceled += ctx => OnJumpCanceled(ctx);
        drop.performed += ctx => OnDrop(ctx);
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        drop.Disable();

        jump.performed -= OnJump;
        jump.canceled -= OnJumpCanceled;
        drop.performed -= OnDrop;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (currentKey != null && !currentKey.ReadPickedUp() && !holdingKey)
        {
            // Attempt to pick up the key
            currentKey.PickUp(transform);
            if(currentKey != null && currentKey.ReadPickedUp()){
                holdingKey = true;
            }
        }
        /*
        else if(currentKey != null && !currentKey.ReadPickedUp()){
            currentKey.PickUp(transform);
        }
        */
        else if (isGrounded && !jumping)
        {
            // Normal jump behavior
            jumping = true;
            jumpTime = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            rb.gravityScale = gravityScale;
            Debug.Log("OnJump function activating gravity.");
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (!isGrounded && rb.linearVelocity.y > 0)
        {
            jumpCancelled = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        Debug.Log("Drop button pressed.");
        // && currentKey.ReadPickedUp()
        if (currentKey != null)
        {
            currentKey.Drop();
            currentKey = null;
            holdingKey = false;
            Debug.Log("Key dropped");
        }
    }

    public void EnteredGround()
    {
        if (!isGrounded)
        {
            isGrounded = true;
            jumping = false;
            jumpCancelled = false;
            rb.gravityScale = gravityScale;
            Debug.Log("EnteredGround function activating gravity.");
        }
    }

    public void ExitedGround()
    {
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.DrawLine(contact.point, contact.point + contact.normal, Color.red, 2f);
            Debug.Log($"Normal: {contact.normal}");

            Vector2 collisionNormal = contact.normal;
            float collisionAngle = Vector2.Angle(Vector2.up, collisionNormal);

            Debug.Log($"Collision Angle: {collisionAngle}°");
        }

        if (collision.gameObject.CompareTag("Key") && !holdingKey)
        {
            Key key = collision.gameObject.GetComponent<Key>();
            Debug.Log("Key detected.");
            if (key != null && !key.ReadPickedUp())
            {
                currentKey = key;
                Debug.Log("Key detected. Ready to pick up.");
            }
            // && key.ReadPickedUp()
            /*
            else if(key != null && collision.transform.parent.gameObject.CompareTag("Waddler")){
                currentKey = key;
                Debug.Log("Key detected being on Waddler. Ready to pick up?");
            }
            */
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Key") && !holdingKey)
        {
            Key key = collision.gameObject.GetComponent<Key>();
            if(currentKey != null && !key.ReadPickedUp()){
                currentKey = null;
            }
            
            Debug.Log("Key out of range.");
        }
    }


    public void ColorChanged(Color newColor)
    {
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Red"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Blue"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Green"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"));

        if (newColor == Color.red)
        {
            Debug.Log("Red selected");
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Red"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), false);
            color = "Red";
        }
        else if (newColor == Color.blue)
        {
            Debug.Log("Blue selected");
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Blue"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), false);
            color = "Blue";
        }
        else if (newColor == Color.green)
        {
            Debug.Log("Green selected");
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Green"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), false);
            color = "Green";
        }
        else if (newColor == Color.white)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Red"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Blue"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Green"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), false);
            color = "White";
        }

        GetComponent<SpriteRenderer>().color = newColor;
        
        //Drop key if not the same layer.
        if(currentKey != null && color != $"{LayerMask.LayerToName(currentKey.gameObject.layer)}" 
        && currentKey.ReadPickedUp() && currentKey.gameObject.GetComponent<SpriteRenderer>().color != Color.white){
            Debug.Log($"gameObject.layer = {gameObject.layer} and currentKey.gameObject.layer {currentKey.gameObject.layer}.");
            Debug.Log($"color = {color} and currentKey.gameObject.layer {LayerMask.LayerToName(currentKey.gameObject.layer)}.");
            currentKey.Drop();
            currentKey = null;
            Debug.Log("Key dropped");
        }
    }

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        moveDirectionX = moveDirection.x;
        moveDirectionY = moveDirection.y;
        if(moveDirection.x > 0){
           lookingRight = true;
        }
        else if(moveDirection.x < 0){
           lookingRight = false;
        }
        /*
        if(currentKey != null && currentKey.ReadPickedUp() && !currentKey.transform.parent.gameObject.CompareTag("Waddler")){
            //currentKey.SetDirection(lookingRight);///Check this if direction isn;t right or key somehow teleports
        }
        */
        if(currentKey != null && currentKey.ReadPickedUp() && currentKey.transform.parent.gameObject.CompareTag("Player")){
            currentKey.SetDirection(lookingRight);///Check this if direction isn;t right or key somehow teleports
        }
        if(currentKey == null){
            holdingKey = false;
        }
        if (isGrounded)
        {
            rb.gravityScale = gravityScale;
            landedOnWaddler = false;
            //Debug.Log("isGrounded bool in FixedUpdate activating gravity.");
            if (jumpTime != 0)
            {
                jumpTime = 0;
            }
        }
        else if (!jumping && !isGrounded && !onLadder)
        {
            rb.gravityScale = fallingGravityScale;
            Debug.Log("!jumping && !isGrounded bools in FixedUpdate activating gravity.");
        }

        if (jumping && !jumpCancelled && jumpTime < buttonTime && jump.IsPressed())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTime += Time.deltaTime;
        }
        if(jumping && landedOnWaddler && jumpTime < buttonTime){
            Debug.Log("Player should bounce off Waddler.");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * waddlerBounceForceButtonUnheld);
            jumpTime += Time.deltaTime;
            rb.gravityScale = gravityScale;
            if(jump.IsPressed()){
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * waddlerBounceForceButtonHeld);
            }
            // && jumpTime < buttonTime && jump.IsPressed()
            if(jump.WasReleasedThisFrame()){
                rb.gravityScale = fallingGravityScale;
            }
        }
        if (jumpTime > buttonTime)
        {
            jumpCancelled = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            rb.gravityScale = fallingGravityScale;
            Debug.Log("jumpTime > buttonTime bool in FixedUpdate activating gravity.");
        }
    }
    public bool readJumping(){
        return jumping;
    }
    public void SetKey(Key key){
        currentKey = key;
    }
    public void LandedOnWaddler(){
        landedOnWaddler = true;
        jumpTime = 0;
    }
    public void RespawnPositionAtPosition(Vector2 respawnPosition){
        gameObject.transform.position = GameObject.Find("Game Manager").GetComponent<GameManager>().playerSpawnPoint;
    }

}

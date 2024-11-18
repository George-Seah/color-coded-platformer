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
    private void Awake(){
        playerControls = new PlayerInputActions();
        if (playerControls == null) {
            Debug.LogError("PlayerInputActions not initialized!");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        groundCollisionCheck = transform.Find("Player/GroundCollisionCheck");
        jumpForce = Mathf.Sqrt(jumpAmount * -2 * (Physics2D.gravity.y * rb.gravityScale));
    }
    private void OnEnable(){
        playerControls.Player.Enable(); // Enable the Player action map
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;

        move.Enable();
        jump.Enable();

        jump.performed += ctx => OnJumpPerformed(ctx);
        jump.canceled += ctx => OnJumpCanceled(ctx);
        Debug.Log("Jump action enabled"); // Add this to verify the action is enabled
    }

    private void OnDisable(){
        move.Disable();
        jump.Disable();

        jump.started -= OnJumpStarted;
        jump.performed -= OnJumpPerformed;
        jump.canceled -= OnJumpCanceled;
    }
    private void OnJumpStarted(InputAction.CallbackContext context) {
        Debug.Log("Jump started");
    }

    private void OnJumpPerformed(InputAction.CallbackContext context) {
        if (isGrounded && !jumping)
        {
            isGrounded = false;
            jumping = true; // Start the jump
            //jumpTime = 0;   // Reset jump time
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpAmount); // Apply upward force
            jumpTime += Time.deltaTime;
            Debug.Log("Jump performed");
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext context) {
    //rb.linearVelocity.y > 0
    if (!isGrounded)
    {
        jumpCancelled = true; // Cancel the jump if it's still going up
        Debug.Log("Jump canceled");
    }
    }
    private void Jump(InputAction.CallbackContext context) {
        Debug.Log("Jump button pressed!");
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            isGrounded = true;
            jumping = false;
            
            Debug.Log("You're grounded");
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            isGrounded = false;
        }
    }
    
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            //isGrounded = true; // Ensure grounded state during the collision
            jumpCancelled = false;
        }
    }
    
    /*
    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (rb.linearVelocity.y > 0) // Cancel upward movement if the button is released while jumping
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); // Reduce upward velocity
            jumpCancelled = true;
        }
        Debug.Log("Jump button released");
    }*/
    // Update is called once per frame

    //void Update(){}
    void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        //jumpInputPressed = jump.ReadValue<bool>();

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, transform.position.y);
        if(isGrounded && jumpInputPressed && !jumping){
            Debug.Log($"jumpInputPressed = {jumpInputPressed}");
            jumping = true; // Start the jump
            jumpTime = 0;   // Reset jump time
        }
        if(jumpCancelled || jumpTime > buttonTime){
            jumping = false;
        }
        if(rb.linearVelocity.y >= 0){
            rb.gravityScale = gravityScale;
        }
        else if(rb.linearVelocity.y < 0){
            rb.gravityScale = fallingGravityScale;
            //anim.SetBool("isJumping", false);//Testing to see. If not working remove this
        }
        if(jumpCancelled && jumping && rb.linearVelocity.y > 0){
            //rb.AddForce(Vector2.down * 100);
        }
    }

    /*private void Jump(InputAction.CallbackContext context){
        Debug.Log("Jump button pressed");
        if(isGrounded){
            jumping = true;
            jumpTime = 0;
        }

    }*/

}



/*
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class PlayerBehaviour : PlayableBehaviour
{
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }

}
*/
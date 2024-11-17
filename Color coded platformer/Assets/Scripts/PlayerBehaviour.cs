using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    public PlayerInputActions playerControls;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction jump;
    private void Awake(){
        playerControls = new PlayerInputActions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable(){
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
    }

    private void OnDisable(){
        move.Disable();
        jump.Disable();
    }
    // Update is called once per frame
    void Update(){
        moveDirection = move.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, transform.position.y);
    }

    private void Jump(InputAction.CallbackContext context){
        Debug.Log("Jump");
    }
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
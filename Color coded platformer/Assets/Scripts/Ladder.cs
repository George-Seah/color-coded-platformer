using UnityEngine;
using UnityEngine.InputSystem;

public class Ladder : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 5f;
    public PlayerInputActions playerControls;
    private Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    [SerializeField] private float moveDirectionY;
    [SerializeField] bool playerOnLadder;
    [SerializeField] private GameObject playerTransform;
    private void Awake()
    {
        playerControls = new PlayerInputActions();
        if (playerControls == null)
        {
            Debug.LogError("PlayerInputActions not initialized!");
        }
    }
    private void OnEnable()
    {
        playerControls.Player.Enable();
        move = playerControls.Player.Move;

        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")
        && (collision.gameObject.GetComponent<PlayerBehaviour>().color == $"{LayerMask.LayerToName(gameObject.layer)}" || 
        $"{LayerMask.LayerToName(gameObject.layer)}" == "Default")){
            // 
            Debug.Log("Player entered Ladder Checker.");
            /*
            if(moveDirection.y == 0){
                Debug.Log("Player isn't pressing vertical inputs near ladder.");
                playerTransform = collision.gameObject.transform;
            }
            else if(moveDirection.y > 0){
                Debug.Log("Player is pressing up vertical inputs near ladder.");
            }
            else if(moveDirection.y < 0){
                Debug.Log("Player is pressing down vertical inputs near ladder.");
            }
            */
            if(moveDirection.y != 0){
                Debug.Log("Player isn't pressing vertical inputs near ladder.");
                playerTransform = collision.gameObject;
                playerOnLadder = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")
        && (collision.gameObject.GetComponent<PlayerBehaviour>().color == $"{LayerMask.LayerToName(gameObject.layer)}" || 
        $"{LayerMask.LayerToName(gameObject.layer)}" == "Default")){
            Debug.Log("Player exited Ladder Checker.");
            playerTransform = null;
            playerOnLadder = false;
            }
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        moveDirectionY = moveDirection.y;
        if(playerOnLadder){
            //moveDirection.x * climbSpeed, rb.linearVelocity.y
            //rb.linearVelocity = new Vector2();
            playerTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            playerTransform.transform.position = new Vector2(transform.position.x, playerTransform.transform.position.y + (moveDirection.y * climbSpeed));
            if(moveDirection.x != 0){
                playerTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
                playerOnLadder = false;
                playerTransform = null;
            }
        }

        

    }

}

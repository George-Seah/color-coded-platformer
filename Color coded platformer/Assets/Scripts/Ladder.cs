using UnityEngine;
using UnityEngine.InputSystem;

public class Ladder : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 0.25f;
    public PlayerInputActions playerControls;
    [SerializeField] private Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    [SerializeField] bool playerOnLadder;
    [SerializeField] private GameObject playerTransform;
    [SerializeField] private Rigidbody2D playerRB;
    //private bool playerIsNear = false;
    //[SerializeField] private PlayerBehaviour player;
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
        if (collision.gameObject.CompareTag("Player")
            && (collision.gameObject.GetComponent<PlayerBehaviour>().color == $"{LayerMask.LayerToName(gameObject.layer)}" ||
                $"{LayerMask.LayerToName(gameObject.layer)}" == "Default"))
        {
            moveDirection = move.ReadValue<Vector2>();
            playerTransform = collision.gameObject;
            playerRB = collision.gameObject.GetComponent<Rigidbody2D>();

            if (moveDirection.y != 0) // Player pressing down
            {
                // Temporarily disable collision with platforms
                /*
                Physics2D.IgnoreLayerCollision(
                    LayerMask.NameToLayer("Player"), 
                    LayerMask.NameToLayer("OneWayPlatform"), 
                    true);
                */
                playerOnLadder = true;
                collision.gameObject.GetComponent<PlayerBehaviour>().onLadder = true;
                playerRB.gravityScale = 0;
                
            }
            else if (moveDirection.y > 0 || moveDirection.y == 0) // Not pressing down
            {
                // Re-enable collision with platforms
                Physics2D.IgnoreLayerCollision(
                    LayerMask.NameToLayer("Player"), 
                    LayerMask.NameToLayer("OneWayPlatform"), 
                    false);
                if (moveDirection.y != 0) // Moving up
                {
                    playerRB.gravityScale = 0;
                    playerOnLadder = true;
                    collision.gameObject.GetComponent<PlayerBehaviour>().onLadder = true;
                    playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0f);
                }
            }

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")
        && (collision.gameObject.GetComponent<PlayerBehaviour>().color == $"{LayerMask.LayerToName(gameObject.layer)}" || 
        $"{LayerMask.LayerToName(gameObject.layer)}" == "Default")){
            Debug.Log("Player exited Ladder Checker.");
            collision.gameObject.GetComponent<PlayerBehaviour>().onLadder = false;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
            playerTransform = null;
            playerOnLadder = false;
            playerRB = null;
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
        if (playerOnLadder)
        {
            playerRB.gravityScale = 0;
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, moveDirection.y * climbSpeed); // Directly set climbing speed
            
            playerTransform.transform.position = new Vector2(
                transform.position.x, 
                playerTransform.transform.position.y + (moveDirection.y * climbSpeed)
            );
            // || playerTransform.gameObject.GetComponent<PlayerBehaviour>().readJumping()
            if (moveDirection.x != 0)
            {
                playerRB.gameObject.GetComponent<PlayerBehaviour>().onLadder = false;
                playerRB.gravityScale = 30;
                playerOnLadder = false;//Uncomment this if issues occur
                //This code should be last
                playerTransform = null;
            }
            else if(playerRB != null && playerRB.gameObject.layer != LayerMask.NameToLayer("Default") &&
            gameObject.layer != LayerMask.NameToLayer("Default") &&
            gameObject.layer != playerRB.gameObject.layer){
                playerRB.gameObject.GetComponent<PlayerBehaviour>().onLadder = false;
                playerRB.gravityScale = 30;
                playerOnLadder = false;//Uncomment this if issues occur
                //This code should be last
                playerTransform = null;
            }
            //playerTransform.gameObject.GetComponent<PlayerBehaviour>().onLadder = true;
        }
        /*
        if(playerTransform.gameObject.GetComponent<PlayerBehaviour>() != null && playerOnLadder){
            playerTransform.gameObject.GetComponent<PlayerBehaviour>().onLadder = true;
        }
        else if(playerTransform.gameObject.GetComponent<PlayerBehaviour>() != null && !playerOnLadder){
            playerTransform.gameObject.GetComponent<PlayerBehaviour>().onLadder = false;
        }
        */
        
        if(playerRB != null && playerRB.gravityScale > 0){
            //Debug.Log("Player is affected by gravity.");
        }


    }

}
/*
//Old ladder function

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
            Debug.Log("Player entered Ladder Checker.");
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
*/
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
            if (moveDirection.y != 0 &&
            (player.color == $"{LayerMask.LayerToName(gameObject.layer)}" ||
            $"{LayerMask.LayerToName(gameObject.layer)}" == "Default"))
            {

                Debug.Log("Player on ladder.");
                //player.OnLadder = true; // Enable ladder mode
                playerTransform = collision.gameObject; // Track the player
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
            if (moveDirection.y != 0 &&
            (player.color == $"{LayerMask.LayerToName(gameObject.layer)}" ||
            $"{LayerMask.LayerToName(gameObject.layer)}" == "Default"))
            {
                playerTransform = collision.gameObject; // Track the player
                //playerOnLadder = true;
                Debug.Log("Player on ladder.");
                playerTransform.transform.position = new Vector2(transform.position.x, playerTransform.transform.position.y);
                //player.OnLadder = true; // Enable ladder mode
                player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                
            }
        }
    }

private void OnTriggerExit2D(Collider2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            Debug.Log("Player exited ladder.");
            //player.OnLadder = false; // Disable ladder mode
            playerTransform = null; // Clear the reference
            player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
            //playerOnLadder = false;
        }
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
        //playerOnLadder && 
        if (playerTransform != null)
        {
            PlayerBehaviour player = playerTransform.GetComponent<PlayerBehaviour>();
            player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (player != null)
            {
                // Move the player vertically based on input
                playerTransform.transform.position += new Vector3(0, moveDirection.y * climbSpeed * Time.fixedDeltaTime, 0);


            }
            // Exit ladder if horizontal input or jumping
            if (moveDirection.x != 0 || player.readJumping())
            {
                Debug.Log("Player left ladder.");
                //player.OnLadder = false; // Disable ladder mode
                playerOnLadder = false;
                playerTransform = null;
            }
            if(player.gameObject.GetComponent<Rigidbody2D>().gravityScale > 0){
                Debug.Log("Player has gravity.");
            }
        }


        

    }

}

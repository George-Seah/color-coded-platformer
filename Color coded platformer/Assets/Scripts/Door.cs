using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private InputAction move;
    public string scene;
    public Vector2 newPlayerPosition;
    private bool playerWithinCollider = false;
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
        playerControls.Player.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        // Get the vertical axis input (Y direction)
        float verticalInput = move.ReadValue<Vector2>().y;
        // Check if the vertical input is positive and no other button press is being held
        if (verticalInput > 0 && playerWithinCollider)
        {
            EnterDoor();
        }
    }
    private void EnterDoor()
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        GameObject.Find("Game Manager").GetComponent<GameManager>().playerSpawnPoint = newPlayerPosition;
        //GameObject.Find("Game Manager").GetComponent<GameManager>().justReloaded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Hardcoded "Player" tag check
        {
            // Enable move action when the player enters the trigger
            move.Enable();
            Debug.Log("player entered door.");
            playerWithinCollider = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Hardcoded "Player" tag check
        {
            // Disable move action when the player exits the trigger
            move.Disable();
            Debug.Log("player exited door.");
            playerWithinCollider = false;
        }
    }
}

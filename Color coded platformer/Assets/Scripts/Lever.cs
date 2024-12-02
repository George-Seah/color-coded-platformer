using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Lever : MonoBehaviour
{
    [Header("Lever State")]
    [SerializeField] private bool activated = false;

    [Header("Lever Events")]
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    [Header("Interaction Settings")]
    [Tooltip("Allow toggling multiple times.")]
    public bool allowMultipleToggles = true;

    // My variables
    public PlayerInputActions playerControls;
    private InputAction move;
    private bool canToggle = true; // Ensures toggling happens only once per press

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

    private void FixedUpdate()
    {
        // Get the vertical axis input (Y direction)
        float verticalInput = move.ReadValue<Vector2>().y;

        // Check if the vertical input is positive and no other button press is being held
        if (verticalInput > 0 && canToggle)
        {
            ToggleLever();
            canToggle = false; // Prevent further toggles until input is released
        }
        else if (verticalInput <= 0)
        {
            canToggle = true; // Reset toggle when input is released
        }
    }

    private void ToggleLever()
    {
        if (!allowMultipleToggles && activated) return;

        activated = !activated;

        if (activated)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            onActivate?.Invoke();
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
            onDeactivate?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Hardcoded "Player" tag check
        {
            // Enable move action when the player enters the trigger
            move.Enable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Hardcoded "Player" tag check
        {
            // Disable move action when the player exits the trigger
            move.Disable();
        }
    }
}

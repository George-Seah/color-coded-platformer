using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    [Header("Scene Transition")]
    [SerializeField] private string scene;
    [SerializeField] private Vector2 newPlayerPosition;

    private bool playerInRange;
    private bool used;

    private PlayerInput playerInput;
    private InputAction interact;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        if (!other.TryGetComponent(out PlayerInput input)) return;

        playerInput = input;
        interact = playerInput.actions["Interact"];

        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerInput input)) return;
        if (input != playerInput) return;

        Cleanup();
    }

    private void Update()
    {
        if (!playerInRange || used || interact == null) return;

        // Fires only on a NEW press
        if (interact.WasPressedThisFrame())
        {
            used = true;
            BeginTransition();
        }
    }

    private void BeginTransition()
    {
        playerInRange = false;
        GetComponent<Collider2D>().enabled = false;

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.playerSpawnPoint = newPlayerPosition;
        }

        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    private void Cleanup()
    {
        playerInput = null;
        interact = null;
        playerInRange = false;
    }
}

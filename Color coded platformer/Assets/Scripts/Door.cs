using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    [Header("Scene Transition")]
    [SerializeField] string scene;
    [SerializeField] Vector2 newPlayerPosition;

    bool playerInRange;
    bool used;

    PlayerInput playerInput;
    InputAction interact;

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        if (!other.TryGetComponent(out PlayerInput input)) return;

        playerInput = input;
        interact = playerInput.actions["Interact"];

        playerInRange = true;
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (!playerInRange || used || interact == null) return;

        // Fires only on a NEW press
        if (interact.WasPressedThisFrame())
        {
            used = true;
            BeginTransition();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerInput input)) return;
        if (input != playerInput) return;

        Cleanup();
    }

    void BeginTransition()
    {
        playerInRange = false;
        GetComponent<Collider2D>().enabled = false;
        
        GameManager.Instance.playerSpawnPoint = newPlayerPosition;

        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void Cleanup()
    {
        playerInput = null;
        interact = null;
        playerInRange = false;
    }
}

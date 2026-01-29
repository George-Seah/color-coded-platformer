using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [Header("Lever State")]
    [SerializeField] bool activated = false;

    [Header("Lever Events")]
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    [Header("Interaction Settings")]
    public bool allowMultipleToggles = true;

    PlayerBehaviour player;
    bool subscribed;

    void OnDisable()
    {
        Unsubscribe();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        player = collision.GetComponent<PlayerBehaviour>();
        if (player == null) return;

        Subscribe();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Unsubscribe();
        player = null;
    }

    void Subscribe()
    {
        if (subscribed || player == null) return;
        player.InteractPressed += ToggleLever;
        subscribed = true;
    }

    void Unsubscribe()
    {
        if (!subscribed || player == null) return;
        player.InteractPressed -= ToggleLever;
        subscribed = false;
    }

    void ToggleLever()
    {
        if (!allowMultipleToggles && activated) return;

        activated = !activated;

        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.flipX = activated;

        if (activated) onActivate?.Invoke();
        else onDeactivate?.Invoke();
    }
}

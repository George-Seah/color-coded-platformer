using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [Tooltip("Speed of the platform movement.")]
    public float speed = 2f;

    [Tooltip("Auto highlight path in Scene View.")]
    public bool autoHighlightPath = false;

    [Tooltip("Target positions for platform movement.")]
    [SerializeField] private List<Vector3> multipleDirections = new List<Vector3>();

    [Tooltip("Whether the platform is active or not.")]
    public bool activated = false;

    [Tooltip("List of events to trigger at specific targets.")]
    public List<UnityEvent> targetEvents = new List<UnityEvent>();

    private List<Rigidbody2D> objectsOnPlatform = new List<Rigidbody2D>();

    private int currentDirectionIndex = 0;
    private Vector3 previousPosition;
    private Vector3 currentVelocity;

    private void Start()
    {
        if (multipleDirections.Count > 0)
        {
            currentDirectionIndex = 0; // Start from the first position
        }

        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!activated) return;

        // Calculate platform velocity
        currentVelocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = transform.position;

        HandleMultipleDirections();

        // Apply velocity to objects on platform
        foreach (var rb in objectsOnPlatform)
        {
            if (rb != null)
            {
                rb.linearVelocity += (Vector2)currentVelocity;
            }
        }
    }

    private void HandleMultipleDirections()
    {
        if (multipleDirections.Count == 0) return;

        // Move towards the current target
        Vector3 movement = (multipleDirections[currentDirectionIndex] - transform.position).normalized * Mathf.Abs(speed) * Time.fixedDeltaTime;
        transform.position += movement;

        // Check if the platform has reached the current target
        if (Vector3.Distance(transform.position, multipleDirections[currentDirectionIndex]) < 0.1f)
        {
            // Trigger event if available
            if (currentDirectionIndex < targetEvents.Count && targetEvents[currentDirectionIndex] != null)
            {
                targetEvents[currentDirectionIndex].Invoke();
            }

            // Move to the next target
            currentDirectionIndex++;
            if (currentDirectionIndex >= multipleDirections.Count)
            {
                currentDirectionIndex = 0; // Loop forward
            }
        }
    }

    public void Activate(bool activate)
    {
        activated = activate;
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void NegativeSpeed()
    {
        speed = -Mathf.Abs(speed); // Ensure speed is negative
    }

    public void ReverseDirections()
    {
        multipleDirections.Reverse();
    }

    private void OnDrawGizmos()
    {
        if (!autoHighlightPath || multipleDirections == null) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < multipleDirections.Count; i++)
        {
            Gizmos.DrawSphere(multipleDirections[i], 0.2f);

            if (i < multipleDirections.Count - 1)
            {
                Gizmos.DrawLine(multipleDirections[i], multipleDirections[i + 1]);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var rb = collision.collider.attachedRigidbody;
        if (rb != null && !objectsOnPlatform.Contains(rb))
        {
            objectsOnPlatform.Add(rb);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var rb = collision.collider.attachedRigidbody;
        if (rb != null && objectsOnPlatform.Contains(rb))
        {
            objectsOnPlatform.Remove(rb);
        }
    }
}

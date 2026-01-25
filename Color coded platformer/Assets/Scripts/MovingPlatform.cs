using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [Tooltip("Speed of the platform movement.")]
    public float speed = 2f;

    [Tooltip("Auto highlight path in Scene View.")]
    public bool autoHighlightPath = false;

    [Tooltip("Target positions for platform movement.")]
    [SerializeField] List<Vector3> multipleDirections = new List<Vector3>();

    [Tooltip("Whether the platform is active or not.")]
    public bool activated = false;

    [Tooltip("List of events to trigger at specific targets.")]
    public List<UnityEvent> targetEvents = new List<UnityEvent>();

    [SerializeField] List<GameObject> objectsOnPlatform = new List<GameObject>();

    [SerializeField] int currentDirectionIndex = 0;
    Rigidbody2D rb;
    [SerializeField] Vector3 direction;
    void Start()
    {
        if (multipleDirections.Count > 0)
        {
            currentDirectionIndex = 0; // Start from the first position
        }

        rb = GetComponent<Rigidbody2D>();
        //rb.isKinematic = true; // Ensure the platform doesn't move due to physics
    }

    void FixedUpdate()
    {
        if (!activated) return;

        HandleMultipleDirections();

        if (multipleDirections.Count > 0)
        {
            Vector3 targetPosition = multipleDirections[currentDirectionIndex];
            Vector3 toTarget = targetPosition - transform.position;

            // Stop moving if close enough to the target
            if (toTarget.magnitude < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                // Move the platform toward the target
                direction = toTarget.normalized;
                Vector3 velocity = direction * speed;

                // Clamp velocity to avoid overshooting the target
                if (velocity.magnitude > toTarget.magnitude / Time.fixedDeltaTime)
                {
                    velocity = toTarget / Time.fixedDeltaTime;
                }

                rb.linearVelocity = velocity;
            }
        }

        // Remove objects no longer on the platform
        objectsOnPlatform = objectsOnPlatform.Where(obj => obj != null && IsOnPlatform(obj)).ToList();

        // Apply platform velocity to objects on the platform
        foreach (GameObject obj in objectsOnPlatform)
        {
            if (obj != null)
            {
                Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
                if (objRb != null && obj.CompareTag("Player"))
                {
                    Vector2 additionalVelocity = rb.linearVelocity;
                    if (rb.linearVelocity.y >= 0)
                    {
                        additionalVelocity = new Vector2(rb.linearVelocity.x, 0);
                    }
                    objRb.linearVelocity += additionalVelocity;
                }
            }
        }
    }


    void HandleMultipleDirections()
    {
        if (multipleDirections.Count == 0) return;

        float distanceToTarget = Vector3.Distance(transform.position, multipleDirections[currentDirectionIndex]);

        // Check if the platform has reached the current target
        if (distanceToTarget < 0.1f)
        {
            // Trigger event if available
            if (currentDirectionIndex < targetEvents.Count && targetEvents[currentDirectionIndex] != null)
            {
                targetEvents[currentDirectionIndex].Invoke();
            }

            // Stop the platform if there's only one direction
            if (multipleDirections.Count == 1)
            {
                rb.linearVelocity = Vector2.zero;
                return;
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

    public void ReverseDirections()
    {
        multipleDirections.Reverse();
    }

    void OnDrawGizmos()
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 collisionNormal = contact.normal;
            float collisionAngle = Vector2.Angle(Vector2.up, collisionNormal);
            if (collisionAngle == 180 &&
                !objectsOnPlatform.Contains(collision.gameObject))
            {
                objectsOnPlatform.Add(collision.gameObject);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        objectsOnPlatform.Remove(collision.gameObject);
    }

    bool IsOnPlatform(GameObject obj)
    {
        // Check if the object is still physically touching the platform using collision bounds
        Collider2D objCollider = obj.GetComponent<Collider2D>();
        Collider2D platformCollider = GetComponent<Collider2D>();
        if (objCollider == null || platformCollider == null) return false;

        return objCollider.IsTouching(platformCollider);
    }
    public void changeToDynamic(){
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void changeToKinematic(){
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementMode { BackAndForth, OneDirection, MultipleDirections }

    [Header("Platform Settings")]
    public MovementMode movementMode = MovementMode.OneDirection;

    [Tooltip("Speed of the platform movement.")]
    public float speed = 2f;

    [Tooltip("Auto highlight path in Scene View.")]
    public bool autoHighlightPath = false;

    // For OneDirection and BackAndForth
    [Tooltip("Direction for OneDirection or BackAndForth.")]
    [SerializeField] private Vector2 direction = Vector2.right;

    [Tooltip("Distance for BackAndForth mode.")]
    [SerializeField] private float backAndForthDistance = 5f;

    // For MultipleDirections
    [Tooltip("Directions and distances for the MultipleDirections mode.")]
    [SerializeField] private List<DirectionData> multipleDirections = new List<DirectionData>();

    [Tooltip("Whether the platform is active or not.")]
    public bool activated = false;

    private Vector3 startPosition;
    private Vector3 currentTarget;
    private int currentDirectionIndex = 0;
    private bool movingForward = true;

    private void Start()
    {
        startPosition = transform.position;

        if (movementMode == MovementMode.MultipleDirections && multipleDirections.Count > 0)
        {
            SetNextTarget();
        }
    }

    private void FixedUpdate()
    {
        if (!activated) return;

        switch (movementMode)
        {
            case MovementMode.BackAndForth:
                HandleBackAndForth();
                break;
            case MovementMode.OneDirection:
                HandleOneDirection();
                break;
            case MovementMode.MultipleDirections:
                HandleMultipleDirections();
                break;
        }
    }

    private void HandleBackAndForth()
    {
        float distanceFromStart = Vector3.Distance(transform.position, startPosition);
        if (distanceFromStart >= backAndForthDistance)
            direction = -direction; // Reverse direction

        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }

    private void HandleOneDirection()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }

    private void HandleMultipleDirections()
    {
        if (multipleDirections.Count == 0) return;

        Vector3 movement = (currentTarget - transform.position).normalized * speed * Time.deltaTime;
        transform.position += movement;

        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            if (movingForward)
            {
                if (currentDirectionIndex < multipleDirections.Count - 1)
                    currentDirectionIndex++;
                else
                {
                    movingForward = false;
                    currentDirectionIndex--;
                }
            }
            else
            {
                if (currentDirectionIndex > 0)
                    currentDirectionIndex--;
                else
                {
                    movingForward = true;
                    currentDirectionIndex++;
                }
            }

            SetNextTarget();
        }
    }

    private void SetNextTarget()
    {
        Vector2 nextDirection = multipleDirections[currentDirectionIndex].direction.normalized;
        float distance = multipleDirections[currentDirectionIndex].distance;
        currentTarget = transform.position + (Vector3)(nextDirection * distance);
    }

    public void Activate()
    {
        activated = !activated;
    }

    private void OnValidate()
    {
        // Automatically hide irrelevant fields based on movement mode
        switch (movementMode)
        {
            case MovementMode.OneDirection:
                direction = direction; // Keep direction visible
                backAndForthDistance = 0f; // Hide BackAndForth settings
                multipleDirections.Clear(); // Hide MultipleDirections settings
                break;

            case MovementMode.BackAndForth:
                direction = direction; // Keep direction visible
                backAndForthDistance = Mathf.Max(backAndForthDistance, 1f); // Ensure distance > 0
                multipleDirections.Clear(); // Hide MultipleDirections settings
                break;

            case MovementMode.MultipleDirections:
                direction = Vector2.zero; // Hide direction
                backAndForthDistance = 0f; // Hide BackAndForth settings
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!autoHighlightPath) return;

        if (movementMode == MovementMode.BackAndForth)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startPosition, startPosition + (Vector3)(direction.normalized * backAndForthDistance));
            Gizmos.DrawLine(startPosition, startPosition - (Vector3)(direction.normalized * backAndForthDistance));
        }
        else if (movementMode == MovementMode.MultipleDirections && multipleDirections != null)
        {
            Gizmos.color = Color.green;
            Vector3 position = startPosition;

            foreach (var dirData in multipleDirections)
            {
                Vector3 nextPosition = position + (Vector3)(dirData.direction.normalized * dirData.distance);
                Gizmos.DrawLine(position, nextPosition);
                position = nextPosition;
            }
        }
    }

    [System.Serializable]
    public class DirectionData
    {
        public Vector2 direction;
        public float distance = 1f;
    }
}

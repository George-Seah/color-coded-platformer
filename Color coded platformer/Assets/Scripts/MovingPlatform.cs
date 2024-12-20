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

    [Tooltip("Target positions for MultipleDirections mode.")]
    [SerializeField] private List<Vector3> multipleDirections = new List<Vector3>();

    [Tooltip("Whether the platform is active or not.")]
    public bool activated = false;

    [SerializeField] private int currentDirectionIndex = 0;

    private void Start()
    {
        if (movementMode == MovementMode.MultipleDirections && multipleDirections.Count > 0)
        {
            currentDirectionIndex = 0; // Start from the first position
        }
    }

    private void FixedUpdate()
    {
        if (!activated) return;

        switch (movementMode)
        {
            case MovementMode.MultipleDirections:
                HandleMultipleDirections();
                break;
            // Handle other cases (BackAndForth, OneDirection) if necessary
        }
    }

    private void HandleMultipleDirections()
    {
        if (multipleDirections.Count == 0) return;

        // Move towards the current target
        Vector3 movement = (multipleDirections[currentDirectionIndex] - transform.position).normalized * Mathf.Abs(speed) * Time.deltaTime;
        transform.position += movement;

        // Check if the platform has reached the current target
        if (Vector3.Distance(transform.position, multipleDirections[currentDirectionIndex]) < 0.1f)
        {
            if (speed > 0)
            {
                // Move forward in the list
                currentDirectionIndex++;
                if (currentDirectionIndex >= multipleDirections.Count)
                {
                    currentDirectionIndex = 0; // Loop forward
                }
            }
            else
            {
                // Move backward in the list
                currentDirectionIndex--;
                if (currentDirectionIndex < 0)
                {
                    currentDirectionIndex = multipleDirections.Count - 1; // Loop backward
                }
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
        // Adjust the current index to maintain consistent behavior
        //currentDirectionIndex = multipleDirections.Count - 1 - currentDirectionIndex;
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
}

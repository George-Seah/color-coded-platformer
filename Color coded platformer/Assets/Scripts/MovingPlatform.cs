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
            SetNextTarget();
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

        Vector3 movement = (multipleDirections[currentDirectionIndex] - transform.position).normalized * speed * Time.deltaTime;
        transform.position += movement;

        if (Vector3.Distance(transform.position, multipleDirections[currentDirectionIndex]) < 0.1f)
        {
            // Advance or reverse the direction index based on speed's sign
            if (speed > 0)
            {
                currentDirectionIndex++;
                if (currentDirectionIndex >= multipleDirections.Count) currentDirectionIndex = 0; // Loop forward
            }
            else
            {
                currentDirectionIndex--;
                if (currentDirectionIndex < 0) currentDirectionIndex = multipleDirections.Count - 1; // Loop backward
            }
        }
    }

    private void SetNextTarget()
    {
        // Already handled dynamically in FixedUpdate
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
        speed = -speed;
    }

    public void ReverseDirections()
    {
        multipleDirections.Reverse();
        // Adjust the current index to maintain the same target behavior after reversal
        currentDirectionIndex = multipleDirections.Count - 1 - currentDirectionIndex;
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

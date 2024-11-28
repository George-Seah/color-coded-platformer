using UnityEngine;

public class DirectionallySolidPlatform : MonoBehaviour
{
    private Collider2D platformCollider;

    public enum DirectionOptions
    {
        Left,
        Right,
        Up,
        Down
    }
    
    [SerializeField] private DirectionOptions selectedDirection; // Expose this as a property

    public DirectionOptions SelectedDirection => selectedDirection; // Public getter for VelocityChecker

    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();

        // Optional: Rotate based on direction
        switch (selectedDirection)
        {
            case DirectionOptions.Left:
                transform.Rotate(0f, 0f, 90f, Space.Self);
                break;
            case DirectionOptions.Right:
                transform.Rotate(0f, 0f, 270f, Space.Self);
                break;
            case DirectionOptions.Up:
                transform.Rotate(0f, 0f, 0f, Space.Self);
                break;
            case DirectionOptions.Down:
                transform.Rotate(0f, 0f, 180f, Space.Self);
                break;
        }
    }
}

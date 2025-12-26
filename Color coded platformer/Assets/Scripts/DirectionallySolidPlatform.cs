using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
public class DirectionallySolidPlatform : MonoBehaviour
{
    public enum DirectionOptions
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private DirectionOptions selectedDirection;
    public DirectionOptions SelectedDirection
    {
        get { return selectedDirection; }
        set
        {
            selectedDirection = value;
            ApplyDirection();
        }
    }

    private PlatformEffector2D effector;

    private void Awake()
    {
        effector = GetComponent<PlatformEffector2D>();
        ApplyDirection();
    }

    private void OnValidate()
    {
        if (effector == null)
            effector = GetComponent<PlatformEffector2D>();

        ApplyDirection();
    }

    private void ApplyDirection()
    {
        effector.useOneWay = true;
        effector.surfaceArc = 180f;

        float zRotation = 0f;

        switch (selectedDirection)
        {
            case DirectionOptions.Up:
                zRotation = 0f;
                break;

            case DirectionOptions.Down:
                zRotation = 180f;
                break;

            case DirectionOptions.Left:
                zRotation = 90f;
                break;

            case DirectionOptions.Right:
                zRotation = -90f;
                break;
        }

        // Rotate the collider itself
        transform.rotation = Quaternion.Euler(0f, 0f, zRotation);

        // rotationalOffset intentionally unused
        effector.rotationalOffset = 0f;
    }
}

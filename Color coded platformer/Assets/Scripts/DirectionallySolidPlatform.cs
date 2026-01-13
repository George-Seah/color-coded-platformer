using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
[RequireComponent(typeof(Collider2D))]
public class DirectionallySolidPlatform : MonoBehaviour
{
    public enum DirectionOptions
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] DirectionOptions selectedDirection;
    public DirectionOptions SelectedDirection
    {
        get => selectedDirection;
        set
        {
            selectedDirection = value;
            ApplyDirection();
        }
    }

    PlatformEffector2D effector;
    Collider2D col;

    void Awake()
    {
        Initialize();
        ApplyDirection();
    }

    void OnValidate()
    {
        Initialize();
        ApplyDirection();
    }

    void Initialize()
    {
        if (!effector)
            effector = GetComponent<PlatformEffector2D>();

        if (!col)
            col = GetComponent<Collider2D>();

        col.usedByEffector = true;
    }

    void ApplyDirection()
    {
        effector.useOneWay = true;
        effector.surfaceArc = 180f;

        float rotation = 0f;

        switch (selectedDirection)
        {
            case DirectionOptions.Up:
                rotation = 0f;
                break;

            case DirectionOptions.Down:
                rotation = 180f;
                break;

            case DirectionOptions.Left:
                rotation = 90f;
                break;

            case DirectionOptions.Right:
                rotation = -90f;
                break;
        }

        // Physics object must NOT be rotated
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }
}

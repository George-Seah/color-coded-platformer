using UnityEngine;
using System.Collections.Generic; // Required for List

[RequireComponent(typeof(Collider2D))]
public class SlopeFixer : MonoBehaviour
{
    [Header("Slope Settings")]
    [Tooltip("Angles <= this are treated as flat ground")]
    [SerializeField] private float flatAngleThreshold = 5f;
    
    [Tooltip("Angles >= this are treated as steep walls (no slope fix)")]
    [SerializeField] private float steepAngleThreshold = 85f;

    private void Awake()
    {
        FixSlope();
    }

    [ContextMenu("Fix Slope Now")]
    public void FixSlope()
    {
        // Delete existing PolygonCollider2D if present
        PolygonCollider2D existingPoly = GetComponent<PolygonCollider2D>();
        if (existingPoly != null)
        {
            #if UNITY_EDITOR
            DestroyImmediate(existingPoly);
            #else
            Destroy(existingPoly);
            #endif
        }

        // Get the ORIGINAL collider (must be BoxCollider2D for your setup)
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        
        // We'll only handle BoxColliders (your player & ground use these)
        if (box == null && capsule == null)
        {
            Debug.LogWarning($"[SlopeFixer] {gameObject.name} has no Box/CapsuleCollider2D to convert.");
            return;
        }

        // Get collider data
        Vector2 size, offset;
        Collider2D oldCollider = null;

        if (box != null)
        {
            size = box.size;
            offset = box.offset;
            oldCollider = box;
        }
        else // Capsule (just in case)
        {
            size = capsule.size;
            offset = capsule.offset;
            oldCollider = capsule;
        }

        // Create new PolygonCollider
        PolygonCollider2D poly = gameObject.AddComponent<PolygonCollider2D>();
        
        // Copy physics settings
        poly.sharedMaterial = oldCollider.sharedMaterial;
        poly.isTrigger = oldCollider.isTrigger;
        poly.usedByEffector = oldCollider.usedByEffector;
        poly.enabled = oldCollider.enabled;

        // Handle PlatformEffector2D (for one-way platforms)
        PlatformEffector2D effector = GetComponent<PlatformEffector2D>();
        if (effector != null) poly.usedByEffector = true;

        // Generate slope points based on rotation
        float angle = transform.eulerAngles.z;
        if (angle > 180f) angle -= 360f; // Normalize to -180..180

        poly.points = GenerateSlopePoints(size, offset, angle);

        // Destroy original collider
        #if UNITY_EDITOR
        if (Application.isEditor && !Application.isPlaying)
            DestroyImmediate(oldCollider);
        else
            Destroy(oldCollider);
        #else
        Destroy(oldCollider);
        #endif

        Debug.Log($"[SlopeFixer] Fixed slope for {gameObject.name} (Angle: {angle:F1}°)");
    }

    private Vector2[] GenerateSlopePoints(Vector2 size, Vector2 offset, float angleDeg)
    {
        // If angle is too flat or too steep, use regular collider
        float absAngle = Mathf.Abs(angleDeg);
        if (absAngle < flatAngleThreshold || absAngle > (180f - flatAngleThreshold) || 
            absAngle > steepAngleThreshold && absAngle < (180f - steepAngleThreshold))
        {
            // Return regular rectangle points (no slope fix)
            Vector2 half = size * 0.5f;
            return new Vector2[4] {
                new Vector2(-half.x, -half.y) + offset,
                new Vector2( half.x, -half.y) + offset,
                new Vector2( half.x,  half.y) + offset,
                new Vector2(-half.x,  half.y) + offset
            };
        }

        // Calculate slope points for rotated BoxCollider
        List<Vector2> points = new List<Vector2>();
        Vector2 halfSize = size * 0.5f;

        // Get corners in local space (before rotation)
        Vector2 bl = new Vector2(-halfSize.x, -halfSize.y) + offset; // Bottom-left
        Vector2 br = new Vector2( halfSize.x, -halfSize.y) + offset; // Bottom-right
        Vector2 tl = new Vector2(-halfSize.x,  halfSize.y) + offset; // Top-left
        Vector2 tr = new Vector2( halfSize.x,  halfSize.y) + offset; // Top-right

        // Rotate all points to match the ground's actual orientation
        // This ensures the slope matches the visual rotation perfectly
        bl = RotatePoint(bl - offset, angleDeg) + offset;
        br = RotatePoint(br - offset, angleDeg) + offset;
        tl = RotatePoint(tl - offset, angleDeg) + offset;
        tr = RotatePoint(tr - offset, angleDeg) + offset;

        // Order points clockwise for PolygonCollider
        return new Vector2[4] { bl, br, tr, tl };
    }

    private Vector2 RotatePoint(Vector2 point, float angleDeg)
    {
        float angleRad = angleDeg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);

        return new Vector2(
            point.x * cos - point.y * sin,
            point.x * sin + point.y * cos
        );
    }

    #if UNITY_EDITOR
    private void Reset() => FixSlope();
    #endif
}
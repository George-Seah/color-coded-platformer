using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class Cloud : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction = Vector3.right;

    Rigidbody2D rb;

    [SerializeField] List<GameObject> objectsOnCloud = new List<GameObject>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure stable kinematic movement
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction.normalized * speed;

        objectsOnCloud = objectsOnCloud
            .Where(obj => obj != null && IsOnCloud(obj))
            .ToList();

        foreach (var obj in objectsOnCloud)
        {
            if (obj == null) continue;
            if (!obj.CompareTag("Player") && !obj.CompareTag("Waddler")) continue;

            Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
            if (objRb == null) continue;

            objRb.linearVelocity += (Vector2)direction.normalized * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground")) 
            Destroy(gameObject);
        if (!objectsOnCloud.Contains(collision.gameObject))
            objectsOnCloud.Add(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        objectsOnCloud.Remove(collision.gameObject);
    }

    bool IsOnCloud(GameObject obj)
    {
        Collider2D objCollider = obj.GetComponent<Collider2D>();
        Collider2D cloudCollider = GetComponent<Collider2D>();
        if (!objCollider || !cloudCollider) return false;
        return objCollider.IsTouching(cloudCollider);
    }

    public void SetDirection(Transform cannonTransform, float speedToSet, bool tiltCloud, float customRotation)
    {
        speed = speedToSet;

        float eulerAngle = cannonTransform.eulerAngles.z;
        direction = new Vector3(
            Mathf.Cos((eulerAngle + 90) * Mathf.Deg2Rad),
            Mathf.Sin((eulerAngle + 90) * Mathf.Deg2Rad),
            0f
        );

        transform.rotation = tiltCloud
            ? Quaternion.Euler(0, 0, customRotation)
            : Quaternion.identity;
    }
}

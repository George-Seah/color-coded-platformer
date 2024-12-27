using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Cloud : MonoBehaviour
{
    public float speed = 5f; // Speed of the cloud
    public Vector3 direction = Vector3.right; // Direction of movement
    private Rigidbody2D rb;
    [SerializeField] List<GameObject> objectsOnCloud = new List<GameObject>();
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.useGravity = false; // Ensure gravity is off
    }

    void FixedUpdate()
    {
        // Move the cloud
        rb.linearVelocity = direction.normalized * speed;

        // Remove objects no longer on the cloud
        objectsOnCloud = objectsOnCloud.Where(obj => obj != null && IsOnCloud(obj)).ToList();

        // Apply cloud velocity to all objects on it
        foreach (GameObject obj in objectsOnCloud.ToList())
        {
            if (obj != null)
            {
                Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
                if (objRb != null)
                {
                    Vector2 additionalVelocity = direction.normalized * speed;
                    objRb.linearVelocity += additionalVelocity;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!objectsOnCloud.Contains(collision.gameObject))
        {
            objectsOnCloud.Add(collision.gameObject);
        }

        Debug.Log($"Cloud collided with {collision.gameObject.name}");

        if (!collision.gameObject.CompareTag("Cannon") &&
            !collision.gameObject.CompareTag("Waddler") &&
            !collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        objectsOnCloud.Remove(collision.gameObject);
    }

    public void SetDirection(Transform cannonTransform, float speedToSet, bool tiltCloud, float customRotation)
    {
        float eulerAngle = (cannonTransform.eulerAngles.z);
        direction = new Vector3(Mathf.Cos((eulerAngle + 90) * Mathf.Deg2Rad), Mathf.Sin((eulerAngle + 90) * Mathf.Deg2Rad), 0);
        if (!tiltCloud)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, customRotation);
        }
    }

    private bool IsOnCloud(GameObject obj)
    {
        // Check if the object is still physically touching the cloud using collision bounds
        Collider2D objCollider = obj.GetComponent<Collider2D>();
        Collider2D cloudCollider = GetComponent<Collider2D>();
        if (objCollider == null || cloudCollider == null) return false;

        return objCollider.IsTouching(cloudCollider);
    }
    /*
    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            //Debug.DrawLine(contact.point, contact.point + contact.normal, Color.red, 2f);
            //Debug.Log($"Normal: {contact.normal}");
            objectsOnCloud.Add(collision.gameObject);
            Vector2 collisionNormal = contact.normal;
            float collisionAngle = Vector2.Angle(Vector2.up, collisionNormal);

            Debug.Log($"Collision Angle: {collisionAngle}°");

            if(collisionAngle == 180){
                //collision.gameObject.GetComponent<PlayerBehaviour>().RespawnPositionAtPosition();
                Debug.Log($"{collision.gameObject} landed on cloud.");
                //collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(direction.normalized.x, direction.normalized.y) * speed;
            }
        }
    }
    */
}

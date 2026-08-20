using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Parent the player so they ride it smoothly without velocity jitter
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
    public void SetDirection(
        Transform cannonTransform,
        float speedToSet,
        bool tiltCloud,
        float customRotation
    )
    {
        speed = speedToSet;

        float angle = cannonTransform.eulerAngles.z + 90f;
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0f
        );

        transform.rotation = tiltCloud
            ? Quaternion.Euler(0f, 0f, customRotation)
            : Quaternion.identity;
    }
}

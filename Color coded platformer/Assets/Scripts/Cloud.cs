using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Cloud : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction = Vector3.right;

    Rigidbody2D rb;

    // Objects physically riding this cloud
    List<Rigidbody2D> riders = new List<Rigidbody2D>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction.normalized * speed;

        // Apply movement ONLY to valid riders
        for (int i = riders.Count - 1; i >= 0; i--)
        {

            PlayerBehaviour player = riders[i].GetComponent<PlayerBehaviour>();
            if (player != null && player.onLadder)
            {
                riders.RemoveAt(i);
                continue;
            }

            if (riders[i] == null)
            {
                riders.RemoveAt(i);
                continue;
            }

            // Stop pushing if collision is ignored mid-ride
            if (Physics2D.GetIgnoreCollision(
                riders[i].GetComponent<Collider2D>(),
                GetComponent<Collider2D>()))
            {
                riders.RemoveAt(i);
                continue;
            }

            if(riders[i].GetComponentInParent<PlayerBehaviour>()) 
                riders[i].linearVelocity += (Vector2)direction.normalized * speed;
            else 
                riders[i].MovePosition(riders[i].position + (Vector2)(direction.normalized * speed * Time.fixedDeltaTime));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D objRb = collision.rigidbody;

        if(collision.gameObject.CompareTag("Ladder") ||
        collision.gameObject.CompareTag("Cloud") ||
        collision.gameObject.GetComponent<GroundCollisionCheck>()) 
            return;

        PlayerBehaviour player = collision.collider.GetComponentInParent<PlayerBehaviour>();
        if (player != null && player.onLadder)
            return;

        if(collision.gameObject.CompareTag("Ground")) 
            Destroy(gameObject);

        if (objRb == null) return;

        // DO NOT add if collision is being ignored
        if (Physics2D.GetIgnoreCollision(
            collision.collider,
            GetComponent<Collider2D>()))
            return;

        if (!riders.Contains(objRb))
            riders.Add(objRb);
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D objRb = collision.rigidbody;
        if (objRb == null) return;

        riders.Remove(objRb);
    }

    void OnDisable()
    {
        riders.Clear();
    }

    void OnDestroy()
    {
        riders.Clear();
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
        direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0f
        );

        transform.rotation = tiltCloud
            ? Quaternion.Euler(0f, 0f, customRotation)
            : Quaternion.identity;
    }
}

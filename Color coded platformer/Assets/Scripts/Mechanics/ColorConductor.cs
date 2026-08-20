using UnityEngine;

public class ColorConductor : MonoBehaviour
{
    public enum ConductorMode { Continuous, EventBased }
    public ConductorMode mode;
    
    public GameColor syncColor;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ReceiveBroadcast(Vector2 playerVelocity, bool jumpPressed)
    {
        if (mode == ConductorMode.Continuous)
        {
            if (rb != null) rb.linearVelocity = playerVelocity;
        }
        else if (mode == ConductorMode.EventBased && jumpPressed)
        {
            transform.position += Vector3.up * 1f; 
        }
    }
}

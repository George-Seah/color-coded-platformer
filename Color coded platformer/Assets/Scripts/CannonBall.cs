using UnityEngine;
using UnityEngine.Pool;

public class CannonBall : MonoBehaviour
{
    public float speed = 20f; 
    Rigidbody2D rb;
    private IObjectPool<CannonBall> parentPool;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 dir, float launchSpeed, IObjectPool<CannonBall> pool)
    {
        parentPool = pool;
        speed = launchSpeed;
        rb.linearVelocity = dir.normalized * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            GameManager.Instance.ReloadScene();
        }
        else if(collision.gameObject.TryGetComponent<Waddler>(out var waddler)){
            if(waddler.GetComponentInChildren<Key>() != null){
                waddler.GetComponentInChildren<Key>().Drop();
            }
            Destroy(waddler.gameObject);
        }
        
        if (!collision.gameObject.CompareTag("Cannon") && !collision.gameObject.CompareTag("Ladder") && !collision.gameObject.CompareTag("Cloud"))
        {
            if (parentPool != null) 
                parentPool.Release(this);
            else 
                Destroy(gameObject); 
        }
    }
}

using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    [SerializeField] Vector2 respawnPosition; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<PlayerBehaviour>().currentKey?.Drop();
            collision.gameObject.transform.position = respawnPosition;
        }
        if(collision.gameObject.CompareTag("Waddler") || collision.gameObject.CompareTag("Key")){
            Destroy(collision.gameObject);
        }
    }
}

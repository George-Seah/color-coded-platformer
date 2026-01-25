using UnityEngine;
using UnityEngine.SceneManagement;
public class RespawnField : MonoBehaviour
{
    [SerializeField] Vector2 respawnPosition;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.playerSpawnPoint = respawnPosition;
        }
    }
}

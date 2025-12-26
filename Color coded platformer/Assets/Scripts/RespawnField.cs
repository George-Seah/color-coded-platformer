using UnityEngine;
using UnityEngine.SceneManagement;
public class RespawnField : MonoBehaviour
{
    [SerializeField] Vector2 respawnPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.gameObject.GetComponent<PlayerBehaviour>().SetRespawnPosition(new Vector2(respawnPosition));
            //collision.gameObject.GetComponent<PlayerBehaviour>().SetRespawnPosition(respawnPosition);
            //Debug.Log("Player entered ground");
            FindFirstObjectByType<GameManager>().playerSpawnPoint = respawnPosition;
        }
    }
}

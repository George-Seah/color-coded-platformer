using UnityEngine;

public class GroundCollisionCheck : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    private void Start()
    {
        playerBehaviour = transform.parent.gameObject.GetComponent<PlayerBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block")
         || collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Cloud"))
        {
            playerBehaviour.EnteredGround();
            //Debug.Log("Player entered ground");
        }
        // && playerBehaviour.readJumping()
        if(collision.gameObject.CompareTag("Waddler")){
            playerBehaviour.LandedOnWaddler();
            if(collision.gameObject.GetComponentInChildren<Key>() != null){
                collision.gameObject.GetComponentInChildren<Key>().Drop();
            }
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block")
         || collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Cloud"))
        {
            playerBehaviour.EnteredGround();
            //Debug.Log("Player staying on ground");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block")
         || collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Cloud"))
        {
            playerBehaviour.ExitedGround();
            //Debug.Log("Player exited ground");
        }
    }
}

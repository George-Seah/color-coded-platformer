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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block"))
        {
            playerBehaviour.EnteredGround();
            //Debug.Log("Player entered ground");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block"))
        {
            playerBehaviour.EnteredGround();
            //Debug.Log("Player staying on ground");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Push Block"))
        {
            playerBehaviour.ExitedGround();
            //Debug.Log("Player exited ground");
        }
    }
}

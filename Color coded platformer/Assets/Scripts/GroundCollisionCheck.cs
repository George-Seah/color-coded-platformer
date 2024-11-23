using UnityEngine;

public class GroundCollisionCheck : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;

    private void Start()
    {
        playerBehaviour = transform.parent.gameObject.GetComponent<PlayerBehaviour>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerBehaviour.enteredGround();
            Debug.Log("You're grounded");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerBehaviour.exitedGround();
            Debug.Log("You're not grounded");
        }
    }
}

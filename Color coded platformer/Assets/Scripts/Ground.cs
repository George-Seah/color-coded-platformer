using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GroundCollisionCheck"))
        {
            collision.transform.parent.gameObject.GetComponent<PlayerBehaviour>().enteredGround();
            Debug.Log("You're grounded");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GroundCollisionCheck"))
        {
            collision.transform.parent.gameObject.GetComponent<PlayerBehaviour>().exitedGround();
            Debug.Log("You're left the ground");
        }
    }
}

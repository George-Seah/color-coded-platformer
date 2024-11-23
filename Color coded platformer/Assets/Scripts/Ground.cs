using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GroundCollisionCheck")) {
            collision.transform.parent.gameObject.GetComponent<PlayerBehaviour>().enteredGround();
            Debug.Log("You're grounded");
        }
    }
    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GroundCollisionCheck")) {
            collision.transform.parent.gameObject.GetComponent<PlayerBehaviour>().exitedGround();
            Debug.Log("You're left the ground");
        }
    }
}

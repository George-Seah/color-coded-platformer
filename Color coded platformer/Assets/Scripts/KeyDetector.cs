using UnityEngine;

public class KeyDetector : MonoBehaviour
{
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
        if (collision.gameObject.CompareTag("Key") && (collision.gameObject.layer == gameObject.layer || collision.gameObject.layer == LayerMask.NameToLayer("Default")))
        {
            Debug.Log("Key entered Door Checker");
            //collision.gameObject.GetComponent<Key>().Drop();
            if(collision.transform.parent != null && collision.transform.parent.CompareTag("Player")){
                collision.transform.parent.GetComponent<PlayerBehaviour>().SetKey(null);
            }
            else if(collision.transform.parent != null && collision.transform.parent.gameObject.CompareTag("Waddler")){
                collision.transform.parent.gameObject.GetComponent<Waddler>().SetKey(null);
            }
            Destroy(transform.parent.gameObject);
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Player")
        && collision.gameObject.GetComponent<PlayerBehaviour>().color == $"{LayerMask.LayerToName(gameObject.layer)}"){
            // 
            Debug.Log("Player entered Door Checker");
        }
    }
}

using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private ESave eSave;
    [SerializeField] string index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eSave = (ESave)FindFirstObjectByType(typeof(ESave));
        if(eSave.LoadStarData(index)){
            //Destroy(gameObject);
            GetComponent<SpriteRenderer>().color = Color.black;
            Debug.Log($"Star {index} was already collected.");
        }
        //eSave = GameObject.Find("SaveFile").GetComponent<ESave>;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player")){
            eSave.SaveStar(index, true);
            Debug.Log($"Star {index} is collected.");
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }
}

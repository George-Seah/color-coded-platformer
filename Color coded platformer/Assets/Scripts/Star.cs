using UnityEngine;
using System;

public class Star : MonoBehaviour
{
    [SerializeField] private ESave eSave;
    [SerializeField] string index;
    private ColorPositionChange CPC;
    private ParticleSystem particleSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CPC = GetComponent<ColorPositionChange>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        eSave = (ESave)FindFirstObjectByType(typeof(ESave));
        if(eSave.LoadStarData(index)){
            //Destroy(gameObject);
            GetComponent<SpriteRenderer>().color = Color.black;
            Debug.Log($"Star {index} was already collected.");
            if(CPC != null){
                Destroy(CPC);
                if(particleSystem != null){
                    Destroy(particleSystem);
                }
            }
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
            if(CPC != null){
                Destroy(CPC);
                if(particleSystem != null){
                    Destroy(particleSystem);
                }
            }
        }
    }
    
    [ContextMenu("Reset Star Data")]
    void ResetStarData(){
        eSave.ResetStar(index);
    }
}

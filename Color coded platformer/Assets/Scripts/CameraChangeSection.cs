using UnityEngine;
using Cinemachine;
public class CameraChangeSection : MonoBehaviour
{
    [SerializeField] GameObject cameraInCollider;
    [SerializeField] GameObject cameraOutCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision){
        HandleCollision(collision, true);
    }
    void OnTriggerStay2D(Collider2D collision){
        HandleCollision(collision, true);
    }
    void OnTriggerExit2D(Collider2D collision){
        HandleCollision(collision, false);
    }
    void HandleCollision(Collider2D collision, bool inCollider){
        if(collision.gameObject.CompareTag("Player")){
            if(inCollider){
                cameraInCollider.SetActive(true);
                cameraOutCollider.SetActive(false);
            }
            else{
                cameraOutCollider.SetActive(true);
                cameraInCollider.SetActive(false);
            }
        }
    }
}

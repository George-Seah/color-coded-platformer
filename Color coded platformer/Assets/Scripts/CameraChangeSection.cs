using UnityEngine;
using Unity.Cinemachine;

public class CameraChangeSection : MonoBehaviour
{
    [SerializeField] GameObject cameraInCollider;
    [SerializeField] GameObject cameraOutCollider;

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

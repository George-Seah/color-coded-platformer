using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Vector2 playerSpawnPoint;
    public GameObject canvasPrefab;
    void Awake(){
    DontDestroyOnLoad(gameObject);

    if (Instance == null)
        Instance = this;
    else
        Destroy(gameObject);
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} has fully loaded.");
        GameObject.Find("Player").GetComponent<PlayerBehaviour>().RespawnPositionAtPosition(playerSpawnPoint);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Red"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Blue"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Green"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), false);

            //Instantiate(Resources.Load("Assets/Prefabs/Canvas.prefab"));
            
            /*
            if(GameObject.Find("Player").transform.position.x == playerSpawnPoint.x && GameObject.Find("Player").transform.position.y == playerSpawnPoint.y){
                justReloaded = false;
                Debug.Log("justReloaded = false");
                Debug.Log($"Player.transform.position = {GameObject.Find("Player").transform.position}");
            }
            */
        // Perform any actions needed after the scene is fully loaded
    }
    /*
    public void SetRespawnPosition(Vector2 newPosition){
        //SceneManager.LoadScene($"{SceneManager.GetActiveScene()}", LoadSceneMode.Single);
        playerSpawnPoint = newPosition;
    }
    */
    public void ReloadScene(){
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Red"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Blue"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Green"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), false);

        //justReloaded = true;
        //Debug.Log("justReloaded = true");
    }
}

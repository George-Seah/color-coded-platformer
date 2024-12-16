using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameObject instance;
    public Vector2 playerSpawnPoint;
    public bool justReloaded = false;
    void Awake(){
        //currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //LevelCompleteMenu = Resources.FindObjectsOfTypeAll(LevelCompleteMenu);
        /*
        if((GameObject)FindObjectOfType(GameObject) == "Level Complete Menu"){
            LevelCompleteMenu = FindObjectOfType(GameObject);
        }
        */
    }
    /*
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
    */
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(justReloaded){
            GameObject.Find("Player").GetComponent<PlayerBehaviour>().RespawnPositionAtPosition(playerSpawnPoint);
            if(GameObject.Find("Player").transform.position.x == playerSpawnPoint.x && GameObject.Find("Player").transform.position.y == playerSpawnPoint.y){
                justReloaded = false;
                Debug.Log("justReloaded = false");
                Debug.Log($"Player.transform.position = {GameObject.Find("Player").transform.position}");
            }
        }
        */
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} has fully loaded.");
        GameObject.Find("Player").GetComponent<PlayerBehaviour>().RespawnPositionAtPosition(playerSpawnPoint);
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
        //justReloaded = true;
        Debug.Log("justReloaded = true");
    }
}

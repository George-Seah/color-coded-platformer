using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Vector2 playerSpawnPoint;

    void Awake(){
    DontDestroyOnLoad(gameObject);

    if (Instance == null)
        Instance = this;
    else
    {
        Destroy(gameObject);
        return;
    }

    if (GameSaveSystem.Instance == null && Object.FindAnyObjectByType<GameSaveSystem>() == null)
    {
        var saveGo = new GameObject("GameSaveSystem");
        saveGo.AddComponent<GameSaveSystem>();
    }
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
        var playerBehaviour = Object.FindAnyObjectByType<PlayerBehaviour>();
        if (playerBehaviour != null)
        {
            playerBehaviour.RespawnPositionAtPosition(playerSpawnPoint);
        }
        ResetLayerCollisions();
    }

    public void ReloadScene(){
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void ResetLayerCollisions()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Red"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Red"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Blue"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Blue"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Green"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GroundCollisionCheck"), LayerMask.NameToLayer("Green"), false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    public static CanvasScript Instance;

    public GameObject pauseMenu;
    public Button continueButton;  // Assign this in the Inspector

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        EnsureEventSystemExists();
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
        EnsureEventSystemExists();
    }

    public void PauseGame()
    {
        if (pauseMenu == null) return;

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);

            // Clear any current selection
            EventSystem.current.SetSelectedGameObject(null);

            // Auto-select the Continue button
            if (continueButton != null)
                continueButton.Select();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    void EnsureEventSystemExists()
    {
        if (FindObjectOfType<EventSystem>() != null) return;

        GameObject es = new GameObject("EventSystem");

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        es.AddComponent<EventSystem>();
        es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();
#endif

        DontDestroyOnLoad(es);
    }
}

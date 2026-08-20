using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CanvasScript : MonoBehaviour
{
    public static CanvasScript Instance;

    [Header("Menus")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject levelSelectMenu;
    [SerializeField] Button continueButton;

    [Header("Level Select")]
#if UNITY_EDITOR
    [SerializeField] SceneAsset[] sceneAssets; // Editor-only convenience
#endif
    [SerializeField] string[] sceneNames;      // Runtime-safe
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] Vector2 buttonSize = new Vector2(150, 150);
    [SerializeField] float fontSize = 60f;

    EventSystem eventSystem;
    GridLayoutGroup gridLayoutGroup;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (pauseMenu) pauseMenu.SetActive(false);

        EnsureEventSystemExists();

        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>(true);
        if (gridLayoutGroup)
        {
            gridLayoutGroup.cellSize = buttonSize;
            if (!levelSelectMenu)
                levelSelectMenu = gridLayoutGroup.transform.parent.gameObject;
        }

        GenerateLevelButtons();
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

    void GenerateLevelButtons()
    {
        if (!gridLayoutGroup || !levelButtonPrefab) return;

        foreach (Transform child in gridLayoutGroup.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < sceneNames.Length; i++)
        {
            string sceneName = sceneNames[i];

            GameObject buttonObj = Instantiate(levelButtonPrefab, gridLayoutGroup.transform);
            Button button = buttonObj.GetComponent<Button>();

            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text)
            {
                text.fontSize = fontSize;
                text.text = (i + 1).ToString();
            }

            button.onClick.AddListener(() => LoadSceneByName(sceneName));
        }
    }

    public void SelectUIComponent(GameObject target)
    {
        EnsureEventSystemExists();

        if (eventSystem == null)
        {
            Debug.LogError("CanvasScript: No EventSystem available.");
            return;
        }

        if (target != null)
            eventSystem.SetSelectedGameObject(target);
    }

    public void PauseGame()
    {
        if (!pauseMenu) return;

        bool closing = pauseMenu.activeSelf || (!pauseMenu.activeSelf && levelSelectMenu && levelSelectMenu.activeSelf);

        if (closing)
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);

            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            if (continueButton) continueButton.Select();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadSceneByName(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' is not in Build Settings.");
            return;
        }

        Time.timeScale = 1f;
        GameManager.Instance.playerSpawnPoint = Vector2.zero;
        SceneManager.LoadScene(sceneName);
    }

    public void ResetAllTrackedStars()
    {
        if (GameSaveSystem.Instance != null)
            GameSaveSystem.Instance.ResetAllTrackedStars();
        else if (ESave.Instance != null)
            ESave.Instance.ResetAllTrackedStars();
    }

    void EnsureEventSystemExists()
    {
        eventSystem = Object.FindAnyObjectByType<EventSystem>();
        if (eventSystem != null) return;

        GameObject es = new GameObject("EventSystem");
        eventSystem = es.AddComponent<EventSystem>();

    #if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
    #else
        es.AddComponent<StandaloneInputModule>();
    #endif

        DontDestroyOnLoad(es);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (sceneAssets == null) return;

        sceneNames = new string[sceneAssets.Length];
        for (int i = 0; i < sceneAssets.Length; i++)
        {
            sceneNames[i] = sceneAssets[i] ? sceneAssets[i].name : string.Empty;
        }
    }
#endif
}

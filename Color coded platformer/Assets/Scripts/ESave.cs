using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Compatibility shim. Legacy scenes/prefabs still reference ESave;
/// all data now routes through GameSaveSystem (JSON + PlayerPrefs).
/// </summary>
public class ESave : MonoBehaviour
{
    public static ESave Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (GameSaveSystem.Instance == null)
        {
            var go = new GameObject("GameSaveSystem");
            go.AddComponent<GameSaveSystem>();
        }
    }

    GameSaveSystem Sys
    {
        get
        {
            if (GameSaveSystem.Instance == null)
            {
                var existing = Object.FindAnyObjectByType<GameSaveSystem>();
                if (existing != null) return existing;
                var go = new GameObject("GameSaveSystem");
                return go.AddComponent<GameSaveSystem>();
            }
            return GameSaveSystem.Instance;
        }
    }

    public void SaveStar(string starIndex, bool collected) => Sys.SaveStar(starIndex, collected);
    public bool LoadStar(string starIndex) => Sys.LoadStar(starIndex);
    public void ResetStar(string starIndex) => Sys.ResetStar(starIndex);
    public List<string> GetAllTrackedStarKeys() => Sys.GetAllTrackedStarKeys();

    [ContextMenu("Reset All Tracked Stars")]
    public void ResetAllTrackedStars() => Sys.ResetAllTrackedStars();
}

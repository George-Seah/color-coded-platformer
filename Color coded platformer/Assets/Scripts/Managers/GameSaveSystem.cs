using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// JSON save data stored in persistentDataPath. Tracks collected stars and unlocked levels.
/// Replaces the third-party ESave plugin.
/// </summary>
[Serializable]
public class GameSaveData
{
    public List<string> collectedStars = new List<string>();
    public List<string> unlockedLevels = new List<string>();
    public int lastUnlockedLevelIndex = 1;
}

public class GameSaveSystem : MonoBehaviour
{
    public static GameSaveSystem Instance { get; private set; }

    const string FileName = "color_coded_save.json";
    const string PrefsKey = "ColorCodedSaveJson";

    GameSaveData data = new GameSaveData();
    string FilePath => Path.Combine(Application.persistentDataPath, FileName);

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void SaveStar(string starIndex, bool collected)
    {
        if (string.IsNullOrEmpty(starIndex)) return;
        if (collected)
        {
            if (!data.collectedStars.Contains(starIndex))
                data.collectedStars.Add(starIndex);
        }
        else
        {
            data.collectedStars.Remove(starIndex);
        }
        Persist();
    }

    public bool LoadStar(string starIndex)
    {
        if (string.IsNullOrEmpty(starIndex)) return false;
        return data.collectedStars.Contains(starIndex);
    }

    public void ResetStar(string starIndex)
    {
        SaveStar(starIndex, false);
    }

    public List<string> GetAllTrackedStarKeys()
    {
        return new List<string>(data.collectedStars);
    }

    [ContextMenu("Reset All Tracked Stars")]
    public void ResetAllTrackedStars()
    {
        data.collectedStars.Clear();
        Persist();
    }

    public void UnlockLevel(string sceneName, int levelIndex = -1)
    {
        if (!string.IsNullOrEmpty(sceneName) && !data.unlockedLevels.Contains(sceneName))
            data.unlockedLevels.Add(sceneName);
        if (levelIndex > data.lastUnlockedLevelIndex)
            data.lastUnlockedLevelIndex = levelIndex;
        Persist();
    }

    public bool IsLevelUnlocked(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return false;
        if (data.unlockedLevels.Count == 0) return sceneName.Contains("1");
        return data.unlockedLevels.Contains(sceneName);
    }

    public void Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                data = JsonUtility.FromJson<GameSaveData>(File.ReadAllText(FilePath)) ?? new GameSaveData();
                return;
            }
            if (PlayerPrefs.HasKey(PrefsKey))
            {
                data = JsonUtility.FromJson<GameSaveData>(PlayerPrefs.GetString(PrefsKey)) ?? new GameSaveData();
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"GameSaveSystem: failed to load save ({e.Message}). Starting fresh.");
            data = new GameSaveData();
        }
    }

    void Persist()
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(FilePath, json);
            PlayerPrefs.SetString(PrefsKey, json);
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogError($"GameSaveSystem: failed to write save ({e.Message}).");
        }
    }
}

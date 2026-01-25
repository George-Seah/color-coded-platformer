using UnityEngine;
using Esper.ESave;
using System.Collections.Generic;

public class ESave : MonoBehaviour
{
    SaveFile saveFile;

    // Key under which the tracked star list is saved
    const string STAR_KEYS_SAVE_KEY = "__StarKeysList";

    void Awake()
    {
        SaveFileSetup setup = GetComponent<SaveFileSetup>();
        if (setup == null)
        {
            Debug.LogError("ESave requires a SaveFileSetup component.");
            return;
        }

        saveFile = setup.GetSaveFile();
        if (saveFile != null)
        {
            saveFile.Load();
            Debug.Log("ESave: Save file loaded in Awake.");
        }
    }

    public void SaveStar(string starIndex, bool collected)
    {
        if (saveFile == null) return;

        saveFile.AddOrUpdateData(starIndex, collected);

        // Track this star index so we can reset it later
        TrackStarKey(starIndex);

        saveFile.Save();
        Debug.Log($"ESave: Saved star '{starIndex}' = {collected}");
    }

    public bool LoadStar(string starIndex)
    {
        if (saveFile == null) return false;

        try
        {
            return saveFile.GetData<bool>(starIndex);
        }
        catch
        {
            // If the key doesn’t exist yet, treat as false
            return false;
        }
    }

    public void ResetStar(string starIndex)
    {
        if (saveFile == null) return;

        saveFile.AddOrUpdateData(starIndex, false);
        saveFile.Save();
        Debug.Log($"ESave: Reset star '{starIndex}' to false.");
    }

    void TrackStarKey(string starIndex)
    {
        if (saveFile == null) return;

        // Get the safe list of tracked keys
        List<string> keys = GetSafeTrackedKeys();

        // If not already tracked, add it
        if (!keys.Contains(starIndex))
        {
            keys.Add(starIndex);
            saveFile.AddOrUpdateData(STAR_KEYS_SAVE_KEY, keys);
        }
    }

    /// <summary>
    /// Returns a non-null list of tracked star keys.
    /// If no tracked star list exists yet, returns an empty list.
    /// This safely catches the exception ESave throws when a key doesn’t exist. :contentReference[oaicite:1]{index=1}
    /// </summary>
    List<string> GetSafeTrackedKeys()
    {
        if (saveFile == null) return new List<string>();

        try
        {
            List<string> list = saveFile.GetData<List<string>>(STAR_KEYS_SAVE_KEY);
            return list ?? new List<string>();
        }
        catch
        {
            // If the data doesn’t exist yet, return an empty list
            return new List<string>();
        }
    }

    public List<string> GetAllTrackedStarKeys()
    {
        return GetSafeTrackedKeys();
    }

    /// <summary>
    /// Loops through all tracked star keys and resets them to false in the save.
    /// </summary>
    [ContextMenu("Reset All Tracked Stars")]
    public void ResetAllTrackedStars()
    {
        if (saveFile == null) return;

        List<string> allKeys = GetSafeTrackedKeys();

        // Loop through all tracked keys and reset them
        for (int i = 0; i < allKeys.Count; i++)
        {
            string key = allKeys[i];
            ResetStar(key);
            Debug.Log($"ESave: Reset saved flag for '{key}'.");
        }

        // Write all changes to disk
        saveFile.Save();
        Debug.Log($"ESave: Reset {allKeys.Count} tracked star(s).");
    }

    /*
    [ContextMenu("Print All Tracked Keys (Debug)")]
    public void PrintAllTrackedKeys()
    {
        List<string> keys = GetSafeTrackedKeys();
        Debug.Log($"Tracked Star Keys Count: {keys.Count}");
        foreach (string k in keys)
        {
            Debug.Log($"• {k}");
        }
    }*/
}

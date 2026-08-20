#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Menu items that swap legacy ColorPicker / VelocityChecker on the open scene
/// and on shared prefabs without moving transforms.
/// </summary>
public static class SceneMigrationUtility
{
    const string LegacyColorPickerGuid = "f3df70b878d90d9478d63dae2e208534";
    const string BaseColorPickerGuid = "cd256bbac20df1140a3b1e48045415f3";
    const string DoorColorPickerGuid = "166f4f177c945ce4ea0a744bc01b36ef";

    [MenuItem("Color Coded/Migrate Open Scene (ColorPicker → BaseColorPicker)")]
    public static void MigrateOpenScene()
    {
        int swapped = 0;
        var pickers = Object.FindObjectsByType<ColorPicker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var picker in pickers)
        {
            if (picker == null) continue;
            if (picker.GetType() != typeof(ColorPicker)) continue;

            var go = picker.gameObject;
            var color = picker.SelectedColor;
            Object.DestroyImmediate(picker, true);
            var next = go.AddComponent<BaseColorPicker>();
            next.colorState = color;
            next.ApplyColor();
            swapped++;
        }

        var solids = Object.FindObjectsByType<DirectionallySolidPlatform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var p in solids)
        {
            if (p.GetComponent<PlatformEffector2D>() == null)
                p.gameObject.AddComponent<PlatformEffector2D>();
            var col = p.GetComponent<Collider2D>();
            if (col != null) col.usedByEffector = true;
            var vc = p.GetComponent<VelocityChecker>();
            if (vc != null) Object.DestroyImmediate(vc, true);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log($"Scene migration: swapped {swapped} ColorPicker(s) to BaseColorPicker. PlatformEffector2D ensured on one-way platforms.");
    }

    [MenuItem("Color Coded/Migrate Shared Prefabs (ColorPicker guid)")]
    public static void MigratePrefabsOnDisk()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
        int files = 0;
        foreach (string g in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(g);
            string text = System.IO.File.ReadAllText(path);
            if (!text.Contains(LegacyColorPickerGuid)) continue;
            text = text.Replace(LegacyColorPickerGuid, BaseColorPickerGuid);
            text = text.Replace("selectedColor:", "colorState:");
            System.IO.File.WriteAllText(path, text);
            files++;
        }
        AssetDatabase.Refresh();
        Debug.Log($"Prefab migration: rewrote ColorPicker → BaseColorPicker on {files} prefab file(s).");
    }
}
#endif

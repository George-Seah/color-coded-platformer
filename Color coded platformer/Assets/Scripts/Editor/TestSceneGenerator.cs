using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

[InitializeOnLoad]
public class TestSceneGenerator
{
    static TestSceneGenerator()
    {
        EditorApplication.delayCall += AutoGenerateOnce;
    }

    static void AutoGenerateOnce()
    {
        if (!EditorPrefs.GetBool("TestScenesGenerated", false))
        {
            GenerateAllTestScenes();
            EditorPrefs.SetBool("TestScenesGenerated", true);
        }
    }

    [MenuItem("ColorPlatformer/Generate Test Scenes")]
    public static void GenerateAllTestScenes()
    {
        string dir = "Assets/Scenes/TestScenes";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        CreateTestScene(dir + "/Test_01_PhysicsMatrix.unity");
        CreateTestScene(dir + "/Test_02_PlayerUpgrades.unity");
        CreateTestScene(dir + "/Test_03_NewTools.unity");
        
        Debug.Log("Generated 3 separate test scenes in " + dir);
    }

    static void CreateTestScene(string path)
    {
        if (!File.Exists(path))
        {
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            
            GameObject cam = new GameObject("Main Camera");
            cam.AddComponent<Camera>();
            
            GameObject light = new GameObject("Directional Light");
            Light l = light.AddComponent<Light>();
            l.type = LightType.Directional;

            GameObject ground = new GameObject("Ground");
            ground.layer = LayerMask.NameToLayer("Default");
            var bc = ground.AddComponent<BoxCollider2D>();
            bc.size = new Vector2(20, 1);
            ground.transform.position = new Vector3(0, -3, 0);

            EditorSceneManager.SaveScene(newScene, path);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ColorDictionary", menuName = "Color Platformer/Color Dictionary")]
public class ColorDictionary : ScriptableObject 
{
    [System.Serializable]
    public struct ColorData 
    {
        public GameColor state;
        public Material material;
        public Color tint;
    }
    
    public List<ColorData> colorDataList = new List<ColorData>();
    
    private static ColorDictionary _instance;
    public static ColorDictionary Instance 
    {
        get 
        {
            if (_instance == null) 
            {
                _instance = Resources.Load<ColorDictionary>("ColorDictionary");
                if (_instance == null)
                    Debug.LogWarning("ColorDictionary not found in Resources folder! Create one via Right Click -> Create -> Color Platformer -> Color Dictionary, and name it 'ColorDictionary'.");
            }
            return _instance;
        }
    }
    
    public Material GetMaterial(GameColor state) 
    {
        foreach (var data in colorDataList) 
        {
            if (data.state == state) return data.material;
        }
        return null;
    }

    public Color GetColorTint(GameColor state) 
    {
        foreach (var data in colorDataList) 
        {
            if (data.state == state) return data.tint;
        }
        return GetDefaultUnityColor(state);
    }

    public Color GetDefaultUnityColor(GameColor color)
    {
        switch(color)
        {
            case GameColor.Red: return Color.red;
            case GameColor.Blue: return Color.blue;
            case GameColor.Yellow: return Color.yellow;
            case GameColor.Green: return Color.green;
            case GameColor.Orange: return new Color(1f, 0.5f, 0f);
            case GameColor.Purple: return new Color(0.5f, 0f, 0.5f);
            case GameColor.Black: return Color.black;
            case GameColor.White: return Color.white;
            default: return Color.white;
        }
    }
}

using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    protected SpriteRenderer sr;

    [SerializeField] protected GameColor selectedColor = GameColor.Default;
    public GameColor SelectedColor
    {
        get => selectedColor;
        set
        {
            selectedColor = value;
            ChangeObjectColor(selectedColor);
        }
    }

    [SerializeField] protected bool changeLayerMask = true;

    public virtual void ChangeObjectColor(GameColor color)
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (sr)
        {
            if (ColorDictionary.Instance != null) 
            {
                Material mat = ColorDictionary.Instance.GetMaterial(color);
                if (mat != null) 
                {
                    sr.material = mat;
                }
                else 
                {
                    sr.color = ColorDictionary.Instance.GetColorTint(color);
                }
            }
            else
            {
                // Safe fallback if dictionary isn't loaded yet (e.g. Editor OnValidate)
                sr.color = GetFallbackColor(color);
            }
        }

        if(changeLayerMask && color != GameColor.ColorPositionChange && color != GameColor.None)
        {
            string layerName = color == GameColor.Default ? "Default" : color.ToString();
            int layerId = LayerMask.NameToLayer(layerName);
            if(layerId != -1) gameObject.layer = layerId;
        }
    }

    protected Color GetFallbackColor(GameColor color)
    {
        switch (color)
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

    void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _ApplyColorSafe;
    }

    private void _ApplyColorSafe()
    {
        if (this != null) ChangeObjectColor(this.selectedColor);
    }

    protected virtual void Awake()
    {
        ChangeObjectColor(this.selectedColor);
    }
}

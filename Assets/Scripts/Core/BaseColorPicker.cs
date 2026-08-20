using UnityEngine;

[ExecuteAlways]
public class BaseColorPicker : MonoBehaviour 
{
    public GameColor colorState = GameColor.Default;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake() 
    {
        ApplyColor();
    }

    protected virtual void OnValidate() 
    {
        UnityEditor.EditorApplication.delayCall += _ApplyColorSafe;
    }

    private void _ApplyColorSafe()
    {
        if (this != null) ApplyColor();
    }

    public virtual void ApplyColor() 
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        if (ColorDictionary.Instance != null) 
        {
            Material mat = ColorDictionary.Instance.GetMaterial(colorState);
            if (mat != null) spriteRenderer.material = mat;
            else spriteRenderer.color = ColorDictionary.Instance.GetColorTint(colorState);
        }

        if (colorState != GameColor.None && colorState != GameColor.ColorPositionChange)
        {
            string layerName = colorState == GameColor.Default ? "Default" : colorState.ToString();
            int layerID = LayerMask.NameToLayer(layerName);
            if (layerID != -1) 
            {
                gameObject.layer = layerID;
            }
        }
    }
}

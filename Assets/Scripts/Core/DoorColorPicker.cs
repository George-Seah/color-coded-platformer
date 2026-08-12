using UnityEngine;

public class DoorColorPicker : BaseColorPicker 
{
    public override void ApplyColor() 
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        if (ColorDictionary.Instance != null) 
        {
            Material mat = ColorDictionary.Instance.GetMaterial(colorState);
            if (mat != null) spriteRenderer.material = mat;
            else spriteRenderer.color = ColorDictionary.Instance.GetColorTint(colorState);
        }

        gameObject.layer = LayerMask.NameToLayer("Default");

        if (colorState != GameColor.None && colorState != GameColor.ColorPositionChange)
        {
            string layerName = colorState == GameColor.Default ? "Default" : colorState.ToString();
            int layerID = LayerMask.NameToLayer(layerName);
            
            if (layerID != -1) 
            {
                BoxCollider2D[] childColliders = GetComponentsInChildren<BoxCollider2D>();
                foreach (var col in childColliders) 
                {
                    if (col.isTrigger && col.gameObject != this.gameObject) 
                    {
                        col.gameObject.layer = layerID;
                    }
                }
            }
        }
    }
}

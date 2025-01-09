using UnityEngine;

public class ColorPositionChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColorAndTag(Color color, string tag)
    {
        // Update the color of the SpriteRenderer
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }

        // Update the GameObject's layer tag
        //gameObject.tag = tag;
        gameObject.layer = LayerMask.NameToLayer(tag); // Ensure layers are correctly set up in Unity
    }
}

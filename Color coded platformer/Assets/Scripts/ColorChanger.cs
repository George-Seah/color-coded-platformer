using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ColorChanger : MonoBehaviour
{
    public enum ColorOptions
    {
        Red,
        Green,
        Blue,
        White
    }

    [SerializeField] ColorOptions selectedColor;
    [SerializeField] Color colorChange;

    [Tooltip("List of tags to detect")]
    [SerializeField] List<string> tagsToDetect = new List<string>();

    [SerializeField] ParticleSystem particleSystem;  // Reference to the particle system
    [SerializeField] Sprite[] sprites;              // Array of sprites to add to the particle system
    [SerializeField] Material shaderMaterial;       // Material using your shader graph
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (tagsToDetect.Contains(collision.tag))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Player detected inside ColorChanger.");
                collision.gameObject.GetComponent<PlayerBehaviour>().ColorChanged(colorChange);
            }
            else
            {
                switch (selectedColor)
                {
                    case ColorOptions.Red:
                        Debug.Log("Red selected");
                        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        collision.gameObject.layer = LayerMask.NameToLayer("Red");
                        break;
                    case ColorOptions.Blue:
                        Debug.Log("Blue selected");
                        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                        collision.gameObject.layer = LayerMask.NameToLayer("Blue");
                        break;
                    case ColorOptions.Green:
                        Debug.Log("Green selected");
                        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                        collision.gameObject.layer = LayerMask.NameToLayer("Green");
                        break;
                    case ColorOptions.White:
                        Debug.Log("White selected");
                        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                        collision.gameObject.layer = LayerMask.NameToLayer("Default");
                        break;
                }
            }
        }
    }
    
    void Awake()
    {
        // Set the color based on the selected option
        switch (selectedColor)
        {
            case ColorOptions.Red:
                colorChange = Color.red;
                break;
            case ColorOptions.Green:
                colorChange = Color.green;
                break;
            case ColorOptions.Blue:
                colorChange = Color.blue;
                break;
            case ColorOptions.White:
                colorChange = Color.white;
                break;
        }

        if (particleSystem != null)
        {
            // Configure the particle system's texture sheet animation
            var textureSheetAnimation = particleSystem.textureSheetAnimation;
            textureSheetAnimation.mode = ParticleSystemAnimationMode.Sprites;

            // Remove any existing sprites in the particle system
            textureSheetAnimation.RemoveSprite(0);

            // Add each sprite from the array and ensure the material is applied
            if (sprites != null)
            {
                foreach (var sprite in sprites)
                {
                    if (sprite != null)
                    {
                        textureSheetAnimation.AddSprite(sprite); // Add sprite to the particle system
                    }
                }
            }

            if (shaderMaterial != null)
            {
                // Assign the shader material to the particle system's renderer
                var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                if (renderer != null) renderer.material = shaderMaterial;

                // Optional: Pass the colorChange value to the shader material
                if (shaderMaterial.HasProperty("_TintColor"))
                {
                    shaderMaterial.SetColor("_TintColor", colorChange);
                }
            }
        }
    }
}

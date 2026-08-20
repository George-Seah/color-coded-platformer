using UnityEngine;
using System.Collections.Generic;

public class ColorChanger : MonoBehaviour, ISerializationCallbackReceiver
{
    [UnityEngine.Serialization.FormerlySerializedAs("selectedColor")]
    [SerializeField, HideInInspector]
    private int legacySelectedColor; 

    [SerializeField, HideInInspector]
    private bool hasMigratedColor = false;

    public GameColor targetColor = GameColor.White;
    private Color colorChange;

    [Tooltip("List of tags to detect")]
    public List<string> tagsToDetect = new List<string>();
    
    [Tooltip("The objects this trigger will change (replaces tags)")]
    public List<GameObject> targetObjects = new List<GameObject>();

    [SerializeField] ParticleSystem colorParticleSystem;  
    [SerializeField] Sprite[] sprites;              
    [SerializeField] Material shaderMaterial;       

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        if (!hasMigratedColor)
        {
            if (legacySelectedColor == 0) targetColor = GameColor.Red;
            else if (legacySelectedColor == 1) targetColor = GameColor.Green;
            else if (legacySelectedColor == 2) targetColor = GameColor.Blue;
            else if (legacySelectedColor == 3) targetColor = GameColor.White;
            else targetColor = GameColor.White;

            hasMigratedColor = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (tagsToDetect.Contains(collision.tag) || targetObjects.Contains(collision.gameObject))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<PlayerBehaviour>().ColorChanged(colorChange);
            }
            else
            {
                if (collision.gameObject.TryGetComponent<BaseColorPicker>(out var picker))
                {
                    picker.colorState = targetColor;
                    picker.ApplyColor();
                }
                else
                {
                    string layerName = targetColor == GameColor.Default ? "Default" : targetColor.ToString();
                    collision.gameObject.layer = LayerMask.NameToLayer(layerName);
                    if (collision.gameObject.TryGetComponent<SpriteRenderer>(out var sr))
                    {
                        sr.color = colorChange;
                    }
                }
            }
        }
    }
    
    void Awake()
    {
        colorChange = ColorDictionary.Instance != null ? ColorDictionary.Instance.GetDefaultUnityColor(targetColor) : Color.white;

        if (colorParticleSystem != null)
        {
            var textureSheetAnimation = colorParticleSystem.textureSheetAnimation;
            textureSheetAnimation.mode = ParticleSystemAnimationMode.Sprites;
            
            while (textureSheetAnimation.spriteCount > 0)
            {
                textureSheetAnimation.RemoveSprite(0);
            }

            foreach (GameObject obj in targetObjects)
            {
                if (obj != null && obj.TryGetComponent<SpriteRenderer>(out var sr))
                {
                    if (sr.sprite != null) textureSheetAnimation.AddSprite(sr.sprite);
                }
            }

            if (sprites != null)
            {
                foreach (var sprite in sprites)
                {
                    if (sprite != null)
                    {
                        textureSheetAnimation.AddSprite(sprite);
                    }
                }
            }

            if (shaderMaterial != null)
            {
                var renderer = colorParticleSystem.GetComponent<ParticleSystemRenderer>();
                if (renderer != null) renderer.material = shaderMaterial;

                if (shaderMaterial.HasProperty("_TintColor"))
                {
                    shaderMaterial.SetColor("_TintColor", colorChange);
                }
            }
        }
    }
}

using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    private SpriteRenderer sr;
    public enum ColorOptions
    {
        Red,
        Green,
        Blue,
        White
    }
    public ColorOptions selectedColor;
    [SerializeField] bool changeColor;
    [SerializeField] bool changeLayerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        switch (selectedColor)
        {
            case ColorOptions.Red:
                Debug.Log("Red selected");
                if(changeColor)
                    sr.color = Color.red;
                if(changeLayerMask)
                    gameObject.layer = LayerMask.NameToLayer("Red");
                break;
            case ColorOptions.Blue:
                Debug.Log("Blue selected");
                if(changeColor)
                    sr.color = Color.blue;
                if(changeLayerMask)
                    gameObject.layer = LayerMask.NameToLayer("Blue");
                break;
            case ColorOptions.Green:
                Debug.Log("Green selected");
                if(changeColor)
                    sr.color = Color.green;
                if(changeLayerMask)
                    gameObject.layer = LayerMask.NameToLayer("Green");
                break;
            case ColorOptions.White:
                Debug.Log("White selected");
                if(changeColor)
                    sr.color = Color.white;
                if(changeLayerMask)
                    gameObject.layer = LayerMask.NameToLayer("Default");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

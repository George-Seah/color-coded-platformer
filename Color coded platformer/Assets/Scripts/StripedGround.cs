using UnityEngine;

public class StripedGround : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public enum ColorOptions
    {
        Red,
        Green,
        Blue,
        White
    }
    public ColorOptions selectedColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        switch (selectedColor)
        {
            case ColorOptions.Red:
                Debug.Log("Red selected");
                GetComponent<SpriteRenderer>().color = Color.red;
                gameObject.layer = LayerMask.NameToLayer("Red");
                break;
            case ColorOptions.Blue:
                Debug.Log("Blue selected");
                GetComponent<SpriteRenderer>().color = Color.blue;
                gameObject.layer = LayerMask.NameToLayer("Blue");
                break;
            case ColorOptions.Green:
                Debug.Log("Green selected");
                GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = LayerMask.NameToLayer("Green");
                break;
            case ColorOptions.White:
                Debug.Log("White selected");
                GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.layer = LayerMask.NameToLayer("Ground");
                break;
        }
    }
}

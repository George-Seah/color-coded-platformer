using UnityEngine;

public class StripedGround : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public enum ColorOptions
    {
        Red,
        Green,
        Blue
    }
    public ColorOptions selectedColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (selectedColor)
        {
            case ColorOptions.Red:
                Debug.Log("Red selected");
                GetComponent<SpriteRenderer>().color = Color.red;
                gameObject.layer = LayerMask.NameToLayer("RedStripedGround");
                break;
            case ColorOptions.Blue:
                Debug.Log("Blue selected");
                GetComponent<SpriteRenderer>().color = Color.blue;
                gameObject.layer = LayerMask.NameToLayer("BlueStripedGround");
                break;
            case ColorOptions.Green:
                Debug.Log("Green selected");
                GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = LayerMask.NameToLayer("GreenStripedGround");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

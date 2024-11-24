using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public enum ColorOptions
    {
        Red,
        Green,
        Blue
    }
    public ColorOptions selectedColor;
    [SerializeField] Color colorChange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("TintColor", Color.green);*/
        switch (selectedColor)
        {
            case ColorOptions.Red:
                Debug.Log("Red selected");
                colorChange = Color.red;
                break;
            case ColorOptions.Green:
                Debug.Log("Green selected");
                colorChange = Color.green;
                break;
            case ColorOptions.Blue:
                Debug.Log("Blue selected");
                colorChange = Color.blue;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //collision.gameObject.GetComponent<SpriteRenderer>().color = colorChange;
            collision.gameObject.GetComponent<PlayerBehaviour>().ColorChanged(colorChange);
        }
    }
}

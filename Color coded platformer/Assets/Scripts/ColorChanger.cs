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
    public ColorOptions selectedColor;
    [SerializeField] Color colorChange;
    // List of tags to detect (set this in the Inspector)
    [Tooltip("List of tags to detect")]
    public List<string> tagsToDetect = new List<string>();
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
            case ColorOptions.White:
                Debug.Log("White selected");
                colorChange = Color.white;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(tagsToDetect.Contains(collision.tag)){
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //collision.gameObject.GetComponent<SpriteRenderer>().color = colorChange;
                Debug.Log("Player detected inside ColorChanger.");
                collision.gameObject.GetComponent<PlayerBehaviour>().ColorChanged(colorChange);
            }
            else{
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
}

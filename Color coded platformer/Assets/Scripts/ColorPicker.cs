using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    private SpriteRenderer sr;
    public enum ColorOptions
    {
        Red,
        Green,
        Blue,
        Default
    }
    public ColorOptions selectedColor;
    //[SerializeField] bool changeColor;
    [SerializeField] bool changeLayerMask = true;
    public void ChangeObjectColor(string newColor){
        sr = GetComponent<SpriteRenderer>();

        //Debug.Log($"Before changing: Color - {sr.color}, Layer - {gameObject.layer}");


        if(newColor == "red" || newColor == "Red"){
            Debug.Log("Red selected in ChangeObjectColor");
            if(sr != null)
                sr.color = Color.red;
            if(changeLayerMask)
                gameObject.layer = LayerMask.NameToLayer("Red");
        }
        else if(newColor == "blue" || newColor == "Blue"){
            Debug.Log("Blue selected in ChangeObjectColor");
            if(sr != null)
                sr.color = Color.blue;
            if(changeLayerMask)
                gameObject.layer = LayerMask.NameToLayer("Blue");
        }
        else if(newColor == "green" || newColor == "Green"){
            Debug.Log("Green selected in ChangeObjectColor");
            if(sr != null)
                sr.color = Color.green;
            if(changeLayerMask)
                gameObject.layer = LayerMask.NameToLayer("Green");
        }
        else{
            Debug.Log("White selected in ChangeObjectColor");
            if(sr != null)
                sr.color = Color.white;
            if(changeLayerMask)
                gameObject.layer = LayerMask.NameToLayer("Default");
        }
        
        //Debug.Log($"After changing: Color - {sr.color}, Layer - {gameObject.layer}");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeObjectColor($"{selectedColor}");
        //Debug.Log($"{gameObject} color is {sr.color}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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
    [SerializeField] bool changeColor;
    [SerializeField] bool changeLayerMask;
    public void ChangeObjectColor(string newColor){
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        //changeColor = true;
        changeLayerMask = true;
        Debug.Log($"Before changing: Color - {sr.color}, Layer - {gameObject.layer}");


        if(newColor == "red" || newColor == "Red"){
            Debug.Log("Red selected in ChangeObjectColor");
            if(changeColor)
                sr.color = Color.red;
            if(changeLayerMask)
                gameObject.layer = LayerMask.NameToLayer("Red");
        }
        else if(newColor == "blue" || newColor == "Blue"){
            Debug.Log("Blue selected in ChangeObjectColor");
            if(changeColor)
                sr.color = Color.blue;
            if(changeLayerMask)
                gameObject.layer = LayerMask.NameToLayer("Blue");
        }
        else if(newColor == "green" || newColor == "Green"){
            Debug.Log("Green selected in ChangeObjectColor");
            if(changeColor)
                sr.color = Color.green;
            if(changeLayerMask)
                gameObject.layer = LayerMask.NameToLayer("Green");
        }
        else{
            Debug.Log("White selected in ChangeObjectColor");
            if(changeColor)
                sr.color = Color.white;
            if(changeLayerMask)
                gameObject.layer = LayerMask.NameToLayer("Default");
        }
        
        Debug.Log($"After changing: Color - {sr.color}, Layer - {gameObject.layer}");
    }
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
            case ColorOptions.Default:
                Debug.Log("White selected");
                if(changeColor)
                    sr.color = Color.white;
                if(changeLayerMask)
                    gameObject.layer = LayerMask.NameToLayer("Default");
                break;
        }
        //Debug.Log($"{gameObject} color is {sr.color}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

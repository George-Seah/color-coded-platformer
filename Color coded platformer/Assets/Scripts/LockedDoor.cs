using UnityEngine;

public class LockedDoor : ColorPicker
{
    GameObject keyDetector;
    
    void Awake()
    {
        keyDetector = GetComponentInChildren<KeyDetector>().gameObject;

        //Auto sets to default layer on awake to make sure locked doors cannot be bypassed by color changing
        changeLayerMask = false;

        gameObject.layer = LayerMask.NameToLayer("Default"); 
    }

    void Start()
    {
        ChangeObjectColor(this.SelectedColor);
    }

    public override void ChangeObjectColor(GameColor color)
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if(keyDetector == null)
            keyDetector = GetComponentInChildren<KeyDetector>().gameObject;

        switch (color)
        {
            case GameColor.Red:
                if (sr) this.sr.color = Color.red;
                keyDetector.layer = LayerMask.NameToLayer("Red");
                break;
            case GameColor.Green:
                if (sr) this.sr.color = Color.green;
                keyDetector.layer = LayerMask.NameToLayer("Green");
                break;
            case GameColor.Blue:
                if (sr) this.sr.color = Color.blue;
                keyDetector.layer = LayerMask.NameToLayer("Blue");
                break;
            default:
                if (sr) this.sr.color = Color.white;
                keyDetector.layer = LayerMask.NameToLayer("Default");
                break;
        }
    }
}

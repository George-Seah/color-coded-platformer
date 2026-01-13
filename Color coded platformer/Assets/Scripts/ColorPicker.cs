using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    protected SpriteRenderer sr;

    [SerializeField] protected GameColor selectedColor = GameColor.Default;
    public GameColor SelectedColor
    {
        get => selectedColor;
        set
        {
            selectedColor = value;
            ChangeObjectColor(selectedColor);
        }
    }

    [SerializeField] protected bool changeLayerMask = true;

    public virtual void ChangeObjectColor(GameColor color)
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        switch (color)
        {
            case GameColor.Red:
                if (sr) this.sr.color = Color.red;
                if(changeLayerMask) gameObject.layer = LayerMask.NameToLayer("Red");
                break;
            case GameColor.Green:
                if (sr) this.sr.color = Color.green;
                if(changeLayerMask) gameObject.layer = LayerMask.NameToLayer("Green");
                break;
            case GameColor.Blue:
                if (sr) this.sr.color = Color.blue;
                if(changeLayerMask) gameObject.layer = LayerMask.NameToLayer("Blue");
                break;
            default:
                if (sr) this.sr.color = Color.white;
                if(changeLayerMask) gameObject.layer = LayerMask.NameToLayer("Default");
                break;
        }
    }

    // Called in editor when inspector changes are made
    void OnValidate()
    {
        ChangeObjectColor(this.selectedColor);
    }

    protected virtual void Awake()
    {
        // Optional runtime guarantee
        ChangeObjectColor(this.selectedColor);
    }
}

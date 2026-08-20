using UnityEngine;

/// <summary>
/// Legacy wrapper around BaseColorPicker so existing prefabs keep working.
/// Prefer attaching BaseColorPicker on new objects.
/// </summary>
public class ColorPicker : BaseColorPicker
{
    [SerializeField] protected GameColor selectedColor = GameColor.Default;
    [SerializeField] protected bool changeLayerMask = true;

    public GameColor SelectedColor
    {
        get => colorState;
        set
        {
            colorState = value;
            selectedColor = value;
            ApplyColor();
        }
    }

    protected override void Awake()
    {
        colorState = selectedColor;
        base.Awake();
    }

    protected override void OnValidate()
    {
        colorState = selectedColor;
        base.OnValidate();
    }

    public virtual void ChangeObjectColor(GameColor color)
    {
        SelectedColor = color;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class RingAura : MonoBehaviour
{
    public GameColor auraColor;
    private Dictionary<BaseColorPicker, GameColor> originalColors = new Dictionary<BaseColorPicker, GameColor>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseColorPicker>(out var picker))
        {
            if (!originalColors.ContainsKey(picker))
            {
                originalColors[picker] = picker.colorState;
            }
            picker.colorState = auraColor;
            picker.ApplyColor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseColorPicker>(out var picker))
        {
            if (originalColors.TryGetValue(picker, out GameColor origColor))
            {
                picker.colorState = origColor;
                picker.ApplyColor();
                originalColors.Remove(picker);
            }
        }
    }
}

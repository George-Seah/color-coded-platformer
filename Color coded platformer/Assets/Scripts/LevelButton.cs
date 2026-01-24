using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    GridLayoutGroup gridLayoutGroup;
    TextMeshProUGUI numberText;
    public static Vector2 buttonSize = new Vector2(150, 150);
    public static float fontSize = 60f;
    [SerializeField] float FontSize
    {
        get => fontSize;
        set 
        { 
            fontSize = value;
            if (numberText != null)
                numberText.fontSize = fontSize;
        }
    }
    int levelNumber;

    void OnValidate()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        numberText = GetComponentInChildren<TextMeshProUGUI>();
        levelNumber = transform.GetSiblingIndex() + 1;

        numberText.fontSize = FontSize;
        numberText.text = levelNumber.ToString();
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.cellSize = buttonSize;
        }

        
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//! Things change color based on PLAYER position
public class PositionBasedColorChanger : MonoBehaviour
{
    public static PositionBasedColorChanger Instance;

    [Header("Player")]
    public PlayerBehaviour playerBehaviour;

    [Header("Region Colors (Order Matters)")]
    public List<GameColor> regionColors =
        new List<GameColor> { GameColor.Default, GameColor.Red, GameColor.Green, GameColor.Blue };

    [Header("Region Settings")]
    public float startX;
    public float startY;
    public float regionWidth = 5f;
    public bool useXCoordinate = true;

    [Header("UI Bar")]
    public RectTransform barTransform;
    public RectTransform arrowTransform;
    public float totalWidth = 250f;
    public float totalHeight = 30f;

    [Header("UI Shadow")]
    [SerializeField] private Vector2 shadowDistance = new Vector2(2f, -2f);
    [SerializeField] private Color shadowColor = new Color(0f, 0f, 0f, 0.75f);

    private readonly List<RectTransform> regionSegments = new();

    void Awake()
    {
        if (!playerBehaviour)
            playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

void Start()
{
    if (barTransform)
    {
        PositionBarInBottomRight();
        GenerateBar();
    }

    if (arrowTransform)
    {
        ApplyShadow(arrowTransform.gameObject);
    }
}


    void FixedUpdate()
    {
        UpdateArrowPosition();
    }

    // ============================
    // PUBLIC API
    // ============================

    public GameColor GetCurrentRegionColor()
    {
        if (!playerBehaviour || regionColors.Count == 0)
            return GameColor.Default;

        float position = useXCoordinate
            ? playerBehaviour.transform.position.x
            : playerBehaviour.transform.position.y;

        float start = useXCoordinate ? startX : startY;

        int regionIndex = Mathf.FloorToInt((position - start) / regionWidth);

        int wrappedIndex =
            ((regionIndex % regionColors.Count) + regionColors.Count) % regionColors.Count;

        return regionColors[wrappedIndex];
    }

    // ============================
    // UI BAR
    // ============================

    private void PositionBarInBottomRight()
    {
        barTransform.anchorMin = new Vector2(1, 0);
        barTransform.anchorMax = new Vector2(1, 0);
        barTransform.pivot = new Vector2(1, 0);
        barTransform.anchoredPosition = Vector2.zero;
        barTransform.sizeDelta = new Vector2(totalWidth, totalHeight);
    }

    private void GenerateBar()
    {
        foreach (Transform child in barTransform)
            Destroy(child.gameObject);

        regionSegments.Clear();

        float regionSize = totalWidth / regionColors.Count;

        for (int i = 0; i < regionColors.Count; i++)
        {
            GameObject segment = new GameObject($"Region {i}", typeof(Image));
            segment.transform.SetParent(barTransform, false);

            RectTransform rt = segment.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 0.5f);
            rt.sizeDelta = new Vector2(regionSize, totalHeight);
            rt.anchoredPosition = new Vector2(i * regionSize, 0);

            Image img = segment.GetComponent<Image>();
            img.color = GetUIColor(regionColors[i]);

            ApplyShadow(segment);

            regionSegments.Add(rt);
        }
    }

    private void UpdateArrowPosition()
    {
        if (!playerBehaviour || !arrowTransform || regionColors.Count == 0)
            return;

        float position = useXCoordinate
            ? playerBehaviour.transform.position.x
            : playerBehaviour.transform.position.y;

        float relative =
            (position - (useXCoordinate ? startX : startY)) / regionWidth;

        relative = Mathf.Repeat(relative, regionColors.Count);

        float arrowX = (relative / regionColors.Count) * totalWidth - totalWidth;

        float arrowHeight = arrowTransform.rect.height;
        float arrowY = barTransform.position.y + (totalHeight / 2f) + arrowHeight;

        arrowTransform.anchoredPosition = new Vector2(arrowX, arrowY);
    }

    // ============================
    // UI UTILITIES
    // ============================

    void ApplyShadow(GameObject target)
    {
        // Remove thin Shadow if present
        Shadow oldShadow = target.GetComponent<Shadow>();
        if (oldShadow)
            Destroy(oldShadow);

        Outline outline = target.GetComponent<Outline>();
        if (!outline)
            outline = target.AddComponent<Outline>();

        outline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        outline.effectDistance = new Vector2(3f, -3f);
        outline.useGraphicAlpha = true;
    }


    private Color GetUIColor(GameColor color)
    {
        return color switch
        {
            GameColor.Red => Color.red,
            GameColor.Green => Color.green,
            GameColor.Blue => Color.blue,
            _ => Color.white
        };
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    [SerializeField] Vector2 shadowDistance = new Vector2(3f, -3f);
    [SerializeField] Color shadowColor = new Color(0f, 0f, 0f, 0.9f);

    readonly List<RectTransform> regionSegments = new();

    // ============================
    // EDITOR SAFETY
    // ============================

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!barTransform)
            return;

        EditorApplication.delayCall += () =>
        {
            if (this == null || barTransform == null)
                return;

            RebuildBarEditorSafe();
        };
    }
#endif

    // ============================
    // LIFECYCLE
    // ============================

    void Awake()
    {
        if (!playerBehaviour)
            playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (!barTransform)
        return;

        EditorApplication.delayCall += () =>
        {
            if (this == null || barTransform == null)
                return;

            RebuildBarEditorSafe();
        };
    }

    void Start()
    {
        if (barTransform)
            RebuildBarRuntime();
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
    // BAR REBUILDING
    // ============================

    void RebuildBarRuntime()
    {
        ClearBar();
        PositionBarInBottomRight();
        GenerateBarInternal();
    }

#if UNITY_EDITOR
    void RebuildBarEditorSafe()
    {
        if (Application.isPlaying)
            return;

        ClearBar();
        PositionBarInBottomRight();
        GenerateBarInternal();
    }
#endif

    void ClearBar()
    {
        for (int i = barTransform.childCount - 1; i >= 0; i--)
        {
            GameObject child = barTransform.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }

        regionSegments.Clear();
    }

    // ============================
    // BAR GENERATION
    // ============================

    void PositionBarInBottomRight()
    {
        barTransform.anchorMin = new Vector2(1, 0);
        barTransform.anchorMax = new Vector2(1, 0);
        barTransform.pivot = new Vector2(1, 0);
        barTransform.anchoredPosition = Vector2.zero;
        barTransform.sizeDelta = new Vector2(totalWidth, totalHeight);
    }

    void GenerateBarInternal()
    {
        if (regionColors.Count == 0)
            return;

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

            ApplyOutlineShadow(segment);

            regionSegments.Add(rt);
        }
    }

    // ============================
    // ARROW
    // ============================

    void UpdateArrowPosition()
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
        float arrowY = barTransform.anchoredPosition.y + totalHeight + arrowHeight;

        arrowTransform.anchoredPosition = new Vector2(arrowX, arrowY);
    }

    // ============================
    // UI UTILITIES
    // ============================

    void ApplyOutlineShadow(GameObject target)
    {
        Shadow shadow = target.GetComponent<Shadow>();
        if (shadow)
        {
            if (Application.isPlaying)
                Destroy(shadow);
            else
                DestroyImmediate(shadow);
        }

        Outline outline = target.GetComponent<Outline>();
        if (!outline)
            outline = target.AddComponent<Outline>();

        outline.effectColor = shadowColor;
        outline.effectDistance = shadowDistance;
        outline.useGraphicAlpha = true;
    }

    Color GetUIColor(GameColor color)
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

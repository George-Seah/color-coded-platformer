using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicColorBar : MonoBehaviour
{
    public RectTransform barTransform; // Reference to the RectTransform of the bar
    public RectTransform arrowTransform; // Reference to the arrow indicator
    public List<string> colorTags = new List<string> { "Default", "Red", "Green", "Blue" }; // Color tags
    public float totalWidth = 500f; // Total width of the bar (in pixels)
    public float totalHeight = 50f; // Height of the bar
    public int numberOfRegions; // Number of regions
    public PlayerBehaviour playerBehaviour; // Reference to player
    public float startX; // Starting position for regions
    public float regionWidth; // Width of each region

    private List<RectTransform> regionSegments = new List<RectTransform>();

    void Start()
    {
        GenerateBar();
    }

    void Update()
    {
        UpdateArrowPosition();
    }

    private void GenerateBar()
    {
        // Clear existing segments
        foreach (Transform child in barTransform)
        {
            Destroy(child.gameObject);
        }

        // Calculate size of each region
        float regionSize = totalWidth / numberOfRegions;

        // Generate regions
        for (int i = 0; i < numberOfRegions; i++)
        {
            // Create region segment
            GameObject segment = new GameObject("Region " + i, typeof(Image));
            segment.transform.SetParent(barTransform);

            // Configure RectTransform
            RectTransform segmentTransform = segment.GetComponent<RectTransform>();
            segmentTransform.anchorMin = new Vector2(0, 0);
            segmentTransform.anchorMax = new Vector2(0, 1);
            segmentTransform.pivot = new Vector2(0, 0.5f);
            segmentTransform.sizeDelta = new Vector2(regionSize, totalHeight);
            segmentTransform.anchoredPosition = new Vector2(i * regionSize, 0);

            // Set color
            Image segmentImage = segment.GetComponent<Image>();
            string colorTag = colorTags[i % colorTags.Count];
            segmentImage.color = GetColorFromTag(colorTag);

            // Save reference
            regionSegments.Add(segmentTransform);
        }
    }

    private void UpdateArrowPosition()
    {
        if (!playerBehaviour) return;

        // Determine player's current region
        float playerPositionX = playerBehaviour.transform.position.x;
        int regionIndex = Mathf.FloorToInt((playerPositionX - startX) / regionWidth);

        // Clamp to valid region index
        regionIndex = Mathf.Clamp(regionIndex, 0, numberOfRegions - 1);

        // Update arrow position
        float arrowX = regionSegments[regionIndex].anchoredPosition.x + regionWidth / 2;
        arrowTransform.anchoredPosition = new Vector2(arrowX, arrowTransform.anchoredPosition.y);
    }

    private Color GetColorFromTag(string tag)
    {
        switch (tag)
        {
            case "Red": return Color.red;
            case "Green": return Color.green;
            case "Blue": return Color.blue;
            default: return Color.white; // Default
        }
    }
}
/*
using UnityEngine;
using System.Collections.Generic;

public class PositionBasedColorChanger : MonoBehaviour
{
    public PlayerBehaviour playerBehaviour; // Reference to the PlayerBehaviour script
    public List<string> colorTags = new List<string> { "Default", "Red", "Green", "Blue" }; // Color tags that match layers
    public float startX; // Starting x value for color changes
    public float startY; // Starting y value for color changes
    public float regionWidth; // Width of each region for color changes
    public bool useXCoordinate = true; // Toggle between using X or Y coordinates

    private int lastRegionIndex = -1; // Tracks the last region the player was in

    void Update()
    {
        // Get the player's current position
        float playerPosition = useXCoordinate ? playerBehaviour.transform.position.x : playerBehaviour.transform.position.y;

        // Calculate the current region index based on the player's position
        float startPosition = useXCoordinate ? startX : startY;
        int currentRegionIndex = Mathf.FloorToInt((playerPosition - startPosition) / regionWidth);

        // If the player enters a new region, update the color
        if (currentRegionIndex != lastRegionIndex)
        {
            ChangeColors(currentRegionIndex);
            lastRegionIndex = currentRegionIndex; // Update the last region index
        }
    }

    private void ChangeColors(int regionIndex)
    {
        // Handle index wrapping for out-of-range regions
        int colorIndex = Mathf.Abs(regionIndex) % colorTags.Count;

        // Get the tag and color for the region
        string currentTag = colorTags[colorIndex];
        Color currentColor = GetColorFromTag(currentTag);

        // Apply the current color to all objects with the ColorPositionChange script
        ColorPositionChange[] colorChangers = FindObjectsOfType<ColorPositionChange>();
        foreach (var changer in colorChangers)
        {
            changer.SetColorAndTag(currentColor, currentTag);
        }
    }

    private Color GetColorFromTag(string tag)
    {
        switch (tag)
        {
            case "Red": return Color.red;
            case "Green": return Color.green;
            case "Blue": return Color.blue;
            default: return Color.white; // Default
        }
    }
}


*/
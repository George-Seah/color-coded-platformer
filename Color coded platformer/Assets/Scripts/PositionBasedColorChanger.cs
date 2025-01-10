using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicColorBar : MonoBehaviour
{
    public RectTransform barTransform; // Reference to the RectTransform of the bar
    public RectTransform arrowTransform; // Reference to the arrow indicator
    public List<string> colorTags = new List<string> { "Default", "Red", "Green", "Blue" }; // Color tags
    public float totalWidth = 250f; // Total width of the bar (in pixels)
    public float totalHeight = 30f; // Height of the bar
    private int numberOfRegions; // Number of regions
    public PlayerBehaviour playerBehaviour; // Reference to player
    public float startX; // Starting position for regions
    public float startY; // Starting position for regions
    public float regionWidth; // Width of each region
    public bool useXCoordinate = true; // Toggle between using X or Y coordinates

    private List<RectTransform> regionSegments = new List<RectTransform>();
    private int lastRegionIndex = -1; // Tracks the last region the player was in

    void Start()
    {
        numberOfRegions = colorTags.Count;
        PositionBarInBottomRight();
        GenerateBar();
    }

    void Update()
    {
        UpdateArrowPosition();
        UpdatePlayerRegionColor();
    }

    private void PositionBarInBottomRight()
    {
        // Set anchors to the bottom-right corner
        barTransform.anchorMin = new Vector2(1, 0); // Bottom-right
        barTransform.anchorMax = new Vector2(1, 0); // Bottom-right

        // Set pivot to the bottom-right
        barTransform.pivot = new Vector2(1, 0); // Bottom-right

        // Set the position relative to the bottom-right corner
        barTransform.anchoredPosition = new Vector2(0f, 0f); // Adjust margin as needed (e.g., -10px inward on X, 10px upward on Y)

        // Set size to the specified width and height
        barTransform.sizeDelta = new Vector2(totalWidth, totalHeight);
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
            segmentTransform.anchorMin = new Vector2(0, 0); // Anchor to the bottom
            segmentTransform.anchorMax = new Vector2(0, 1); // Anchor to the top
            segmentTransform.pivot = new Vector2(0, 0.5f); // Center vertically on the Y-axis
            segmentTransform.sizeDelta = new Vector2(regionSize, totalHeight); // Set the size to match the bar

            // Position segments from left to right, adjusting their X based on region index
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

    // Get the player's position (X or Y based on the setting)
    float playerPosition = useXCoordinate ? playerBehaviour.transform.position.x : playerBehaviour.transform.position.y;

    // Calculate the player's position relative to the start of the bar
    float playerRelativePosition = (playerPosition - startX) / regionWidth;

    // Normalize the player's relative position to the number of regions
    playerRelativePosition = Mathf.Repeat(playerRelativePosition, numberOfRegions); // This ensures it loops between 0 and 1

    // Map the player's relative position to the bar's total width (from 0 to totalWidth)
    float arrowX = ((playerRelativePosition * totalWidth) / numberOfRegions) - totalWidth; // No subtraction, just directly scale to total width

    // Dynamically adjust the arrow's Y position to be on top of the bar and account for its height
    float arrowHeight = arrowTransform.rect.height; // Get the height of the arrow
    float arrowY = barTransform.position.y + (totalHeight / 2) + (arrowHeight); // Place the arrow above the bar

    // Set the arrow's anchored position
    arrowTransform.anchoredPosition = new Vector2(arrowX, arrowY);
}





    private void UpdatePlayerRegionColor()
    {
        if (!playerBehaviour) return;

        // Get the player's position (X or Y based on the setting)
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
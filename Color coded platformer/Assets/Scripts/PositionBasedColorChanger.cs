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

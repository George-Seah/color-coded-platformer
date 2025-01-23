using UnityEngine;

public class ColorPositionChangeOffset : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PositionBasedColorChanger PBCC;

    [SerializeField] private int colorOffset = 0; // Offset value for color selection

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Try to find the PositionBasedColorChanger object by name
        PBCC = GameObject.Find("Position Based Color Changer").GetComponent<PositionBasedColorChanger>();

        // Check if PBCC is found
        if (PBCC != null)
        {
            Debug.Log("PositionBasedColorChanger found! Number of tags: " + PBCC.colorTags.Count);

            if (PBCC.colorTags.Count > 0)
            {
                UpdateColor();
            }
            else
            {
                Debug.LogError("No color tags found in PositionBasedColorChanger.");
            }
        }
        else
        {
            Debug.LogError("PositionBasedColorChanger not found!");
        }
    }

    public void UpdateColor()
    {
        if (PBCC == null) return;

        float playerPosition = PBCC.useXCoordinate ? PBCC.playerBehaviour.transform.position.x : PBCC.playerBehaviour.transform.position.y;
        int currentRegionIndex = Mathf.FloorToInt((playerPosition - PBCC.startX) / PBCC.regionWidth);

        // Apply the offset while ensuring wrapping within bounds
        int adjustedIndex = (currentRegionIndex + colorOffset + PBCC.colorTags.Count) % PBCC.colorTags.Count;

        string currentTag = PBCC.colorTags[adjustedIndex];
        Color currentColor = PBCC.GetColorFromTag(currentTag);

        SetColorAndTag(currentColor, currentTag);
    }

    public void SetColorAndTag(Color color, string tag)
    {
        Debug.Log("Setting color: " + color + " and tag: " + tag);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        else
        {
            Debug.LogError("SpriteRenderer is not found.");
        }

        gameObject.layer = LayerMask.NameToLayer(tag);
    }

    void Update()
    {
        UpdateColor();
    }
}

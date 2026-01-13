using UnityEngine;

public class ColorPositionChange : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [Header("Layer Control")]
    [SerializeField] bool changeLayer = true;

    [Header("Particle Settings")]
    ParticleSystem particleSystem;
    [SerializeField] Color particleColor = Color.white;
    [SerializeField] float startSize = 0.2f;
    [SerializeField] float startSpeed = 1f;
    [SerializeField] float particleSystemScale = 0.5f;

    [Header("Offset Settings")]
    [SerializeField] int colorOffset = 0;

    int lastRegionIndex = int.MinValue;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (PositionBasedColorChanger.Instance == null) Debug.LogError("No PositionBasedColorChanger found in scene.");

        colorOffset %= PositionBasedColorChanger.Instance.regionColors.Count;
        // Set initial particle color based on offset
        particleColor = colorOffset != 0 ? Color.black : Color.white;

        AddSparkles();
    }
    void Update()
    {
        UpdateColor();
    }

    /// <summary>
    /// REQUIRED BY Cannon.cs — RESTORED.
    /// </summary>
    public void SetOffset(int offset)
    {
        colorOffset = offset;
        particleColor = colorOffset != 0 ? Color.black : Color.white;

        if (particleSystem != null)
        {
            var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            renderer.material.color = particleColor;

            var main = particleSystem.main;
            main.startColor = particleColor;
        }
    }

    void UpdateColor()
    {
        if (PositionBasedColorChanger.Instance == null) return;

        // Determine player's current region
        float position = PositionBasedColorChanger.Instance.useXCoordinate
            ? PositionBasedColorChanger.Instance.playerBehaviour.transform.position.x
            : PositionBasedColorChanger.Instance.playerBehaviour.transform.position.y;

        float start = PositionBasedColorChanger.Instance.useXCoordinate
            ? PositionBasedColorChanger.Instance.startX
            : PositionBasedColorChanger.Instance.startY;

        int regionIndex = Mathf.FloorToInt((position - start) / PositionBasedColorChanger.Instance.regionWidth);
        regionIndex += colorOffset;

        if (regionIndex == lastRegionIndex) return;
        lastRegionIndex = regionIndex;

        int wrappedIndex = ((regionIndex % PositionBasedColorChanger.Instance.regionColors.Count)
            + PositionBasedColorChanger.Instance.regionColors.Count)
            % PositionBasedColorChanger.Instance.regionColors.Count;

        var currentColor = PositionBasedColorChanger.Instance.regionColors[wrappedIndex];

        SetColorAndLayer(currentColor);
    }

    void SetColorAndLayer(GameColor gameColor)
    {
        Color unityColor = gameColor switch
        {
            GameColor.Red => Color.red,
            GameColor.Green => Color.green,
            GameColor.Blue => Color.blue,
            _ => Color.white
        };

        if (spriteRenderer != null)
            spriteRenderer.color = unityColor;

        if (changeLayer)
        {
            string layerName = gameColor.ToString();
            gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    // ===============================
    // ORIGINAL PARTICLE EFFECT SYSTEM
    // ===============================
    void AddSparkles()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();

        if (particleSystem == null)
        {
            particleSystem = new GameObject("Sparkles").AddComponent<ParticleSystem>();
            particleSystem.transform.SetParent(transform);
            particleSystem.transform.localPosition = Vector3.zero;
        }

        particleSystem.transform.localScale =
            new Vector3(particleSystemScale, particleSystemScale, particleSystemScale);

        ParticleSystem.MainModule main = particleSystem.main;
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        ParticleSystem.ShapeModule shape = particleSystem.shape;

        main.startSize = startSize;
        main.startLifetime = 1.0f;
        main.startSpeed = startSpeed;
        main.startColor = particleColor;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        emission.rateOverTime = 10f;

        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.2f;

        ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.material.color = particleColor;
        renderer.sortingLayerName = "Default";
    }
}

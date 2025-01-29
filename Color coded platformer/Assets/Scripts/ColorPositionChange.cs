using UnityEngine;

public class ColorPositionChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PositionBasedColorChanger PBCC;
    [SerializeField] private int colorOffset = 0; // Offset value for color selection
    [SerializeField] private bool changeLayer = true; // Scale of the ParticleSystem itself (to adjust the overall size of the particles)
    private ParticleSystem particleSystem;
    [SerializeField] private Color particleColor = Color.white; // Scale of the ParticleSystem itself (to adjust the overall size of the particles)
    [SerializeField] private float startSize = 0.2f; // Size of individual particles
    [SerializeField] private float startSpeed = 1f; // Speed of individual particles
    [SerializeField] private float particleSystemScale = 0.5f; // Scale of the ParticleSystem itself (to adjust the overall size of the particles)
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(colorOffset != 0){
            particleColor = Color.black;
        }
        else{
            particleColor = Color.white;
        }
        AddSparkles();
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
        if(changeLayer){
            gameObject.layer = LayerMask.NameToLayer(tag);
        }

    }
    public void SetOffset(int offset){
        colorOffset = offset;
        if(colorOffset != 0){
            particleColor = Color.black;
        }
        else{
            particleColor = Color.white;
        }
        particleSystem.GetComponent<ParticleSystemRenderer>().material.color = particleColor;
    }
    void Update()
    {
        UpdateColor();
    }
    void AddSparkles()
    {
        // Check if the GameObject already has a ParticleSystem component
        particleSystem = GetComponentInChildren<ParticleSystem>();

        if (particleSystem == null)
        {
            // Add a ParticleSystem if it doesn't already exist
            particleSystem = new GameObject("Sparkles").AddComponent<ParticleSystem>();
            particleSystem.transform.SetParent(transform);  // Make the particle system a child of the original GameObject
            particleSystem.transform.localPosition = Vector3.zero; // Center the particle system on the GameObject
        }

        // Adjust the scale of the ParticleSystem (do NOT scale the GameObject itself)
        particleSystem.transform.localScale = new Vector3(particleSystemScale, particleSystemScale, particleSystemScale); 

        // Configure the ParticleSystem
        ParticleSystem.MainModule main = particleSystem.main;
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        ParticleSystem.ShapeModule shape = particleSystem.shape;

        // Adjust the particle size using the startSize (to control individual particles)
        main.startSize = startSize;

        // Set particle system properties
        main.startLifetime = 1.0f;
        main.startSpeed = startSpeed;
        main.startColor = particleColor;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        emission.rateOverTime = 10f;
        shape.shapeType = ParticleSystemShapeType.Circle;

        // Adjust the radius of the emission area (this controls how far apart particles are spread)
        shape.radius = 0.2f; // Make it smaller to avoid spread being too wide

        ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.material.color = particleColor;
        renderer.sortingLayerName = "Default";
    }
}

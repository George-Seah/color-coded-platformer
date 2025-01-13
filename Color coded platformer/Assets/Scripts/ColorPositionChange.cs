using UnityEngine;

public class ColorPositionChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PositionBasedColorChanger PBCC;
    
    [SerializeField] private float startSize = 0.2f; // Size of individual particles
    [SerializeField] private float startSpeed = 1f; // Speed of individual particles
    [SerializeField] private float particleSystemScale = 0.5f; // Scale of the ParticleSystem itself (to adjust the overall size of the particles)

    private ParticleSystem particleSystem;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AddSparkles();

        // Try to find the PositionBasedColorChanger object by name
        PBCC = GameObject.Find("Position Based Color Changer").GetComponent<PositionBasedColorChanger>();

        // Check if PBCC is found
        if (PBCC != null)
        {
            // Debug log to check if PBCC colorTags list is being populated
            Debug.Log("PositionBasedColorChanger found! Number of tags: " + PBCC.colorTags.Count);

            // Make sure there are color tags in PBCC before using them
            if (PBCC.colorTags.Count > 0)
            {
                // Calculate which region the player is in (using player position and region width)
                float playerPosition = PBCC.useXCoordinate ? PBCC.playerBehaviour.transform.position.x : PBCC.playerBehaviour.transform.position.y;
                int currentRegionIndex = Mathf.FloorToInt((playerPosition - PBCC.startX) / PBCC.regionWidth);

                // Wrap the index to stay within colorTags bounds
                currentRegionIndex = Mathf.Abs(currentRegionIndex) % PBCC.colorTags.Count;

                // Get the color and tag for the region based on the player's position
                string currentTag = PBCC.colorTags[currentRegionIndex];
                Color currentColor = PBCC.GetColorFromTag(currentTag);

                // Set the color and tag immediately
                SetColorAndTag(currentColor, currentTag);
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

    public void SetColorAndTag(Color color, string tag)
    {
        // Debug log to check if the color and tag are being set
        Debug.Log("Setting color: " + color + " and tag: " + tag);

        // Update the color of the SpriteRenderer
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        else
        {
            Debug.LogError("SpriteRenderer is not found.");
        }

        // Update the GameObject's layer tag
        gameObject.layer = LayerMask.NameToLayer(tag); // Ensure layers are correctly set up in Unity
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
        main.startColor = Color.white;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        emission.rateOverTime = 10f;
        shape.shapeType = ParticleSystemShapeType.Circle;

        // Adjust the radius of the emission area (this controls how far apart particles are spread)
        shape.radius = 0.2f; // Make it smaller to avoid spread being too wide

        ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.material.color = Color.white;
        renderer.sortingLayerName = "Default";
    }
}

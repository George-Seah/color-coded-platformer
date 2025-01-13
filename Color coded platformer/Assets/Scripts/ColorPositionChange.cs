using UnityEngine;

public class ColorPositionChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AddSparkles(gameObject);
    }

    public void SetColorAndTag(Color color, string tag)
    {
        // Update the color of the SpriteRenderer
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }

        // Update the GameObject's layer tag
        //gameObject.tag = tag;
        gameObject.layer = LayerMask.NameToLayer(tag); // Ensure layers are correctly set up in Unity
    }

void AddSparkles(GameObject target)
{
    // Check if the target already has a ParticleSystem component
    ParticleSystem particleSystem = target.GetComponent<ParticleSystem>();

    if (particleSystem == null)
    {
        // Add a ParticleSystem if it doesn't already exist
        particleSystem = target.AddComponent<ParticleSystem>();
    }

    // Configure the ParticleSystem
    ParticleSystem.MainModule main = particleSystem.main;
    ParticleSystem.EmissionModule emission = particleSystem.emission;
    ParticleSystem.ShapeModule shape = particleSystem.shape;

    main.startSize = 0.2f;
    main.startLifetime = 1.0f;
    main.startSpeed = 1f;
    main.startColor = Color.white;
    main.simulationSpace = ParticleSystemSimulationSpace.World;

    emission.rateOverTime = 10f;
    shape.shapeType = ParticleSystemShapeType.Circle;
    shape.radius = 0.5f;

    ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
    renderer.material = new Material(Shader.Find("Sprites/Default"));
    renderer.material.color = Color.white;
    renderer.sortingLayerName = "Default";
}

}

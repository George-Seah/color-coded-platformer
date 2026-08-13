using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] ESave saveManager;
    [SerializeField] string index;

    bool hadCPC;
    bool hadParticles;

    ColorPositionChange cpc;
    ParticleSystem ps;
    SpriteRenderer sprite;

    public string Index => index;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        cpc = GetComponent<ColorPositionChange>();
        ps = GetComponentInChildren<ParticleSystem>();

        hadCPC = (cpc != null);
        hadParticles = (ps != null);
    }

    void Start()
    {
        if (saveManager == null)
            saveManager = Object.FindAnyObjectByType<ESave>();

        if (saveManager != null && saveManager.LoadStar(index))
            ApplyCollectedVisuals();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && saveManager != null)
        {
            saveManager.SaveStar(index, true);
            ApplyCollectedVisuals();
        }
    }

    void ApplyCollectedVisuals()
    {
        if (sprite != null)
            sprite.color = Color.black;

        if (cpc != null)
            Destroy(cpc);

        if (ps != null)
            Destroy(ps);
    }

    public void ResetStarVisuals()
    {
        if (sprite != null)
            sprite.color = Color.white;

        if (hadCPC && GetComponent<ColorPositionChange>() == null)
            gameObject.AddComponent<ColorPositionChange>();

        // Particles: you can add prefab respawn if needed
    }

    [ContextMenu("Reset This Star Data")]
    void ResetThisStar()
    {
        if (saveManager != null)
        {
            saveManager.ResetStar(index);
            ResetStarVisuals();
        }
    }
}
